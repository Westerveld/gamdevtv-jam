using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dating
{
    public class DatingController : MonoBehaviour
    {
        public Animator anim;

        private int animID_attempt = Animator.StringToHash("Attempt");
        private int animID_success = Animator.StringToHash("Success");
        private int animID_failure = Animator.StringToHash("Failure");

        public AudioClip correctAnswer, wrongAnswer;

        private float pitch;
        private float initialPitch = 0.95f;
        private float increment = 0.01f;

        void Awake()
        {
            pitch = initialPitch;
            anim = GetComponent<Animator>();
        }

        public void Success()
        {
            AudioManager.instance?.PlaySFX(correctAnswer, pitch);
            pitch += increment;
            anim.SetTrigger(animID_success);
        }

        public void Attempt()
        {
            anim.SetTrigger(animID_attempt);
        }

        public void Failure()
        {
            AudioManager.instance?.PlaySFX(wrongAnswer);
            anim.SetTrigger(animID_failure);
        }
    }
}