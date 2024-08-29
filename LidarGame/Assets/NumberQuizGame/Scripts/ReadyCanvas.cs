using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NumberQuizGame
{
    public class ReadyCanvas : MonoBehaviour
    {
        [HideInInspector] public NumberQuizGame numberQuizGame;
        [HideInInspector] public TextMeshProUGUI readyTimerText;

        private float readyTimer;

        private void Awake()
        {
            numberQuizGame = GameObject.Find("NumberQuizGame").GetComponent<NumberQuizGame>();
            readyTimerText = transform.Find("ReadyTimerText").GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            readyTimer = 3;
        }

        void Update()
        {
            readyTimer -= Time.deltaTime;
            readyTimerText.text = Mathf.FloorToInt(readyTimer + 1).ToString();

            if (readyTimer < 0)
            {
                GameStart();
            }
        }

        void GameStart()
        {
            gameObject.SetActive(false);
            numberQuizGame.playCanvas.SetActive(true);
        }
    }
}