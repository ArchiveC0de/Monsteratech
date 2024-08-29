using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Video;

namespace MoleGame
{
    public class TimeLineController : MonoBehaviour
    {
        public PlayableDirector playableDirector;

        void Start()
        {
            // 시네마틱 영상 및 Timeline 재생
            PlayCinematic();

            // Timeline 재생 완료 이벤트 등록
            playableDirector.stopped += OnTimelineStopped;
        }

        void Update()
        {

            // 화면 터치 시 시네마틱 스킵
            if (Input.GetMouseButtonDown(0))
            {
                SkipCinematic();
            }
        }

        void PlayCinematic()
        {
            // Timeline 재생
            if (playableDirector != null)
            {
                playableDirector.Play();
            }

            // VideoPlayer를 통한 시네마틱 재생
            VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
            if (videoPlayer != null)
            {
                videoPlayer.Play();
                videoPlayer.loopPointReached += EndReached; // 영상이 끝났을 때 자동으로 호출
            }
        }

        void EndReached(VideoPlayer vp)
        {
            // 시네마틱이 끝나면 바로 게임 인트로 씬으로 전환
            LoadGameIntroScene();
        }

        void SkipCinematic()
        {
            // 시네마틱 중단 및 "Game Intro" 씬으로 전환
            StopCinematic();
            LoadGameIntroScene();
        }

        void StopCinematic()
        {
            // Timeline 중단
            if (playableDirector != null)
            {
                playableDirector.Stop();
            }

            // VideoPlayer 중단
            VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
            if (videoPlayer != null)
            {
                videoPlayer.Stop();
            }
        }

        void OnTimelineStopped(PlayableDirector aDirector)
        {
            // 재생된 Timeline이 이 스크립트에 설정된 director와 동일한지 확인
            if (playableDirector == aDirector)
            {
                LoadGameIntroScene();
            }
        }

        void LoadGameIntroScene()
        {
            // "Game Intro" 씬으로 전환
            SceneManager.LoadScene("Game Intro");
        }
    }
}
