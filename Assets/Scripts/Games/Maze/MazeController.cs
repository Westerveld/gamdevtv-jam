using System;
using System.Collections;
using System.Collections.Generic;
using Generic;
using TMPro;
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
        public float sprintSpeed = 10;
        private float animationBlend;
        [Range(0.0f, 0.3f)] public float rotationSmoothTime = 0.12f;
        public float speedChangeRate = 10.0f;
        private float rotationVelocity;
        public Camera cam;

        private int animID_Speed = Animator.StringToHash("Speed");
        private int animID_MotionSpeed = Animator.StringToHash("MotionSpeed");

        private bool canPlay = false;

        public Stat sprintStamina;

        public TMP_Text staminaText;

        private GameManager manager;
        public void Setup(GameManager manager, float maxSprintStamina = 100f, float sprintRegenSpeed = 5f)
        {
            canPlay = true;
            sprintStamina = new Stat(maxSprintStamina, sprintRegenSpeed);
        }

        private void Update()
        {
            if (!canPlay) return;
            Rotate();
        }

        void FixedUpdate()
        {
            if (!canPlay) return;
            Move();
            sprintStamina.RegenStat(Time.fixedDeltaTime);
            if(staminaText != null)
                staminaText.text = $"{sprintStamina.GetPercentage()}%";
        }

        void Move()
        {

            float targetSpeed = moveSpeed;
            if (input.sprint && sprintStamina.currentValue > 0f)
            {
                targetSpeed = sprintSpeed;
                anim.SetBool("Sprint", true);
                sprintStamina.RemoveStat(1f, 0.5f);
            }
            else
            {
                anim.SetBool("Sprint", false);
            }
            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (input.move == Vector2.zero) targetSpeed = 0.0f;

            float inputMagnitude = input.move.magnitude;

            speed = targetSpeed * inputMagnitude;

            // normalise input direction
            Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;
            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;
            Vector3 targetDirection = (inputDirection.x * transform.right) + (inputDirection.z * transform.forward);
            targetDirection *= (speed * Time.fixedDeltaTime);
            // move the player
            controller.Move(targetDirection);

            anim.SetFloat("Horizontal", input.move.x);
            anim.SetFloat("Vertical", input.move.y);
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

        void OnFootstep()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Door"))
            {
                manager.EndGame();
            }
        }
    }
}
