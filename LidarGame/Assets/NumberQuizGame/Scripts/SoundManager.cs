using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace NumberQuizGame
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] audioClips;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayClip(int clipNum)
        {
            audioSource.clip = audioClips[clipNum];
            audioSource.Play();
        }

        public void PlaySound(string clipName)
        {
            
        }
    }
}