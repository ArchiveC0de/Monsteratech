using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoleGame
{
    public class ButtonClickEvent : MonoBehaviour
    {
        public GameObject howToPlayPanel;

        void Start()
        {
            if (howToPlayPanel == null)
            {
                howToPlayPanel = GameObject.Find("Panel | How To Play");
            }
        }

        public void SceneLoader(string SceneName)
        {
            SceneManager.LoadScene(SceneName);
        }

        public void GameExit()
        {
            // 유니티 에디터 일때 실행
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;

            // 안드로이드일때 실행
#elif UNITY_ANDROID
            Application.Quit();

#endif // 종료
        }

        public void ToggleHowToPlayPanel()
        {
            if (howToPlayPanel != null)
            {
                howToPlayPanel.SetActive(!howToPlayPanel.activeSelf);
            }
            else
            {
                Debug.LogError("How To Play Panel is not assigned or has been destroyed.");
            }
        }
    }

}