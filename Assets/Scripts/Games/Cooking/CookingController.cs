using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class CookingController : MonoBehaviour
    {
        private bool canPlay = false;
        public CookingInput input;
        public CharacterController controller;
        public Animator anim;
        public float moveSpeed = 2.0f;
        public float sprintSpeed = 5.335f;
        [Range(0.0f,0.3f)]
        public float rotationSmoothTime = 0.12f;
        public float speedChangeRate = 10.0f;
        public float blendSpeed = 2f;
        
        private float speed;
        private float animationBlend;
        private float targetRotation = 0.0f;
        private float rotationVelocity;

        private int animID_Speed = Animator.StringToHash("Speed");
        private int animID_MotionSpeed = Animator.StringToHash("MotionSpeed");
        private int animID_PickUp = Animator.StringToHash("Pickup");
        private int animID_Drop = Animator.StringToHash("Drop");

        public Camera cam;

        public bool canInteractWithArea;
        public bool hasObject;

        public GameObject currentObject;
        public InteractArea currentArea;
        public Transform holdLocation;

        private bool interacting = false;
        
        
        [ContextMenu("Test")]
        public void Setup()
        {
            canPlay = true;
        }

        void Update()
        {
            if (!canPlay) return;

            if (!interacting)
            {
                Move();
                Interact();
            }
        }
        
        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = input.sprint ? moveSpeed : sprintSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (input.move == Vector2.zero) targetSpeed = 0.0f;


            float inputMagnitude = input.move.magnitude;

            speed = targetSpeed;

            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (input.move != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  cam.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            // move the player
            controller.Move(targetDirection.normalized * (speed * Time.deltaTime));

            anim.SetFloat(animID_Speed, animationBlend);
            anim.SetFloat(animID_MotionSpeed, inputMagnitude);
        }


        void Interact()
        {
            if (input.interact)
            {
                input.interact = false;
                interacting = true;
                if (canInteractWithArea && !hasObject)
                {
                    if (!hasObject)
                    {
                        anim.SetTrigger(animID_PickUp);
                    }
                    else if (hasObject)
                    {
                        anim.SetTrigger(animID_Drop);
                    }
                }
                else if (hasObject)
                {
                    anim.SetTrigger(animID_Drop);
                }
            }
        }
        private void OnFootstep(AnimationEvent animationEvent)
        {
            
        }

        private void EnableLayer()
        {
            if (currentArea != null)
            {
                if (currentArea.allowPickup)
                {
                    currentObject = currentArea.GetItem(holdLocation);
                    if (currentObject != null)
                    {
                        hasObject = true;
                        StartCoroutine(BlendLayer(true));
                    }
                }
            }
            interacting = false;
        }

        private void DisableLayer()
        {
            if (currentObject == null)
                return;
            StartCoroutine(BlendLayer(false));
            if (currentArea != null)
            {
                hasObject = !currentArea.PlaceItem(currentObject);
            }
            else
            {
                if (currentObject.GetComponent<IDroppable>() != null)
                {
                    currentObject.GetComponent<IDroppable>().DropItem();
                }
                else
                {
                    currentObject.transform.parent = null;
                }
            }
            interacting = false;
        }

        IEnumerator BlendLayer(bool on)
        {
            if (on)
            {
                while (anim.GetLayerWeight(1) < 1)
                {
                    anim.SetLayerWeight(1, anim.GetLayerWeight(1) + (blendSpeed * Time.fixedDeltaTime));
                    yield return null;
                }
            }
            else
            {
                while (anim.GetLayerWeight(1) > 0)
                {
                    anim.SetLayerWeight(1, anim.GetLayerWeight(1) - (blendSpeed * Time.fixedDeltaTime));
                    yield return null;
                }
            }
        }
    }
}