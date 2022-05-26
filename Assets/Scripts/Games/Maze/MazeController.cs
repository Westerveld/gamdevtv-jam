using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class MazeController : MonoBehaviour
    {
        public CharacterController controller;

        public float lookThreshold = 0.1f;
        public float maxLookAngle = 28f;
        public Transform headBone;

        public float lookYSpeed;
        public float lookXSpeed = 15f;

        public Animator anim;

        public MazeInput input;
        private float speed;
        public float moveSpeed;
        private float animationBlend;
        [Range(0.0f, 0.3f)] public float rotationSmoothTime = 0.12f;
        public float speedChangeRate = 10.0f;
        private float rotationVelocity;
        public Camera cam;

        private int animID_Speed = Animator.StringToHash("Speed");
        private int animID_MotionSpeed = Animator.StringToHash("MotionSpeed");

        private bool canPlay = false;
        public void Setup()
        {
            canPlay = true;
        }

        void Update()
        {
            if (!canPlay) return;
            Move();
            Rotate();
        }

        void Move()
        {

            float targetSpeed = moveSpeed;
            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (input.move.y == 0) targetSpeed = 0.0f;

            float inputMagnitude = input.move.y;

            speed = targetSpeed * input.move.y;

            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;

            // move the player
            controller.Move(transform.forward.normalized * (speed * Time.deltaTime));

            anim.SetFloat(animID_Speed, animationBlend);
            anim.SetFloat(animID_MotionSpeed, inputMagnitude);
        }

        void Rotate()
        {
            if (headBone != null)
            {
                headBone.transform.Rotate(lookYSpeed * input.look.y * Vector3.right);
                float x = headBone.transform.localRotation.eulerAngles.x;
                if (x > 180f)
                {
                    x -= 360f;
                }
                else if (x < -180f)
                {
                    x += 360f;
                }

                headBone.transform.localRotation = Quaternion.Euler(Mathf.Clamp(x, -maxLookAngle, maxLookAngle), 0f, 0f);
            }

            if (input.move.x > lookThreshold || input.move.x < -lookThreshold || input.look.x > lookThreshold || input.look.x < -lookThreshold)
            {
                //Get whats higher;
                float rotateSpeed = input.look.x;
                //Rotate player on the spot
                transform.Rotate(Vector3.up * (input.look.x * lookXSpeed * Time.fixedDeltaTime));
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(true);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}
