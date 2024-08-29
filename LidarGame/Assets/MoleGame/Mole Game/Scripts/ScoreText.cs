using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Draft : textInfo.meshInfo[i].vertices
// Working : textInfo.meshInfo[i].mesh.vertices

namespace MoleGame
{
    public class ScoreText : MonoBehaviour
    {
        public TMP_Text textComponent;
        public GameController gameController;

        public float fontSizeIncrease = 10f;
        public float duration = 0.5f;
        public bool isRainbowEffectActive;

        private float originalFontSize;
        private float timer;
        private bool isAnimating;

        void Start()
        {
            // 원래 폰트 크기 저장
            originalFontSize = textComponent.fontSize;
        }


        void Update()
        {
            //if (gameController.Combo >= 3 && !isAnimating)
            //{
            //    TriggerEffect();  // 애니메이션 시작
            //}

            if (isAnimating)
            {
                timer += Time.deltaTime;
                float progress = timer / duration;

                // 폰트 크기 애니메이션 적용
                textComponent.fontSize = Mathf.Lerp(originalFontSize + fontSizeIncrease, originalFontSize, progress);

                if (progress >= 1f)
                {
                    isAnimating = false;

                    // 애니메이션이 끝나면 폰트 크기를 원래대로 되돌림
                    textComponent.fontSize = originalFontSize;

                    if (isRainbowEffectActive)
                    {
                        textComponent.color = Color.white; // 무지개 효과 끝나면 원래 색상으로 복귀
                        isRainbowEffectActive = false;     // 무지개 효과 비활성화
                    }
                }
            }
        }

        private Color32 GetRainbowColor(float time)
        {
            byte r = (byte)(Mathf.Sin(time * 2f) * 127 + 128);
            byte g = (byte)(Mathf.Sin(time * 2f + 2f) * 127 + 128);
            byte b = (byte)(Mathf.Sin(time * 2f + 4f) * 127 + 128);
            return new Color32(r, g, b, 255);
        }

        public void TriggerEffect(bool activateRainbow = false)
        {
            if (!isAnimating)
            {
                timer = 0f;
                isAnimating = true;
                isRainbowEffectActive = activateRainbow;
            }
        }

    }
}