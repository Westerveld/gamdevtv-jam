using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TwinStick
{
    public class TwinStickInput : MonoBehaviour
    {
        public Vector2 move;
        public Vector2 rotate;
        public bool shoot;
        
        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnRotate(InputValue value)
        {
            rotate = value.Get<Vector2>();
        }

        public void OnShoot(InputValue value)
        {
            Debug.Log(value.Get<float>());
            shoot = (value.Get<float>() > 0.5f);
        }
    }
}