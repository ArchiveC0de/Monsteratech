using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MoleGame
{
    public class CharacterController : MonoBehaviour
    {
        public Animator catAnimator;       // Animator 컴포넌트 참조
        public Renderer catRenderer;       // Renderer 컴포넌트 참조

        public Texture idleTexture;        // "Idle A" 표정 텍스처
        public Texture jumpTexture;        // "Jump" 표정 텍스처
        public Texture victoryTexture;     // "Victory" 표정 텍스처
        public Texture bowTexture;         // "Bow" 표정 텍스처

        public GameObject powerImage;      // 힘내라 냥! 이미지
        public GameObject greatJobImage;   // 대단하다 냥! 이미지
        public GameObject AngryImage;      // 혼내주자 냥! 이미지

        private Material catMaterial;      // 얼굴 텍스처용 메터리얼
        private GameObject currentImage;   // 현재 표시 중인 이미지

        private void Start()
        {
            // 얼굴 텍스처를 위한 메터리얼 설정 
            catMaterial = catRenderer.materials[1]; // 두 번째 메터리얼이 얼굴 텍스처를 사용

            // 이미지 초기 비활성화
            powerImage.SetActive(false);
            greatJobImage.SetActive(false);
            AngryImage.SetActive(false);
            currentImage = null;
        }

        // 카운트다운이 끝난 후 Idle 상태로 변경
        public void OnCountdown()
        {
            SetCharacterState("Idle A", idleTexture);
            ShowImage(AngryImage, 5.0f);
        }

        // 카운트다운이 끝난 후 Idle 상태로 변경
        public void OnCountdownEnd()
        {
            SetCharacterState("Idle A", idleTexture);
        }

        // 두더지를 때렸을 때 Jump 애니메이션 실행
        public void OnMoleHit()
        {
            SetCharacterState("Jump", jumpTexture);
            ShowImage(powerImage, 2.0f); 
        }

        // 3콤보 이상일 때 Victory 애니메이션 실행
        public void OnComboAchieved(int combo)
        {
            if (combo >= 3)
            {
                SetCharacterState("Combo", victoryTexture);
                ShowImage(greatJobImage, 2.0f); 
            }
            else
            {
                SetCharacterState("Idle A", idleTexture);
            }
        }

        // 게임이 종료되었을 때 Bow 애니메이션 실행
        public void OnGameOver()
        {
            SetCharacterState("Bow", bowTexture);
        }

        private void SetCharacterState(string trigger, Texture texture)
        {
            // 애니메이션 트리거 설정
            catAnimator.SetTrigger(trigger);

            // 얼굴 텍스처 변경
            catMaterial.mainTexture = texture;
        }

        private void ShowImage(GameObject image, float duration)
        {
            // 다른 이미지가 활성화되어 있다면 비활성화
            if (currentImage != null && currentImage != image)
            {
                currentImage.SetActive(false);
            }

            // 새로운 이미지를 활성화하고 현재 활성화된 이미지로 설정
            image.SetActive(true);
            currentImage = image;

            // 지정된 시간이 지나면 이미지를 비활성화하는 코루틴 실행
            StartCoroutine(HideImageAfterDelay(image, duration));
        }

        private IEnumerator HideImageAfterDelay(GameObject image, float delay)
        {
            yield return new WaitForSeconds(delay);
            image.SetActive(false);

            // 이미지가 비활성화되면 currentImage를 null로 설정
            if (currentImage == image)
            {
                currentImage = null;
            }
        }
    }
}
