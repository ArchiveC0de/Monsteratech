using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace MoleGame
{

    // 외부에서 메서드를 호출하여 CountDown을 시작하고 CountDown이 완료되면
    // 원하는 메서드를 호출하도록 하기위해 앞에서 작성한 ObjectDetector 클래스 와 마찬가지로 Event를 이용한다.
    public class CountDown : MonoBehaviour
    {
        [System.Serializable]
        private class CountDownEvent : UnityEvent { }   // UnityEvent를 상속받는 CountDownEvent클래스를 정의한다.
        private CountDownEvent endOfCountDown;          // 카운트 다운 종료 후 외부 메소드 실행을 위해 이벤트 클래스 사용

        private TextMeshProUGUI textCountDown;          // 카운트 다운 텍스트를 출력하는 Text UI
        private AudioSource audioSource;                // 카운트 다운 사운드 재생

        [SerializeField]
        private int maxFontSize;                        // 폰트의 최대 크기

        [SerializeField]
        private int minFontSize;                        // 폰트의 최소 크기

        private CharacterController characterController; // CharacterController 참조


        private void Awake()
        {
            endOfCountDown = new CountDownEvent();
            textCountDown = GetComponent<TextMeshProUGUI>();
            audioSource = GetComponent<AudioSource>();
            characterController = FindObjectOfType<CharacterController>();
        }

        // CountDownEvent클래스의 부모가 UnityEvent이기 때문에
        // UnityAction action에는 매개변수가 없는 메소드를 등록할 수 있다.

        // int start, int end는 선택적 매개변수로
        // 매개변수 값을 입력하지 않으면 start = 3, end = 1 이 설정된다.

        public void StartCountDown(UnityAction action, int start = 5, int end = 1)
        {
            StartCoroutine(OnCountDown(action, start, end));
            if (characterController != null)
            {
                characterController.OnCountdown(); // 게임 시작할 때 이미지 표시 및 Idle 상태 설정
            }
        }

        private IEnumerator OnCountDown(UnityAction action, int start, int end)
        {
            // 외부에서 받아온 action 메소드를 이벤트에 등록
            endOfCountDown.AddListener(action);

            // start = 3 / end 1 이기때문에 start가 1이될때까지 실행된다.
            while (start > end - 1)
            {
                // 카운트 다운 사운드 실행
                audioSource.Play();

                // 카운트 다운 텍스트 설정 (소수점 제거)
                textCountDown.text = start.ToString("F0");

                // 폰트 크기를 변경하는 애니메이션 (재생 완료시 아래 코드 실행)
                yield return StartCoroutine("OnFontAnimation");

                // 카운트 다운 숫자 1 감소
                start--;
            }

            // action 메소드를 실행
            endOfCountDown.Invoke();

            // action 메소드를 이벤트에서 제거
            endOfCountDown.RemoveListener(action);

            // 카운트 다운 오브젝트 비활성화
            gameObject.SetActive(false);
        }

        private IEnumerator OnFontAnimation()
        {
            float percent = 0;

            while (percent < 1)
            {
                percent += Time.deltaTime;

                // 폰트 크기를 200에서 100까지 percent 시간 동안 감소
                textCountDown.fontSize = Mathf.Lerp(maxFontSize, minFontSize, percent);

                yield return null;
            }
        }

    }

}
