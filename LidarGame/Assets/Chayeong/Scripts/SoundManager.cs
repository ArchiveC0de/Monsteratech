using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace chayeong
{
    [Serializable]
    public class Sound
    {
        public string name; // �� �̸�
        public AudioClip clip; // ����� Ŭ��
    }


    public class SoundManager : MonoBehaviour
    {
        static public SoundManager instance;

        private void Awake()
        {
            // ���� ������
            if (instance == null)
            {
                instance = this; // ������ �ڱ� �ڽ��� �Ҵ�
                DontDestroyOnLoad(gameObject); // �� ��ȯ�ص� �ڱ� �ڽ��� �ı����� �ʰ� �����ǵ���
            }
            // ���� sound manager object instance�� ��쿣 �ı� <- �� �Ѱ��� �����ϵ���
            else
            {
                Destroy(this.gameObject);
            }
        }

        public Sound[] effectSounds; // ȿ���� ����� Ŭ��
        public Sound[] bgmSounds; // bgm ����� Ŭ��

        // AudioSource == �����
        public AudioSource audiosourceBGM; // bgm�� �� ���������� �̷������ �ǹǷ� �迭X
        public AudioSource[] audioSourceEffects; // ȿ������ ������ ���� ��������ϹǷ� �迭O

        public string[] playSoundName; // ��� ���� ȿ���� ���� �̸� �迭

        // playSoundName �迭�� ũ�⸦ ȿ���� ����� ������ ��ġ��Ŵ
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
                    Debug.Log("��� ���� AudioSource�� ������Դϴ�");
                    return;
                }
            }
            Debug.Log(_name + "���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�");
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
            Debug.Log(_name + "���尡 soundManager�� ��ϵ��� �ʾҽ��ϴ�");
        }

        // ��� ȿ���� ��� ����
        public void StopAllSE()
        {
            for (int i = 0; i < audioSourceEffects.Length; i++)
            {
                audioSourceEffects[i].Stop();
            }
        }

        // Ư�� ȿ���� ��� ����
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

            Debug.Log("��� ����" + _name + "���尡 �����ϴ�");
        }
    }


}
