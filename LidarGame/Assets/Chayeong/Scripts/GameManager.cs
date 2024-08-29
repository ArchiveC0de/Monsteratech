using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace chayeong
{
    public class GameManager : MonoBehaviour
    {
        public float gameDuration = 15f;
        private float timeRemaining;
        public Text timerText;
        public Text blueText;
        public Text redText;
        public Text resultText;
        public GameObject[] panels;
        public GameObject gameover;

        public GameObject yellow;
        public GameObject red;
        public GameObject tie;

        private int redCount;
        private int blueCount;
        private bool isGameOver = false; // 게임 종료 상태 변수

        void Start()
        {
            timeRemaining = gameDuration;
            //panels = GameObject.FindGameObjectsWithTag("Panel");
        }

        void Update()
        {
            if (isGameOver) return; // 게임이 종료되었으면 업데이트를 하지 않음

            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                EndGame();
            }
            else
            {
                timerText.text = Mathf.Ceil(timeRemaining).ToString();
                UpdateScoreText();
            }
        }

        void UpdateScoreText()
        {
            redCount = 0;
            blueCount = 0;

            foreach (GameObject panel in panels)
            {
                Panel panelScript = panel.GetComponent<Panel>();
                if (panelScript.IsRed())
                {
                    redCount++;
                    //blueCount--;
                }
                else
                {
                    blueCount++;
                    //redCount--;
                }
            }

            redText.text = redCount.ToString();
            blueText.text = blueCount.ToString();

        }


        void EndGame()
        {
            //UpdateScoreText();
            isGameOver = true; // 게임 종료 상태 설정
            gameover.gameObject.SetActive(true);
            timerText.gameObject.SetActive(false);
            //resultText.gameObject.SetActive(true);

            if (redCount > blueCount)
            {
                resultText.text = redCount.ToString();
                red.SetActive(true);
            }
            else if (blueCount > redCount)
            {
                resultText.text = blueCount.ToString();
                yellow.SetActive(true);
            }
            else
            {
                resultText.text = "무승부!";
                tie.SetActive(true);
            }
        }

        public bool IsGameOver()
        {
            return isGameOver;
        }

        public void OnMenuButton()
        {
            SceneManager.LoadScene("HomeScene");
        }

        public void OnRestartButton()
        {
            SceneManager.LoadScene("GameScene");
        }
    }

}
