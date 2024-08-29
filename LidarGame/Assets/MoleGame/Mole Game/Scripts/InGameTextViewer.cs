using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace MoleGame
{
    public class InGameTextViewer : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController;

        [SerializeField]
        private TextMeshProUGUI textScore;

        [SerializeField]
        private TextMeshProUGUI textPlayTime;

        [SerializeField]
        private Slider sliderPlayTime;

        [SerializeField]
        private TextMeshProUGUI textCombo;

        private void Update()
        {
            textScore.text = "" + gameController.Score;

            // CurrentTime을 정수로 형식화하여 소수점 없이 표시
            textPlayTime.text = Mathf.FloorToInt(gameController.CurrentTime).ToString();
            sliderPlayTime.value = gameController.CurrentTime / gameController.MaxTime;


            textCombo.text = "" + gameController.Combo;
        }

    }
}