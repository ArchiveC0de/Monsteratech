using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NumberQuizGame
{
    public class EndCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject winnerVFX;

        [SerializeField] private Vector2 leftPosition;
        [SerializeField] private Vector2 rightPosition;

        private NumberQuizGame numberQuizGame;
        private Text scoreTextLeft;
        private Text scoreTextRight;

        private Animator buttonsEndAnimator;

        private void Awake()
        {
            numberQuizGame = GameObject.Find("NumberQuizGame").GetComponent<NumberQuizGame>();
            scoreTextLeft = transform.Find("ScoreTextLeft").GetComponent<Text>();
            scoreTextRight = transform.Find("ScoreTextRight").GetComponent<Text>();
            buttonsEndAnimator = transform.Find("ButtonsEnd").GetComponent<Animator>();
        }

        private void OnEnable()
        {
            scoreTextLeft.text = numberQuizGame.scoreLeft.ToString();
            scoreTextRight.text = numberQuizGame.scoreRight.ToString();

            if (numberQuizGame.scoreLeft > numberQuizGame.scoreRight)
            {
                WinnerEffect(leftPosition);
            }
            else if (numberQuizGame.scoreLeft < numberQuizGame.scoreRight)
            {
                WinnerEffect(rightPosition);
            }
        }

        private void WinnerEffect(Vector2 winnerPosition)
        {
            Instantiate(winnerVFX, winnerPosition, Quaternion.identity);
        }

        public void RestartGame()
        {
            gameObject.SetActive(false);
            numberQuizGame.playCanvas.SetActive(true);
        }

        public void GoToMain()
        {
            gameObject.SetActive(false);
            numberQuizGame.startCanvas.SetActive(true);
        }
    }
}
