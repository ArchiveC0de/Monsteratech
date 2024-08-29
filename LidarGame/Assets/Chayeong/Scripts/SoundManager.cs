using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace chayeong
{
    [Serializable]
    public class Sound
    {
        public string name; // 곡 이름
        public AudioClip clip; // 오디오 클립
    }


    public class SoundManager : MonoBehaviour
    {
        static public SoundManager instance;

        private void Awake()
        {
            // 최초 생성시
            if (instance == null)
            {
                instance = this; // 현재의 자기 자신을 할당
                DontDestroyOnLoad(gameObject); // 씬 전환해도 자기 자신이 파괴되지 않고 유지되도록
            }
            // 새로 sound manager object instance일 경우엔 파괴 <- 단 한개만 존재하도록
            else
            {
                Destroy(this.gameObject);
            }
        }

        public Sound[] effectSounds; // 효과음 오디오 클립
        public Sound[] bgmSounds; // bgm 오디오 클립

        // AudioSource == 재생기
        public AudioSource audiosourceBGM; // bgm은 한 군데에서만 이루어지면 되므로 배열X
        public AudioSource[] audioSourceEffects; // 효과음은 여러개 동시 재생가능하므로 배열O

        public string[] playSoundName; // 재생 중인 효과음 사운드 이름 배열

        // playSoundName 배열의 크기를 효과음 재생기 개수와 일치시킴
        private void Start()
        {
            playSoundName = new string[audioSourceEffects.Length];
        }

        public void PlaySE(string _name)
        {
            for (int i = 0; i < effectSounds.Length; i++)
            {
                if (_name == effectSounds[i].name)
                {
                    for (int j = 0; j < audioSourceEffects.Length; j++)
                    {
                        if (!audioSourceEffects[j].isPlaying)
                        {
                            audioSourceEffects[j].clip = effectSounds[i].clip;
                            audioSourceEffects[j].Play();
                            playSoundName[j] = effectSounds[i].name;
                            return;
                        }
                    }
                    Debug.Log("모든 가용 AudioSource가 사용중입니다");
                    return;
                }
            }
            Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았습니다");
        }

        public void playBGM(string _name)
        {
            for (int i = 0; i < bgmSounds.Length; i++)
            {
                if (_name == bgmSounds[i].name)
                {
                    audiosourceBGM.clip = bgmSounds[i].clip;
                    audiosourceBGM.Play();
                    return;
                }
            }
            Debug.Log(_name + "사운드가 soundManager에 등록되지 않았습니다");
        }

        // 모든 효과음 재생 중지
        public void StopAllSE()
        {
            for (int i = 0; i < audioSourceEffects.Length; i++)
            {
                audioSourceEffects[i].Stop();
            }
        }

        // 특정 효과움 재생 중지
        public void StopSE(string _name)
        {
            for (int i = 0; i < audioSourceEffects.Length; i++)
            {
                if (playSoundName[i] == _name)
                {
                    audioSourceEffects[i].Stop();
                    break;
                }
            }

            Debug.Log("재생 중인" + _name + "사운드가 없습니다");
        }
    }


}
