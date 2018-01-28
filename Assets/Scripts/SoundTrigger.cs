using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class SoundTrigger : MonoBehaviour
    {
        public AudioClip Clip;

        private AudioSource Source;

        public void Awake()
        {
            Source = FindObjectOfType<Camera>().GetComponent<AudioSource>();
        }

        public void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Character") && Source.clip != Clip)
            {
                Source.clip = Clip;
                Source.Play();
            }
        }
    }
}