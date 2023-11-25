using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    [Header("Menues")]
    public GameObject teamMenu;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject gameScene;
    public GameObject pauseMenu;
    public GameObject scoreScene;
    public GameObject winnerScene;

    [Header("Sliders")]
    public Slider passSlider;
    public Slider timeSlider;
    public Slider roundSlider;

    [Header("Text Value")]
    public TextMeshProUGUI passText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI roundText;

    [Header("Team Name Texts")]
    public TextMeshProUGUI team1;
    public TextMeshProUGUI team2;

    [Header("GameUI")]
    public TextMeshProUGUI passButton;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI roundText2;
    public Image timeImage;
    public TextMeshProUGUI movieText;
    public TextMeshProUGUI kindText;

    [Header("Score Scene UI")]
    public TextMeshProUGUI tag1;
    public TextMeshProUGUI tag2;
    public TextMeshProUGUI score1;
    public TextMeshProUGUI score2;


    [Header("Winner Scene UI")]
    public Text team1FinalScore;
    public Text team2FinalScore;
    public Text winnerName;
    public GameObject winnerEffect;



    int pass,tempPass;
    int maxTime,tempTime;
    int round, tempRound, maxRound;
    int filmID;
    int team1Score, team2Score;

    string team1name, team2name;

    bool readyToPlay;
    bool isSettinsMenuOpen;
    bool stopCountDown;

    Movies movies;

    


    private void Start()
    {
        movies = GetComponent<Movies>();

        tempRound = 1;
        round = 1;
        maxRound = 10;

        ChangeMovie();

        pass = 3;
        tempTime = 60;
        maxTime = 60;

    }
    private void Update()
    {
        MenuSettings();
        EndOfTour();
        EndOfTheGame();
    }




    void MenuSettings()
    {
        if(isSettinsMenuOpen==true)
        {
            // Pass Settings
            pass = (int)passSlider.value;
            passText.text = pass.ToString();
            tempPass = pass;

            // Time Settings
            maxTime = (int)timeSlider.value;
            timeText.text = maxTime.ToString();
            tempTime = maxTime;

            // Round Settings
            maxRound = (int)roundSlider.value;
            roundText.text = maxRound.ToString();
        }

        passButton.text = "Pas(" + tempPass + ")";
        roundText2.text = "Round : " + round;
        timerText.text = tempTime.ToString();


    }
    void ChangeMovie()
    {
        filmID = Random.Range(0, (movies.filmler.Length)/2);
        movieText.text = movies.filmler[filmID, 0];
        kindText.text = movies.filmler[filmID, 1];
    }
    void EndOfTour()
    {
        if (tempTime < 0 && readyToPlay==true && round<=maxRound)
        {
            score1.text = team1Score.ToString();
            score2.text = team2Score.ToString();
            scoreScene.SetActive(true);
            gameScene.SetActive(false);
            tempRound++;
            readyToPlay = false;
            tempTime = maxTime;
        }

        if ((tempRound==3))
        {
            round++;
            tempRound = 1;
        }
            
    }

    void EndOfTheGame()
    {
        if(round>maxRound)
        {
            gameScene.SetActive(false);
            winnerScene.SetActive(true);
            scoreScene.SetActive(false);

            

            team1FinalScore.text = team1name + " : " + team1Score.ToString();
            team2FinalScore.text = team2name + " : " + team2Score.ToString();

            if (team1Score > team2Score)
                winnerName.text = team1name;
            else
                winnerName.text = team2name;
        }

        winnerEffect.transform.Rotate(0, 0, -20f*Time.deltaTime);
    }



    public void OpenTeamMenu()
    {
        teamMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        teamMenu.SetActive(false);
    }
    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
        isSettinsMenuOpen = true;
    }
    public void StartGame()
    {

            gameScene.SetActive(true);
            teamMenu.SetActive(false);
            scoreScene.SetActive(false);

            stopCountDown = false;
            readyToPlay = true;
            tempPass = pass;

            team1name = team1.text;
            team2name = team2.text;

            tag1.text = team1name;
            tag2.text = team2name;

            StartCoroutine(CountDown());


    }
    public void BackToSettingsMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        isSettinsMenuOpen = false;
    }
    public void StopGame()
    {
        pauseMenu.SetActive(true);
        gameScene.SetActive(false);
        Time.timeScale = 0;
    }


    public void ContinueButton()
    {
        ChangeMovie();

        if(tempRound%2==1)
            team1Score++;
        
        else if (tempRound % 2 ==0)
            team2Score++;
        
    }
    public void PassButton()
    {
        if(tempPass >0)
        {
            ChangeMovie();
            tempPass--;
        }
    }


    public void ResumeButton()
    {
        pauseMenu.SetActive(false);
        gameScene.SetActive(true);
        Time.timeScale = 1;
    }
    public void RestartButton()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        gameScene.SetActive(true);
        tempTime = maxTime;
        tempRound = 1;
        round = 1;
        tempPass = pass;
        team1Score = 0;
        team2Score = 0;
        ChangeMovie();
    }
    public void MainMenuButton()
    {
        StopCoroutine(CountDown());
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);
        winnerScene.SetActive(false);
        tempTime = maxTime+1;
        tempRound = 1;
        round = 1;
        team1Score = 0;
        team2Score = 0;
        stopCountDown = true;
        
    }

    public void ExitButton()
    {
        Application.Quit();
    }
    
    IEnumerator CountDown()
    {
        while(tempTime>=0)
        {
            if(readyToPlay == true)
            {
                timeImage.fillAmount = Mathf.InverseLerp(0, maxTime, tempTime);
                yield return new WaitForSeconds(1.0f);
                tempTime--;

                if(tempTime == 0)
                {
                    tempTime--;
                    break;
                }
                if(stopCountDown==true)
                {
                    break; 
                }
            }
            
            yield return null;
        }

        yield return null;
    }

}
