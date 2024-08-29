using chayeong;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NumberQuizGame
{
    public class StartCanvas : MonoBehaviour
    {
        [HideInInspector] public NumberQuizGame numberQuizGame;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            numberQuizGame = GameObject.Find("NumberQuizGame").GetComponent<NumberQuizGame>();
        }

        private void OnEnable()
        {
            // numberQuizGame.soundManager.PlayClip(0);
        }

        public void GameStart()
        {
            animator.SetTrigger("Start");
            gameObject.SetActive(false);
            numberQuizGame.readyCanvas.SetActive(true);
        }

        public void ExitGame()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}