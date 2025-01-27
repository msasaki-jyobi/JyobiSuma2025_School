using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;
using System;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input / InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<bool> PrimaryAttackEvent; // 攻撃判定
    public event Action StartedJumpEvent; // ジャンプ判定(押下時のみ）
    public event Action<bool> PrimaryJumpEvent; // ジャンプ判定
    public event Action<Vector2> MoveEvent; // 移動入力判定

    private InputSystem_Actions _control;

    private void OnEnable()
    {
        if (_control == null)
        {
            _control = new InputSystem_Actions();
            _control.Player.SetCallbacks(this);
        }

        _control.Player.Enable(); // キー入力検知可能に
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) // 入力されているなら
        {
            PrimaryAttackEvent?.Invoke(true);
        }
        else if (context.canceled) // 入力がキャンセルされた時
        {
            PrimaryAttackEvent?.Invoke(false);
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            StartedJumpEvent?.Invoke();
        }
        else if (context.performed) // 入力されているなら
        {
            PrimaryJumpEvent?.Invoke(true);
        }
        else if (context.canceled) // 入力がキャンセルされた時
        {
            PrimaryJumpEvent?.Invoke(false);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnNext(InputAction.CallbackContext context)
    {
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
    }
}
