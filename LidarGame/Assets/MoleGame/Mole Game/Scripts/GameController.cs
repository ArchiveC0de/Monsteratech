using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MoleGame
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private CountDown countDown;

        [SerializeField]
        private MoleSpawner moleSpawner;

        [SerializeField]
        private AudioSource bgmAudioSource;

        [SerializeField]
        private FadeController fadeController;


        [SerializeField] private float maxTime = 60f;  // 전체 게임 시간

        // [SerializeField]
        // private AudioSource burningModeAudioSource; // 버닝 모드 시 재생될 오디오 소스

        [SerializeField]
        private AudioClip panelOpenClip; // 패널이 켜질 때 재생할 효과음 클립


        [SerializeField]
        private AudioClip burningModeClip; // 버닝 모드 시 재생될 오디오 클립



        private bool isBurningModeActive = false; // 버닝 모드 활성화 여부
        private bool isGameStarted = false; // 게임 시작 여부

        private CharacterController characterController;

        private int score;
        private int combo;
        private float currentTime;
        //    private bool doubleScoreActive = false; // 점수 두 배 활성화 여부
        //    private float doubleScoreDuration;      // 점수 두 배 지속 시간

        // Game 씬 현재점수 표시
        public GameObject gameScorePanel;  // [Panel] Game Score Bord에 연결
        public TextMeshProUGUI scoreText;
    

        public int Score
        {
            // Math.Max() 메소드
            // float result = Mathf.Max(float a, float b);
            // a 와 b 중 더 큰 값을 반환
            set
            {
                score = Mathf.Max(0, value);
                UpdateScoreUI();  // 점수가 변경될 때마다 UI 업데이트
            }
            get => score;
        }

        public int Combo
        {
            // set => combo = Mathf.Max(0, value);
            set
            {
                combo = Mathf.Max(0, value);
                // 70 이상일 땐 MaxSpawnMole이 5로 고정되기 때문에 70까지만 체크
                if (combo <= 70)
                {
                    // 콤보에 따라 생성되는 최대 두더지 숫자
                    moleSpawner.MaxSpawnMole = 1 + (combo + 10) / 20;
                }

                // 최대 콤보 저장 
                if (combo > MaxCombo)
                {
                    MaxCombo = combo;
                }
            }

            get => combo;
        }

        public int MaxCombo { private set; get; }
        public int NormalMoleHitCount { set; get; }
        public int RedMoleHitCount { set; get; }
        public int BlueMoleHitCount { set; get; }

        // [field: SerializeField]
        // 자동 구현 프로퍼티를 Inspector View에 보이게 할 떄 사용
        // [field: SerializeField]
        // public float MaxTime { private set; get; }
        public float MaxTime => maxTime;  // 기존 maxTime을 사용하도록 수정

        // public float CurrentTime { private set; get; }
        public float CurrentTime
        {
            // value값과 0사이로 시간값을 제한해준다.
            set => currentTime = Mathf.Clamp(value, 0, MaxTime);
            get => currentTime;
        }

        private void Start()
        {
            countDown.StartCountDown(GameStart);
            currentTime = maxTime;

            // 게임 시작 시 점수 패널 비활성화
            gameScorePanel.SetActive(false);
            // CharacterController 컴포넌트를 찾음
            characterController = FindObjectOfType<CharacterController>();
        }

        private void FixedUpdate()
        {
            // 게임 시작 전에는 시간 흐름 중지
            if (!isGameStarted) return; 

            // currentTime -= Time.deltaTime; 시간이 중복되어 줄어들어 60초가 30초로 되는 매직 !

            // Debugging: currentTime 값 확인
            // Debug.Log("Current Time: " + currentTime);

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                // 게임 종료 처리 로직 (필요시 추가)
            }
        }

        private void GameStart()
        {
            isGameStarted = true;  // 게임 시작 플래그 설정
            moleSpawner.Setup();
            bgmAudioSource.Play();
            StartCoroutine("OnTimeCount");
            
        }

        private IEnumerator OnTimeCount()
        {

            CurrentTime = MaxTime;
            
            while (CurrentTime > 0)
            {
                if (CurrentTime <= 20f && !isBurningModeActive)
                {
                    StartBurningMode();
                }


                CurrentTime -= Time.deltaTime;

                yield return null;
            }
            // CurrentTime이 0이 되면 GameOver() 메소드를 호출해 게임오버 처리
            GameOver();
        }

        private void GameOver()
        {
            isGameStarted = false;     // 게임 종료 시 시간 흐름 중지
            bgmAudioSource.Stop();    // 게임 종료 시 BGM 중지

                if (characterController != null)
            {
                characterController.OnGameOver(); // CharacterController의 OnGameOver 메서드 호출
            }
            // 두더지 생성 중지
            moleSpawner.StopSpawning();

            // 점수 UI 업데이트
            UpdateScoreUI();

            // 게임 점수 패널을 활성화
            gameScorePanel.SetActive(true);

            // 패널이 켜질 때 효과음 재생
            if (bgmAudioSource != null && panelOpenClip != null)
            {
                bgmAudioSource.PlayOneShot(panelOpenClip); // BGM AudioSource를 사용하여 효과음 재생
            }


            // 페이드 아웃 시작 후 완료되면 씬 전환

            fadeController.FadeOut(null);

            fadeController.FadeOut(() =>
            {
                // 현재 스테이지에서 획득한 여러 정보 저장
                PlayerPrefs.SetInt("CurrentScore", Score);
                PlayerPrefs.SetInt("CurrentMaxCombo", MaxCombo);
                PlayerPrefs.SetInt("CurrentNormalMoleHitCount", NormalMoleHitCount);
                PlayerPrefs.SetInt("CurrentRedMoleHitCount", RedMoleHitCount);
                PlayerPrefs.SetInt("CurrentBlueMoleHitCount", BlueMoleHitCount);

                // "GameOver" 씬으로 이동
                // SceneManager.LoadScene("GameOver");
            });
        }

        private void StartBurningMode()
        {
            if (!isBurningModeActive) // 버닝 모드가 이미 활성화된 경우 중복 실행 방지
            {
                isBurningModeActive = true;

                // 기존 BGM을 중지하고 버닝 모드 클립을 재생
                // bgmAudioSource.Stop();
                // bgmAudioSource.clip = burningModeClip;
                // bgmAudioSource.Play();

                // 알림 소리 재생 (기존 BGM은 유지)
                bgmAudioSource.PlayOneShot(burningModeClip);

                // 두더지 스폰 속도를 증가시킴
                moleSpawner.StartBurningMode();
            }
        }

        public float GetRemainingTime()
        {
            return currentTime;
        }


        void UpdateScoreUI()
        {
            if (scoreText != null)
            {
                scoreText.text = $"{Score} 점";  // "점수 + 점" 형태로 표시  // 현재 Score 값을 텍스트로 표시
            }
        }
        //// 점수 두 배 효과 활성화 메서드
        //public void ActivateDoubleScore(float duration)
        //{
        //    if (doubleScoreActive)
        //    {
        //        doubleScoreDuration = Mathf.Max(doubleScoreDuration, duration);
        //    }
        //    else
        //    {
        //        StartCoroutine(DoubleScoreCoroutine(duration));
        //    }
        //}

        //// 점수 두 배 지속 시간 관리 코루틴
        //private IEnumerator DoubleScoreCoroutine(float duration)
        //{
        //    doubleScoreActive = true;
        //    doubleScoreDuration = duration;

        //    while (doubleScoreDuration > 0)
        //    {
        //        doubleScoreDuration -= Time.deltaTime;
        //        yield return null;
        //    }

        //    doubleScoreActive = false;
        //}

        //// 점수 계산 메서드 (두 배 효과 반영)
        //public int GetScore(int baseScore)
        //{
        //    return doubleScoreActive ? baseScore * 2 : baseScore;
        //}

    }
} 