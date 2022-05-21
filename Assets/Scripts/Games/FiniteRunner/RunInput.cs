using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runner
{
    public class RunInput : MonoBehaviour
    {
        public bool slide;
        public bool jump;


        public void OnJump(InputValue value)
        {
            jump = value.isPressed;
        }

        public void OnSlide(InputValue value)
        {
            slide = value.isPressed;
        }
    }
}