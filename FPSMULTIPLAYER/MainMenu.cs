using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.BoarShroom.Prototype
{
    public class MainMenu : MonoBehaviour
    {
        public Launcher launcher;

        void Start()
        {
            Pause.paused = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void JoinMatch()
        {
            launcher.Join();
        }

        public void CreateMatch()
        {
            launcher.Create();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}