using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace NumberQuizGame
{
    public class GameCanvas : MonoBehaviour
    {
        [SerializeField] private Vector2 scorePosition;
        [SerializeField] private GameObject rightVFX;
        [SerializeField] private GameObject wrongVFX;

        [HideInInspector] public int score;

        [HideInInspector] public NumberQuizGame numberQuizGame;
        private Text scoreText;
        private Text questionText;
        private NumberButton[] buttons = new NumberButton[6];

        private float coolDownTimeLeft;

        private void Awake()
        {
            numberQuizGame = GameObject.Find("NumberQuizGame").GetComponent<NumberQuizGame>();
            scoreText = transform.Find("ScoreText").GetComponent<Text>();
            questionText = transform.Find("QuestionText").GetComponent<Text>();

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = transform.Find("Buttons/Button " + i).GetComponent<NumberButton>();
            }
        }

        private void OnEnable()
        {
            score = 0;
            scoreText.text = score.ToString();
            QuizStart();
        }

        private void Update()
        {
            coolDownTimeLeft -= Time.deltaTime;
        }

        private int answer;

        private bool CheckNumber(int i)
        {
            if (answer.ToString() == buttons[i].buttonText.text)
            for (int j = 0; j < i - 1; j++)
            {
                if (buttons[j].buttonText.text == buttons[i].buttonText.text)
                { return true; }
            }
            return false;
        }

        void QuizStart()
        {
            coolDownTimeLeft = numberQuizGame.coolDownTime;

            int num1;
            int num2;

            switch (Random.Range(0, numberQuizGame.gameLevel.gameLevel))
            {
                case 0:
                    num1 = Random.Range(1, numberQuizGame.gameLevel.addMaxNumber);
                    num2 = Random.Range(1, num1);
                    answer = num1;
                    questionText.text = num1 - num2 + " + " + num2;

                    for (int i = 0; i < 6; i++)
                    {
                        buttons[i].buttonText.text = Random.Range(1, numberQuizGame.gameLevel.addMaxNumber).ToString();

                        if (CheckNumber(i))
                        {
                            buttons[i].buttonText.text = Random.Range(1, numberQuizGame.gameLevel.addMaxNumber).ToString();
                        }
                    }

                    buttons[Random.Range(0, 6)].buttonText.text = answer.ToString();

                    break;

                case 1:
                    num1 = Random.Range(1, numberQuizGame.gameLevel.addMaxNumber);
                    num2 = Random.Range(1, num1);
                    answer = num1 - num2;
                    questionText.text = num1 + " - " + num2;

                    for (int i = 0; i < 6; i++)
                    {
                        buttons[i].buttonText.text = Random.Range(1, numberQuizGame.gameLevel.addMaxNumber).ToString();

                        if (CheckNumber(i))
                        {
                            buttons[i].buttonText.text = Random.Range(1, numberQuizGame.gameLevel.addMaxNumber).ToString();
                        }
                    }

                    buttons[Random.Range(0, 6)].buttonText.text = answer.ToString();

                    break;

                case 2:
                    num1 = Random.Range(1, 10);
                    num2 = Random.Range(1, 10);
                    questionText.text = num1 + " ¡¿ " + num2;
                    answer = num1 * num2;

                    for (int i = 0; i < 6; i++)
                    {
                        buttons[i].buttonText.text = (Random.Range(1, 10) * Random.Range(1, 10)).ToString();

                        if (CheckNumber(i))
                        {
                            buttons[i].buttonText.text = (Random.Range(1, 10) * Random.Range(1, 10)).ToString();
                        }
                    }

                    buttons[Random.Range(0, 6)].buttonText.text = answer.ToString();

                    break;

                case 3:
                    num1 = Random.Range(1, 10);
                    num2 = Random.Range(1, 10);
                    questionText.text = num1 * num2 + " ¡À " + num1;
                    answer = num2;

                    for (int i = 0; i < 6; i++)
                    {
                        buttons[i].buttonText.text = Random.Range(1, 10).ToString();

                        if (CheckNumber(i))
                        {
                            buttons[i].buttonText.text = Random.Range(1, 10).ToString();
                        }
                    }

                    buttons[Random.Range(0, 6)].buttonText.text = answer.ToString();

                    break;
            }
        }

        public void ButtonClick(int i)
        {
            if (coolDownTimeLeft < 0)
            {
                if (answer == int.Parse(buttons[i].buttonText.text))
                {
                    score++;
                    Instantiate(rightVFX, scorePosition, Quaternion.identity);
                }
                else
                {
                    if (score > 0)
                    {
                        score--;
                    }
                    Instantiate(wrongVFX, scorePosition, Quaternion.identity);
                }

                scoreText.text = score.ToString();
                QuizStart();
            }
        }
    }
}
