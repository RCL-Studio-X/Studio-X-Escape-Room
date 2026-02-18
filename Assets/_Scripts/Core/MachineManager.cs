using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit; 
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;

public class MachineManager : MonoBehaviour
{
   [Header("UI & Setup")]
    public List<GameObject> teamNum;
    public Button nextButton;
    public Button prevButton;
    public Transform outputPos; // Where wrong cards get spit out
    public GameObject resultIndicatorWrong;
    public GameObject resultIndicatorRight;

    private Color defaultIndicatorColorRight;
    private Color defaultIndicatorColorWrong;

    [Header("Interaction")]
    public XRSocketInteractor socket;
    private int currentTeamIndex = 0;

    [Header("Team 1 Data")]
    public GameObject George1; 
    public GameObject Henry1; 
    public GameObject Tilly1;
    private bool team1Completed = false;
    private int team1cards = 0;
    private int team1TotalAttempts = 0;
    private List<GameObject> team1Entered = new List<GameObject>(); 
    public GameObject hiddenMessage1;


    [Header("Team 2 Data")]
    public GameObject Beatrice2;
    public GameObject Glady2;
    private bool team2Completed = false;
    private int team2cards = 0;
    private int team2TotalAttempts = 0;
    private List<GameObject> team2Entered = new List<GameObject>(); 
    public GameObject hiddenMessage2;


    [Header("Team 3 Data")]
    public GameObject Victoria3;
    public GameObject James3;
    public GameObject Jackie3;
    private bool team3Completed = false;
    private int team3cards = 0;
    private int team3TotalAttempts = 0;
    private List<GameObject> team3Entered = new List<GameObject>(); 
    public GameObject hiddenMessage3;


    [Header("Team 4 Data")]
    public GameObject Ken4;
    public GameObject Bob4;
    public GameObject Benjamin4;
    public GameObject Bethaine4;
    private bool team4Completed = false;
    private int team4cards = 0;
    private int team4TotalAttempts = 0;
    private List<GameObject> team4Entered = new List<GameObject>(); 
    public GameObject hiddenMessage4;


    [Header("Team 5 Data")]
    public GameObject Charles5;
    public GameObject Robert5;
    private bool team5Completed = false;
    private int team5cards = 0;
    private int team5TotalAttempts = 0;
    private List<GameObject> team5Entered = new List<GameObject>(); 
    public GameObject hiddenMessage5;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateTeamDisplay();
        defaultIndicatorColorRight = resultIndicatorRight.GetComponent<Renderer>().material.color;
        defaultIndicatorColorWrong = resultIndicatorWrong.GetComponent<Renderer>().material.color;
        
    }

    // Update is called once per frame
    public void UpdateTeamDisplay()
        {
            foreach (GameObject team in teamNum)
            {
                team.SetActive(false);
            }

            teamNum[currentTeamIndex].SetActive(true);

            prevButton.interactable = currentTeamIndex > 0;
            nextButton.interactable = currentTeamIndex < teamNum.Count - 1;
        }
    
    public void NextPage()
        {
            if (currentTeamIndex < teamNum.Count - 1)
            {
                currentTeamIndex++;
                UpdateTeamDisplay();
            }
        }

        public void PreviousPage()
        {
            if (currentTeamIndex > 0)
            {
                currentTeamIndex--;
                UpdateTeamDisplay();
            }
        }

       public void checkCard()
        {
            if (socket.hasSelection)
            {
                GameObject cardInSocket = socket.GetOldestInteractableSelected().transform.gameObject;

                if (currentTeamIndex == 0)
                {
                    if (team1Completed)
                    {
                        cardInSocket.SetActive(false); 
                        cardInSocket.transform.position = outputPos.position; 
                        cardInSocket.SetActive(true); 
                        return; 
                    }
                    // 1. Just Count Attempts
                    team1TotalAttempts++;
                    team1Entered.Add(cardInSocket);

                    // 2. Just Count Correct Cards
                    if (cardInSocket == George1 || cardInSocket == Henry1 || cardInSocket == Tilly1)
                    {
                        team1cards++;
                    }

                    // 3. Clear the socket for the next card
                    cardInSocket.SetActive(false);
                }

                if (currentTeamIndex == 1)
                {
                    if (team2Completed)
                    {
                        cardInSocket.SetActive(false); 
                        cardInSocket.transform.position = outputPos.position; 
                        cardInSocket.SetActive(true); 
                        return; 
                    }
                    // 1. Just Count Attempts
                    team2TotalAttempts++;
                    team2Entered.Add(cardInSocket);

                    // 2. Just Count Correct Cards
                    if (cardInSocket == Beatrice2 || cardInSocket == Glady2)
                    {
                        team2cards++;
                    }

                    // 3. Clear the socket for the next card
                    cardInSocket.SetActive(false);
                }

                if (currentTeamIndex == 2)
                {
                    if (team3Completed)
                    {
                        cardInSocket.SetActive(false); 
                        cardInSocket.transform.position = outputPos.position; 
                        cardInSocket.SetActive(true); 
                        return; 
                    }
                    // 1. Just Count Attempts
                    team3TotalAttempts++;
                    team3Entered.Add(cardInSocket);

                    // 2. Just Count Correct Cards
                    if (cardInSocket == Victoria3 || cardInSocket == James3 || cardInSocket == Jackie3)
                    {
                        team3cards++;
                    }

                    // 3. Clear the socket for the next card
                    cardInSocket.SetActive(false);
                }

                if (currentTeamIndex == 3)
                {
                    if (team4Completed)
                    {
                        cardInSocket.SetActive(false); 
                        cardInSocket.transform.position = outputPos.position; 
                        cardInSocket.SetActive(true); 
                        return; 
                    }
                    // 1. Just Count Attempts
                    team4TotalAttempts++;
                    team4Entered.Add(cardInSocket);

                    // 2. Just Count Correct Cards
                    if (cardInSocket == Ken4 || cardInSocket == Bob4 || cardInSocket == Benjamin4 || cardInSocket == Bethaine4)
                    {
                        team4cards++;
                    }

                    // 3. Clear the socket for the next card
                    cardInSocket.SetActive(false);
                }

                if (currentTeamIndex == 4)
                {
                    if (team5Completed)
                    {
                        cardInSocket.SetActive(false); 
                        cardInSocket.transform.position = outputPos.position; 
                        cardInSocket.SetActive(true); 
                        return; 
                    }
                    // 1. Just Count Attempts
                    team5TotalAttempts++;
                    team5Entered.Add(cardInSocket);

                    // 2. Just Count Correct Cards
                    if (cardInSocket == Charles5 || cardInSocket == Robert5)
                    {
                        team5cards++;
                    }
                    // 3. Clear the socket for the next card
                    cardInSocket.SetActive(false);
                }

            }
        }

        public void OnSubmitPressed()
        {
            // Check Team 1 Logic
            if (currentTeamIndex == 0)
            {
                if (team1Completed)
                {
                    StartCoroutine(FlashIndicatorCorrect()); // Remind them it's done
                    return; // Stop checking
                }
                else if (team1cards == 3 && team1TotalAttempts == 3)
                {
                    team1Completed = true;
                    StartCoroutine(FlashIndicatorCorrect());
                    hiddenMessage1.SetActive(true);
                }
                else
                {
                    ResetCurrentTeam(); 
                    StartCoroutine(FlashIndicatorWrong());
                }
            }

            if (currentTeamIndex == 1)
            {
                if (team2Completed)
                {
                    StartCoroutine(FlashIndicatorCorrect()); // Remind them it's done
                    return; // Stop checking
                }
                else if (team2cards == 2 && team2TotalAttempts == 2)
                {
                    team2Completed = true;
                    StartCoroutine(FlashIndicatorCorrect());
                    hiddenMessage2.SetActive(true);
                }
                else
                {
                    ResetCurrentTeam(); 
                    StartCoroutine(FlashIndicatorWrong());

                }
            }

            if (currentTeamIndex == 2)
            {
                if (team3Completed)
                {
                    StartCoroutine(FlashIndicatorCorrect()); // Remind them it's done
                    return; // Stop checking
                }
                else if (team3cards == 3 && team3TotalAttempts == 3)
                {
                    team3Completed = true;
                    StartCoroutine(FlashIndicatorCorrect());
                    hiddenMessage3.SetActive(true);
                }
                else
                {
                    ResetCurrentTeam(); 
                    StartCoroutine(FlashIndicatorWrong());

                }
            }

            if (currentTeamIndex == 3)
            {
                if (team4Completed)
                {
                    StartCoroutine(FlashIndicatorCorrect()); // Remind them it's done
                    return; // Stop checking
                }
                else if (team4cards == 4 && team4TotalAttempts == 4)
                {
                    team4Completed = true;
                    StartCoroutine(FlashIndicatorCorrect());
                    hiddenMessage4.SetActive(true);
                }
                else
                {
                    ResetCurrentTeam(); 
                    StartCoroutine(FlashIndicatorWrong());
                }
            }

            if (currentTeamIndex == 4)
            {
                if (team5Completed)
                {
                    StartCoroutine(FlashIndicatorCorrect()); // Remind them it's done
                    return; // Stop checking
                }
                else if (team5cards == 2 && team5TotalAttempts == 2)
                {
                    team5Completed = true;
                    StartCoroutine(FlashIndicatorCorrect());
                    hiddenMessage5.SetActive(true);
                }
                else
                {
                    ResetCurrentTeam(); 
                    StartCoroutine(FlashIndicatorWrong());
                }
            }
        }

       void ResetCurrentTeam()
    {
        // RESET TEAM 1
        if (currentTeamIndex == 0 && !team1Completed)
        {
            // 1. Respawn all cards entered for this team
            foreach(GameObject card in team1Entered)
            {
                card.SetActive(true);
                card.transform.position = outputPos.position + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f)); // Spit them out
            }
            // 2. Clear the memory of this team
            team1Entered.Clear();
            team1cards = 0;
            team1TotalAttempts = 0;
            Debug.Log("Team 1 Reset");
        }

        // RESET TEAM 2
        if (currentTeamIndex == 1 && !team2Completed)
        {
            foreach(GameObject card in team2Entered)
            {
                card.SetActive(true);
                card.transform.position = outputPos.position + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
            }
            team2Entered.Clear();
            team2cards = 0;
            team2TotalAttempts = 0;
        }

        // RESET TEAM 3
        if (currentTeamIndex == 2 && !team3Completed)
        {
            foreach(GameObject card in team3Entered)
            {
                card.SetActive(true);
                card.transform.position = outputPos.position + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
            }
            team3Entered.Clear();
            team3cards = 0;
            team3TotalAttempts = 0;
        }

        // RESET TEAM 4
        if (currentTeamIndex == 3 && !team4Completed)
        {
            foreach(GameObject card in team4Entered)
            {
                card.SetActive(true);
                card.transform.position = outputPos.position + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
            }
            team4Entered.Clear();
            team4cards = 0;
            team4TotalAttempts = 0;
        }

        // RESET TEAM 5
        if (currentTeamIndex == 4 && !team5Completed)
        {
            foreach(GameObject card in team5Entered)
            {
                card.SetActive(true);
                card.transform.position = outputPos.position + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
            }
            team5Entered.Clear();
            team5cards = 0;
            team5TotalAttempts = 0;
        }
    }


    private IEnumerator FlashIndicatorCorrect()
    {
        Renderer renderer = resultIndicatorRight.GetComponent<Renderer>();
        
        // Change to the new color (Green)
        renderer.material.color = Color.green;

        // Wait for exactly 2 seconds
        yield return new WaitForSeconds(2f);

        // Change it back to the absolute default color
        renderer.material.color = defaultIndicatorColorRight;
    }

      private IEnumerator FlashIndicatorWrong()
    {
        Renderer renderer = resultIndicatorWrong.GetComponent<Renderer>();
        
        // Change to the new color (Red)
        renderer.material.color = Color.red;

        // Wait for exactly 2 seconds
        yield return new WaitForSeconds(2f);

        // Change it back to the absolute default color
        renderer.material.color = defaultIndicatorColorWrong;
    }
}

