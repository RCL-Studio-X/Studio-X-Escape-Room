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
            [Tooltip("The number of cards required to complete this team's task.")]
            public int requiredCards;

            [Tooltip("The message or object revealed when the team's task is completed.")]
            public GameObject hiddenMessage;

            [HideInInspector]
            [Tooltip("Indicates whether this team has successfully completed their task.")]
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

        #endregion

        #region Public Method definitions

        /// <summary>
        /// Cycles the machine selection to the next team.
        /// </summary>
        public void NextTeam()
        {
            if (_currentTeamIndex == 7) // if at the last team then loop index back to the first team
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
            if (_currentTeamIndex == 0) //if at first team loop it back to the last team
            {
                _currentTeamIndex = 7;
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

                // 1. Array Safety Check for blank sides that spit the card back out
                if (_currentTeamIndex >= teams.Length)
                {
                    cardInSocket.SetActive(false);
                    cardInSocket.transform.position = outputPos.position;
                    cardInSocket.SetActive(true);
                    return;
                }

                TeamTracker currentTeam = teams[_currentTeamIndex];

                // 2. If team is already done, spit the card back out
                if (currentTeam.isCompleted)
                {
                    cardInSocket.SetActive(false);
                    cardInSocket.transform.position = outputPos.position;
                    cardInSocket.SetActive(true);
                    return;
                }

                // 3. Count the attempt and add to the List 
                currentTeam.totalAttempts++;
                currentTeam.enteredCards.Add(cardInSocket);

                // 4. Read the ID badge
                if (cardInSocket.GetComponent<CardData>().targetTeamIndex == _currentTeamIndex)
                {
                    currentTeam.currentCards++;
                }

                // 5. Clear the socket for the next card
                cardInSocket.SetActive(false);
            }
        }

        /// <summary>
        /// Evaluates the current team's submitted cards to determine if the task is complete.
        /// </summary>
        public void OnSubmitPressed()
        {
            // Safety check for blank sides
            if (_currentTeamIndex >= teams.Length) return;

            TeamTracker currentTeam = teams[_currentTeamIndex];

            if (currentTeam.isCompleted)
            {
                StartCoroutine(FlashIndicator(Color.green));
                return; // Stop checking
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

        #endregion

        #region Private Method definitions

        /// <summary>
        /// Initializes default components on startup.
        /// </summary>
        private void Start()
        {
            _defaultIndicatorColor = resultIndicator.GetComponent<Renderer>().material.color;
        }

        /// <summary>
        /// Resets the current team's progress and ejects their entered cards.
        /// </summary>
        private void ResetCurrentTeam()
        {
            // Safety check for blank sides
            if (_currentTeamIndex >= teams.Length) return;

            TeamTracker currentTeam = teams[_currentTeamIndex];

            foreach (GameObject card in currentTeam.enteredCards)
            {
                card.SetActive(true);
                card.transform.position = outputPos.position;
            }

            // Clear the memory 
            currentTeam.enteredCards.Clear();
            currentTeam.currentCards = 0;
            currentTeam.totalAttempts = 0;
            Debug.Log("Team Reset");
        }

        /// <summary>
        /// Flashes the result indicator a specific color for a set duration.
        /// </summary>
        /// <param name="color">The color to flash the indicator material.</param>
        /// <returns>An IEnumerator to be used in a Coroutine.</returns>
        private IEnumerator FlashIndicator(Color color)
        {
            Renderer indicatorRenderer = resultIndicator.GetComponent<Renderer>();

            // Change to the new color
            indicatorRenderer.material.color = color;

            // Wait for exactly 2 seconds
            yield return new WaitForSeconds(2f);

            // Change it back to the absolute default color
            indicatorRenderer.material.color = _defaultIndicatorColor;
        }

        #endregion
    }
}