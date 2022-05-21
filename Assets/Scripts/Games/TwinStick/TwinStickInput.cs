using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        shoot = value.isPressed;
    }
}
