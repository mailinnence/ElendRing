using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class CharacterSoundFXManager : MonoBehaviour
    {

        private AudioSource audioSource;
        private AudioSource audioSource1;
        private AudioSource audioSource2;
        private AudioSource audioSource3;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
        }


        public void Walk_1_SoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.Walk1);
        }

        public void Walk_2_SoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.Walk2);
        }

        public void Walk_3_SoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.Walk3);
        }


        public void StopSound()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if(audioSource != null)
            {
                audioSource.Stop();
            }
        }
    }
}

