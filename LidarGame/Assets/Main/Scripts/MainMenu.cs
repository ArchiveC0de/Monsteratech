using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainScene
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private float openningTime;
        [SerializeField] private GameObject[] gamePannels;
        private int currentPannel;

        private GameObject openningCanvas;
        private GameObject mainMenuCanvas;

        [SerializeField] private GameObject touchEffect;
        private float screenRatio;

        private void Awake()
        {
            openningCanvas = transform.Find("OpenningCanvas").gameObject;
            mainMenuCanvas = transform.Find("MainMenuCanvas").gameObject;
            
            for (int i = 0; i < gamePannels.Length; i++)
            {
                gamePannels[i] = transform.Find("MainMenuCanvas/Panel/GamePannel" + i).gameObject;
            }
        }

        private void Start()
        {
            for (int i = 0;i < gamePannels.Length; i++)
                gamePannels[i].SetActive(false);
            gamePannels[currentPannel].SetActive(true);
            mainMenuCanvas.SetActive(false);

            screenRatio = Screen.width / (float)Screen.height;
        }

        private void Update()
        {
            if (openningTime > 0)
            {
                openningTime -= Time.deltaTime;
            }
            else if(openningTime < 0 && !mainMenuCanvas.activeSelf)
            {
                openningCanvas.SetActive(false);
                mainMenuCanvas.SetActive(true);
            }

            if (Input.GetMouseButtonDown(0))
            {
                TouchEffect();
            }
        }

        public void LoadGameScene(string gameName)
        {
            SceneManager.LoadScene(gameName);
        }

        public void ChangeGamePannel(int num)
        {
            gamePannels[currentPannel].SetActive(false);

            currentPannel += num;
            if (gamePannels.Length <= currentPannel)
                currentPannel = 0;
            else if (currentPannel < 0)
                currentPannel = gamePannels.Length - 1;

            gamePannels[currentPannel].SetActive(true);
        }

        private void TouchEffect()
        {
            Vector3 mPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            mPosition.z = 0;
            Vector3 touchPosition = new Vector3((mPosition.x * 2 - 1) * Camera.main.orthographicSize * screenRatio, (mPosition.y * 2 - 1) * Camera.main.orthographicSize, 0);
            Instantiate(touchEffect, touchPosition, Quaternion.identity);
        }
    }
}
