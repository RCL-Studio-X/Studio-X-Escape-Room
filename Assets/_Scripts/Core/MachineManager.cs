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

        #endregion

        #region Private Variable declarations

        /// <summary> The original base color of the result indicator. </summary>
        private Color _defaultIndicatorColor;

        /// <summary> The index of the currently selected team. </summary>
        private int _currentTeamIndex = 0;

        /// <summary> Gets the Render of the result indicator. </summary>
        private Renderer _indicatorRenderer;

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
            float targetXAngle = -90f + (_currentTeamIndex * 45f);
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
            float targetXAngle = -90f + (_currentTeamIndex * 45f);
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
                StartCoroutine(FlashIndicator(Color.green));
                return;
            }

            // Check if they got the right amount of cards, with no extra wrong guesses
            if (currentTeam.currentCards == currentTeam.requiredCards && currentTeam.totalAttempts == currentTeam.requiredCards)
            {
                currentTeam.isCompleted = true;
                StartCoroutine(FlashIndicator(Color.green));
                currentTeam.hiddenMessage.SetActive(true);
            }
            else
            {
                ResetCurrentTeam();
                StartCoroutine(FlashIndicator(Color.red));
            }
        }

        /// <summary>
        /// Resets the current team's progress and ejects their entered cards.
        /// </summary>
        public void ResetCurrentTeam()
        {
            if (_currentTeamIndex >= teams.Length) return;

            TeamTracker currentTeam = teams[_currentTeamIndex];
            if (!currentTeam.isCompleted)
            {
                foreach (GameObject card in currentTeam.enteredCards)
                {
                    if (card == null) continue;

                    card.SetActive(true);
                    card.transform.position = outputPos.position;
                }

                currentTeam.enteredCards.Clear();
                currentTeam.currentCards = 0;
                currentTeam.totalAttempts = 0;
                Debug.Log("Team Reset");
            }
        }

        #endregion

        #region Private Method definitions

        /// <summary>
        /// Initializes default components on startup.
        /// </summary>
        private void Start()
        {
            _indicatorRenderer = resultIndicator.GetComponent<Renderer>();

            if (_indicatorRenderer != null)
            {
                _defaultIndicatorColor = _indicatorRenderer.material.color;
            }
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
        /// Ejects a card by disabling it, moving it to the output position, and re-enabling it.
        /// </summary>
        /// <param name="card">The GameObject of the card to eject.</param>
        private void EjectCard(GameObject card)
        {
            card.SetActive(false);
            card.transform.position = outputPos.position;
            card.SetActive(true);
        }

        #endregion
    }
}