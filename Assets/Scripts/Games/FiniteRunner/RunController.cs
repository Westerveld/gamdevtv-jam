using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner
{
    public class RunController : MonoBehaviour
    {
        public RunInput input;

        public Animator anim;

        public CapsuleCollider capsule;
        private float defaultCapsuleHeight;
        private float yOffset;
        
        private bool canDoAction = false;

        private int animID_jump = Animator.StringToHash("Jump");

        private int animID_slide = Animator.StringToHash("Slide");

        public AudioClip jumpSFX, slideSFX;
        
        // Start is called before the first frame update
        void Start()
        {
            if (anim == null) anim = GetComponent<Animator>();
            defaultCapsuleHeight = capsule.height;
            yOffset = capsule.center.y;
        }

        public void Setup()
        {
            canDoAction = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!canDoAction)
            {
                input.jump = false;
                input.slide = false;
                return;
            }

            if (input.jump)
            {
                AudioManager.instance?.PlaySFX(jumpSFX);
                input.jump = false;
                anim.SetTrigger(animID_jump);
                canDoAction = false;
                capsule.height = defaultCapsuleHeight / 2f;
                capsule.center = new Vector3(0f, yOffset + (yOffset / 2),0f);
                
                return;
            }

            if (input.slide)
            {
                AudioManager.instance?.PlaySFX(slideSFX);
                canDoAction = false;
                input.slide = false;
                anim.SetTrigger(animID_slide);
                capsule.height = defaultCapsuleHeight / 2f;
                capsule.center = new Vector3(0f, yOffset - (yOffset / 2),0f);
                return;
            }
        }

        void Finished()
        {
            canDoAction = true;
            capsule.height = defaultCapsuleHeight;
            capsule.center = new Vector3(0f, yOffset);
        }
    }
}
