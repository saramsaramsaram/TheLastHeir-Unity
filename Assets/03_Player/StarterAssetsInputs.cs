using UnityEngine;
using UnityEngine.InputSystem;

public class StarterAssetsInputs : MonoBehaviour
{
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool roll;
    public bool attack;
    public bool heavyAttack;

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        jump = value.isPressed;
    }

    public void OnSprint(InputValue value)
    {
        sprint = value.isPressed;
    }

    public void OnRoll(InputValue value)
    {
        roll = value.isPressed;
    }

    public void OnAttack(InputValue value)
    {
        attack = value.isPressed;
    }

    public void OnHeavyAttack(InputValue value)
    {
        heavyAttack = value.isPressed;
    }
} 