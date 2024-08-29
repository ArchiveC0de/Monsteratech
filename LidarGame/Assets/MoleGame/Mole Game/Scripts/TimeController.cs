using UnityEngine;


namespace MoleGame
{
    public class TimeController : MonoBehaviour
    {
        [SerializeField] private Animator timeAnimator;      // 시간 애니메이션을 제어할 Animator
        [SerializeField] private AudioSource countdownAudio; // 10초 카운트다운 사운드
        [SerializeField] private GameObject timeImageObject; // [Image] Time 오브젝트
        [SerializeField] private GameController gameController;  // GameController 참조

        private bool isCountdownStarted = false;

        private void Start()
        {
            timeImageObject.SetActive(false);  // 게임 시작 시 비활성화
        }

        void Update()
        {
            float gameTime = gameController.GetRemainingTime();  // GameController에서 남은 시간 가져오기

            if (gameTime <= 10f && gameTime > 0f && !isCountdownStarted)
            {
                StartCountdownAnimation(gameTime);  // 10초 전에 애니메이션과 소리 시작
            }

            if (gameTime <= 0f && isCountdownStarted)
            {
                StopCountdownAnimation();  // 게임 종료 시 애니메이션과 소리 중지
            }
        }

        public void StartCountdownAnimation(float gameTime)
        {
            if (!isCountdownStarted)
            {
                timeImageObject.SetActive(true);          // 오브젝트 활성화
                timeAnimator.SetBool("ShowTime", true);   // 애니메이션 시작

                // 알람 소리를 남은 시간에 맞춰 플레이
                countdownAudio.time = 10f - gameTime;     // 현재 남은 시간에 따라 알람 소리의 시작 시간을 조정
                countdownAudio.Play();

                isCountdownStarted = true;
            }
        }

        public void StopCountdownAnimation()
        {
            timeAnimator.SetBool("ShowTime", false);  // 애니메이션 중지
            timeImageObject.SetActive(false);         // 오브젝트 비활성화
            countdownAudio.Stop();                    // 소리 중지
        }
    }
}