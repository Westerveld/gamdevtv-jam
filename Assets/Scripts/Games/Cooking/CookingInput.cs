using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cooking
{
    public class CookingInput : MonoBehaviour
    {
        public Vector2 move;
        public bool interact;
        public bool sprint;
        public bool action;


        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnInteract(InputValue value)
        {
            interact = value.isPressed;
        }

        public void OnSprint(InputValue value)
        {
            sprint = value.isPressed;
        }

        public void OnAction(InputValue value)
        {
            action = value.isPressed;
        }
    }
}