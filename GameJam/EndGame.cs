using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Com.BoarShroom.GameJam
{
    public class EndGame : MonoBehaviour
    {
        public TMP_Text highscore;
        public Manager manager;
        public AudioSource newHighScore;

        public void HighScoreSet()
        {
            if (PlayerPrefs.GetInt("HighScore") < manager.kills)
            {
                PlayerPrefs.SetInt("HighScore", manager.kills);
                highscore.text = "HighScore \n" + "!" + PlayerPrefs.GetInt("HighScore").ToString() + "!";
                newHighScore.Play();
            }
            else
            {
                highscore.text = "HighScore \n" + PlayerPrefs.GetInt("HighScore").ToString();
            }

            
    }

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
