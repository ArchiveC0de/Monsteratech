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
        private bool isGameOver = false; // ���� ���� ���� ����

        void Start()
        {
            timeRemaining = gameDuration;
            //panels = GameObject.FindGameObjectsWithTag("Panel");
        }

        void Update()
        {
            if (isGameOver) return; // ������ ����Ǿ����� ������Ʈ�� ���� ����

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
            isGameOver = true; // ���� ���� ���� ����
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
                resultText.text = "���º�!";
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
