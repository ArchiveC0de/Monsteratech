using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NumberQuizGame
{
    public class LevelButton : MonoBehaviour
    {
        [HideInInspector] public NumberQuizGame numberQuizGame;

        private TextMeshProUGUI levelText;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            levelText = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();

            numberQuizGame = GameObject.Find("NumberQuizGame").GetComponent<NumberQuizGame>();
        }

        private void Start()
        {
            levelText.text = 1.ToString();
        }

        public void OnButtonClick()
        {
            animator.SetTrigger("OnButtonClick");
        }

        public void OnLevelButtonClick(int level)
        {
            numberQuizGame.SetGameLevel(level);
            levelText.text = level.ToString();
        }
    }
}
