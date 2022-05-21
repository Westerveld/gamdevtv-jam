using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoulsInput : MonoBehaviour
{
    public Vector2 move;
    public bool jump; 
    public bool dodge;
    public bool attack;
    public bool sprint;


    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        jump = value.isPressed;
    }

    public void OnDodge(InputValue value)
    {
        dodge = value.isPressed;
    }

    public void OnAttack(InputValue value)
    {
        attack = value.isPressed;
    }

    public void OnSprint(InputValue value)
    {
        sprint = value.isPressed;
    }
}
