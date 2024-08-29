using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MoleGame
{

    public class FadeController : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration = 2.0f;

        private void Awake()
        {
            if (fadeImage == null)
            {
                fadeImage = GetComponentInChildren<Image>();
                // FadeIn();
            }
            // 초기 알파값을 0으로 설정하여 투명한 상태로 시작
            SetInitialAlpha(0.0f);
        }

        // 초기 알파값을 설정하는 메서드
        private void SetInitialAlpha(float alpha)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }

        // 페이드 인 효과를 시작하는 메서드
        public void FadeIn()
        {
            // 알파값을 1.0에서 0.0으로 줄여나감
            StartCoroutine(Fade(1.0f, 0.0f, null));
        }

        public void FadeOut(System.Action onComplete = null)
        {
            // 알파값을 0.0에서 1.0으로 증가시킴
            StartCoroutine(Fade(0.0f, 1.0f, onComplete));
        }

        // 페이드 효과를 구현하는 코루틴
        private IEnumerator Fade(float startAlpha, float endAlpha, System.Action onComplete)
        {
            // 경과 시간 초기화
            float elapsedTime = 0.0f;

            // 현재 이미지의 색상을 가져옴
            Color color = fadeImage.color;

            // 경과 시간이 fadeDuration보다 작을 때까지 반복
            while (elapsedTime < fadeDuration)
            {
                // 경과 시간을 증가
                elapsedTime += Time.deltaTime;

                // 알파값을 선형 보간(Lerp)하여 변경
                color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

                // 변경된 알파값을 이미지에 적용
                fadeImage.color = color;

                // 다음 프레임까지 대기
                yield return null;
            }

            // 반복이 끝난 후 최종 알파값을 설정
            color.a = endAlpha;
            fadeImage.color = color;

            // 페이드가 완료된 후 콜백 함수 호출
            onComplete?.Invoke();
        }
    }
}