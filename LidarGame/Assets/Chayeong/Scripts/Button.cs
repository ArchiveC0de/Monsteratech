using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace chayeong
{
    public class Button : MonoBehaviour
    {

        public Canvas HowTo;

        public void OnStartButton()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void OnHowToPlayButton()
        {
            HowTo.enabled = true;
        }

        public void OnClosedButton()
        {
            SceneManager.LoadScene("MainScene");
        }
    }

}
