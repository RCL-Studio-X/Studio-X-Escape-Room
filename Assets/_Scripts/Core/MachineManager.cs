using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Manages the machine logic for evaluating team cards and displaying results.
    /// </summary>
    public class MachineManager : MonoBehaviour
    {
        /// <summary>
        /// Tracks the progress and entered cards for a specific team.
        /// </summary>
        [System.Serializable]
        public class TeamTracker
        {
            [Tooltip("The number of cards required for this team.")]
            public int requiredCards;

            [Tooltip("The message or object revealed when all team's members are correctly entered.")]
            public GameObject hiddenMessage;

            [HideInInspector]
            [Tooltip("Indicates whether this team has successfully met their card requirement.")]
            public bool isCompleted = false;

            [HideInInspector]
            [Tooltip("The current number of correct cards entered for this team.")]
            public int currentCards = 0;

            [HideInInspector]
            [Tooltip("The total number of cards attempted for this team.")]
            public int totalAttempts = 0;

            [HideInInspector]
            [Tooltip("List of card objects that have been entered for this team.")]
            public List<GameObject> enteredCards = new List<GameObject>();
        }

        #region Public Variable declarations

        [Header("UI & Setup")]
        [Tooltip("Transform position where incorrect cards are ejected.")]
        public Transform outputPos;

        [Tooltip("Indicator object that changes color based on result.")]
        public GameObject resultIndicator;

        [Tooltip("The UI or object representing the currently selected team.")]
        public GameObject teamSelected;

        [Header("Interaction")]
        [Tooltip("The XR socket interactor for receiving cards.")]
        public XRSocketInteractor socket;

        [Header("Team Data")]
        [Tooltip("Array of trackers containing data for each team.")]
        public TeamTracker[] teams;

        [Header("Unity Events")]
        [Tooltip("Event invoked when the current team is completed.")]
        public UnityEngine.Events.UnityEvent onTeamCompleted;

        [Tooltip("Event invoked when a team submits an incorrect set of cards.")]
        public UnityEngine.Events.UnityEvent onIncorrectSubmission;

        #endregion

        #region Private Variable declarations

        /// <summary> The original base color of the result indicator. </summary>
        private Color _defaultIndicatorColor;

        /// <summary> How much the dial rotates to select the next team. </summary>
        private float _dialRotation = 45f;

        /// <summary> The index of the currently selected team. </summary>
        private int _currentTeamIndex = 0;

        /// <summary> Gets the Renderer of the result indicator. </summary>
        private Renderer _indicatorRenderer;

        /// <summary> Delay in seconds between each card ejection to prevent physics collisions. </summary>
        private const float CARD_EJECT_DELAY = 0.3f;

        #endregion

        #region Public Method definitions

        /// <summary>
        /// Cycles the machine selection to the next team.
        /// </summary>
        public void NextTeam()
        {
            if (_currentTeamIndex == teams.Length - 1)
            {
                _currentTeamIndex = 0;
            }
            else
            {
                _currentTeamIndex++;
            }
            float targetXAngle = -220f + (_currentTeamIndex * _dialRotation);
            teamSelected.transform.localEulerAngles = new Vector3(targetXAngle, 90f, -90f);
        }

        /// <summary>
        /// Cycles the machine selection to the previous team.
        /// </summary>
        public void PreviousTeam()
        {
            if (_currentTeamIndex == 0)
            {
                _currentTeamIndex = teams.Length - 1;
            }
            else
            {
                _currentTeamIndex--;
            }
            float targetXAngle = -220f + (_currentTeamIndex * _dialRotation);
            teamSelected.transform.localEulerAngles = new Vector3(targetXAngle, 90f, -90f);
        }

        /// <summary>
        /// Checks the card currently placed in the socket and processes it for the active team.
        /// </summary>
        public void CheckCard()
        {
            if (socket.hasSelection)
            {
                GameObject cardInSocket = socket.GetOldestInteractableSelected().transform.gameObject;

                // Array Safety Check for blank sides that eject the card
                if (_currentTeamIndex >= teams.Length)
                {
                    EjectCard(cardInSocket);
                    return;
                }

                TeamTracker currentTeam = teams[_currentTeamIndex];

                // If team is already complete, eject the card
                if (currentTeam.isCompleted)
                {
                    EjectCard(cardInSocket);
                    return;
                }

                currentTeam.totalAttempts++;
                currentTeam.enteredCards.Add(cardInSocket);

                CardData socketedCardData = cardInSocket.GetComponent<CardData>();

                if (socketedCardData != null)
                {
                    if (socketedCardData.targetTeamIndex == _currentTeamIndex)
                    {
                        currentTeam.currentCards++;
                    }
                }
                else
                {
                    Debug.LogWarning("The object in the socket is missing a CardData script: " + cardInSocket.name);
                }

                // Clear the socket for the next card
                cardInSocket.SetActive(false);
            }
        }

        /// <summary>
        /// Evaluates the current team's submitted cards to determine if the puzzle is solved.
        /// </summary>
        public void OnSubmitPressed()
        {
            if (_currentTeamIndex >= teams.Length) return;

            TeamTracker currentTeam = teams[_currentTeamIndex];

            if (currentTeam.isCompleted)
            {
                onTeamCompleted?.Invoke();
                StartCoroutine(FlashIndicator(Color.green));
                return;
            }

            // Check if they got the right amount of cards, with no extra wrong guesses
            if (currentTeam.currentCards == currentTeam.requiredCards && currentTeam.totalAttempts == currentTeam.requiredCards)
            {
                currentTeam.isCompleted = true;
                onTeamCompleted?.Invoke();
                StartCoroutine(FlashIndicator(Color.green));
                currentTeam.hiddenMessage.SetActive(true);
            }
            else
            {
                onIncorrectSubmission?.Invoke();
                ResetCurrentTeam();
                StartCoroutine(FlashIndicator(Color.red));
            }
        }

        /// <summary>
        /// Resets the current team's progress and ejects their entered cards one at a time.
        /// </summary>
        public void ResetCurrentTeam()
        {
            if (_currentTeamIndex >= teams.Length) return;

            TeamTracker currentTeam = teams[_currentTeamIndex];
            if (!currentTeam.isCompleted)
            {
                StartCoroutine(EjectCardsSequentially(currentTeam));
            }
        }

        #endregion

        #region Private Method definitions

        /// <summary>
        /// Initializes default components on startup.
        /// </summary>
        private void Start()
        {
            //dialRotation = 360f/teams.Length;

            _indicatorRenderer = resultIndicator.GetComponent<Renderer>();

            if (_indicatorRenderer != null)
            {
                _defaultIndicatorColor = _indicatorRenderer.material.color;
            }
        }

        /// <summary>
        /// Ejects each card in the team's entered list one at a time to prevent physics collisions.
        /// </summary>
        /// <param name="team">The TeamTracker whose cards should be ejected.</param>
        /// <returns>An IEnumerator for use in a Coroutine.</returns>
        private IEnumerator EjectCardsSequentially(TeamTracker team)
        {
            List<GameObject> cardsToEject = new List<GameObject>(team.enteredCards);

            team.enteredCards.Clear();
            team.currentCards = 0;
            team.totalAttempts = 0;

            foreach (GameObject card in cardsToEject)
            {
                if (card == null) continue;

                EjectCard(card);
                yield return new WaitForSeconds(CARD_EJECT_DELAY);
            }

            Debug.Log("Team Reset");
        }

        /// <summary>
        /// Flashes the result indicator a specific color for a set duration.
        /// </summary>
        /// <param name="color">The color to flash the indicator material.</param>
        /// <returns>An IEnumerator to be used in a Coroutine.</returns>
        private IEnumerator FlashIndicator(Color color)
        {
            if (_indicatorRenderer == null)
            {
                yield break;
            }

            _indicatorRenderer.material.color = color;

            yield return new WaitForSeconds(2f);

            _indicatorRenderer.material.color = _defaultIndicatorColor;
        }

        /// <summary>
        /// Ejects a card by disabling it, moving it to the output position, wiping its Rigidbody
        /// velocity to prevent physics explosions, then re-enabling it.
        /// </summary>
        /// <param name="card">The GameObject of the card to eject.</param>
        private void EjectCard(GameObject card)
        {
            card.SetActive(false);
            card.transform.position = outputPos.position;

            Rigidbody rb = card.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            card.SetActive(true);
        }

        #endregion
    }
}