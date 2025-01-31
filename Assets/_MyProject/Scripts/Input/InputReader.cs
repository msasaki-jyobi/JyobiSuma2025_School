using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;

public class InputReader : MonoBehaviour, IPlayerActions
{
    public event Action<bool> PrimaryAttackEvent;
    public event Action StartedJumpEvent;
    public event Action<bool> PrimaryJumpEvent;
    public event Action<Vector2> MoveEvent;
    public event Action CanceledMoveEvent;
    public event Action<bool> PrimaryAimEvent;

    public InputSystem_Actions Control { get; private set; }

    private bool _keyboardOnly;

    public bool KeyboardOnly
    {
        get => _keyboardOnly;
        set
        {
            if (_keyboardOnly != value)
            {
                _keyboardOnly = value;
                SetupInputDevices();  // キーボード/ゲームパッドの切り替え
            }
        }
    }

    private void OnEnable()
    {
        if (Control == null)
        {
            Control = new InputSystem_Actions();
            Control.Player.SetCallbacks(this);
        }

        SetupInputDevices();
        Control.Player.Enable();
    }

    private void SetupInputDevices()
    {
        List<InputDevice> devices = new List<InputDevice>();

        if (_keyboardOnly || Gamepad.all.Count == 0)
        {
            devices.Add(Keyboard.current);
            devices.Add(Mouse.current);
        }
        else
        {
            devices.Add(Gamepad.current);
        }

        Control.devices = new ReadOnlyArray<InputDevice>(devices.ToArray());
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        PrimaryAttackEvent?.Invoke(context.performed);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            StartedJumpEvent?.Invoke();
        PrimaryJumpEvent?.Invoke(context.performed);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.performed ? context.ReadValue<Vector2>() : Vector2.zero);

        if (context.canceled)
            CanceledMoveEvent?.Invoke();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        PrimaryAimEvent?.Invoke(context.performed);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
    }

    public void OnNext(InputAction.CallbackContext context)
    {
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
    }
}






//using UnityEngine;
//using UnityEngine.InputSystem;
//using static InputSystem_Actions;
//using System;

//[CreateAssetMenu(fileName = "InputReader", menuName = "Input / InputReader")]
//public class InputReader : ScriptableObject, IPlayerActions 
//{
//    public event Action<bool> PrimaryAttackEvent; // 攻撃判定
//    public event Action StartedJumpEvent; // ジャンプ判定(押下時のみ）
//    public event Action<bool> PrimaryJumpEvent; // ジャンプ判定
//    public event Action<Vector2> MoveEvent; // 移動入力判定
//    public event Action<bool> PrimaryAimEvent;

//    public InputSystem_Actions Control { get; private set; }

//    private void OnEnable()
//    {
//        if (Control == null)
//        {
//            Control = new InputSystem_Actions();
//            Control.Player.SetCallbacks(this);
//        }

//        Control.Player.Enable(); // キー入力検知可能に
//    }

//    public void OnAttack(InputAction.CallbackContext context)
//    {
//        if (context.performed) // 入力されているなら
//        {
//            PrimaryAttackEvent?.Invoke(true);
//        }
//        else if (context.canceled) // 入力がキャンセルされた時
//        {
//            PrimaryAttackEvent?.Invoke(false);
//        }
//    }

//    public void OnCrouch(InputAction.CallbackContext context)
//    {
//        //throw new NotImplementedException();
//    }

//    public void OnInteract(InputAction.CallbackContext context)
//    {
//    }

//    public void OnJump(InputAction.CallbackContext context)
//    {
//        if(context.started)
//        {
//            StartedJumpEvent?.Invoke();
//        }
//        else if (context.performed) // 入力されているなら
//        {
//            PrimaryJumpEvent?.Invoke(true);
//        }
//        else if (context.canceled) // 入力がキャンセルされた時
//        {
//            PrimaryJumpEvent?.Invoke(false);
//        }
//    }

//    public void OnLook(InputAction.CallbackContext context)
//    {
//    }

//    public void OnMove(InputAction.CallbackContext context)
//    {
//        if (context.performed) // キーが押されている間呼び続ける
//        {
//            Vector2 movement = context.ReadValue<Vector2>();
//            Debug.Log($"Move input: {movement}");
//            MoveEvent?.Invoke(movement);
//        }
//        else if (context.canceled) // キーが離されたらゼロベクトルを送る
//        {
//            Debug.Log("Move input canceled");
//            MoveEvent?.Invoke(Vector2.zero);
//        }
//    }

//    public void OnNext(InputAction.CallbackContext context)
//    {
//    }

//    public void OnPrevious(InputAction.CallbackContext context)
//    {
//    }

//    public void OnSprint(InputAction.CallbackContext context)
//    {
//    }

//    public void OnAim(InputAction.CallbackContext context)
//    {
//        if (context.performed) // 入力されているなら
//        {
//            PrimaryAimEvent?.Invoke(true);
//        }
//        else if (context.canceled) // 入力がキャンセルされた時
//        {
//            PrimaryAimEvent?.Invoke(false);
//        }
//    }
//}
