using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NumberQuizGame
{
    public class PlayCanvas : MonoBehaviour
    {
        [HideInInspector] public NumberQuizGame numberQuizGame;

        [HideInInspector] public Text timerText;
        [HideInInspector] public GameObject gameCanvas;
        [HideInInspector] public GameObject leftCanvas;
        [HideInInspector] public GameObject rightCanvas;

        private GameObject warnningPanel;

        private float gameTimer;
        private bool warnned;

        private void Awake()
        {
            numberQuizGame = GameObject.Find("NumberQuizGame").GetComponent<NumberQuizGame>();
            timerText = transform.Find("TimerText").GetComponent<Text>();
            gameCanvas = transform.Find("GameCanvas").gameObject;
            leftCanvas = transform.Find("GameCanvas/LeftCanvas").gameObject;
            rightCanvas = transform.Find("GameCanvas/RightCanvas").gameObject;
            warnningPanel = transform.Find("WarningPanel").gameObject;
        }

        private void OnEnable()
        {
            gameTimer = numberQuizGame.gameTime;

            warnningPanel.SetActive(false);
            warnned = false;
        }

        private void Update()
        {
            gameTimer -= Time.deltaTime;
            timerText.text = Mathf.FloorToInt(gameTimer + 1).ToString();

            if (gameTimer < numberQuizGame.gameWarningTime && warnned == false)
            {
                warnned = true;
                warnningPanel.SetActive (true);
            }
            else if (gameTimer < 0)
            {
                GameEnd();
            }
        }

        private void GameEnd()
        {
            numberQuizGame.scoreLeft = leftCanvas.GetComponent<GameCanvas>().score;
            numberQuizGame.scoreRight = rightCanvas.GetComponent<GameCanvas>().score;
            gameObject.SetActive(false);
            numberQuizGame.endCanvas.SetActive(true);
        }
    }
}