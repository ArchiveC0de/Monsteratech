using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NumberQuizGame
{
    public class NumberButton : MonoBehaviour
    {
        [HideInInspector] public Text buttonText;

        private void Awake()
        {
            buttonText = transform.Find("Text").GetComponent<Text>();
        }
    }
}
