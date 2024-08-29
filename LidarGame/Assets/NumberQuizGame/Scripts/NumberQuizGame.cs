using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NumberQuizGame
{
    public class NumberQuizGame : MonoBehaviour
    {
        public int gameTime;
        public int gameWarningTime;
        public float coolDownTime;
        [SerializeField] private GameObject touchEffect;

        public GameLevel[] gameLevels = new GameLevel[4];
        [HideInInspector] public GameLevel gameLevel;

        [HideInInspector] public GameObject startCanvas;
        [HideInInspector] public GameObject readyCanvas;
        [HideInInspector] public GameObject playCanvas;
        [HideInInspector] public GameObject endCanvas;

        [HideInInspector] public int scoreLeft;
        [HideInInspector] public int scoreRight;

        private float screenRatio;

        [HideInInspector] public SoundManager soundManager;

        private void Awake()
        {
            startCanvas = transform.Find("StartCanvas").gameObject;
            readyCanvas = transform.Find("ReadyCanvas").gameObject;
            playCanvas = transform.Find("PlayCanvas").gameObject;
            endCanvas = transform.Find("EndCanvas").gameObject;

            soundManager = GetComponent<SoundManager>();
        }

        private void Start()
        {
            screenRatio = Screen.width / (float)Screen.height;

            gameLevel = gameLevels[0];

            startCanvas.SetActive(true);
            readyCanvas.SetActive(false);
            playCanvas.SetActive(false);
            endCanvas.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TouchEffect();
            }
        }

        private void TouchEffect()
        {
            Vector3 mPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            mPosition.z = 0;
            Vector3 touchPosition = new Vector3((mPosition.x * 2 - 1) * Camera.main.orthographicSize * screenRatio, (mPosition.y * 2 - 1) * Camera.main.orthographicSize, 0);
            Instantiate(touchEffect, touchPosition, Quaternion.identity);
        }

        public void SetGameLevel(int level)
        {
            gameLevel = gameLevels[level - 1];
        }
    }

    [System.Serializable]
    public class GameLevel
    {
        public int gameLevel;
        public int addMaxNumber;
    }
}
