using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MoleGame
{
    public class FadeEffect_TMP : MonoBehaviour
    {
        [SerializeField]
        private float fadeTime = 0.5f;

        private TextMeshProUGUI fadeText;

        private void Awake()
        {
            fadeText = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            StartCoroutine("FadeLoop");
        }

        // 에디터에서 색상 값은 0 ~ 255까지 표현 가능
        // 스크립트에서 생상 값은 0 ~ 1까지 표현 가능
        private IEnumerator FadeLoop()
        {
            while (true)
            {
                yield return StartCoroutine(Fade(1, 0));

                yield return StartCoroutine(Fade(0, 1));
            }
        }

        private IEnumerator Fade(float start, float end)
        {
            float current = 0;
            float percent = 0;

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / fadeTime;

                Color color = fadeText.color;
                color.a = Mathf.Lerp(start, end, percent);
                fadeText.color = color;

                yield return null;
            }
        }
    }
}