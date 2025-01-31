using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;
using System;
using Cysharp.Threading.Tasks;

public class Movement : InputBase
{
    [SerializeField] private FlickDetector _flickDetector;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private StateManager _state;
    [SerializeField] private Gravity _gravity;

    [field: SerializeField] public float WalkPower { get; private set; } = 6f;
    [SerializeField] private float _dashRange = 3f;
    [SerializeField] private float _maxSlopeAngle = 45f;
    [SerializeField] private float _slopeDistance = 0.2f;

    private Camera _camera;
    private float _inputX;
    private float _inputY;

    private Vector3 _inputVelocity; // 入力による移動ベクトル
    private Quaternion _targetRotation; // 入力による回転ベクトル

    private float _rotateSpeed = 20000f;
    private float _defaultWalkPower;

    private InputAction _moveAction;

    protected override void Start()
    {
        base.Start();

        _camera = Camera.main;
        _defaultWalkPower = WalkPower;

        _moveAction = _inputReader.Control.Player.Move;
        _inputReader.CanceledMoveEvent += OnCanceledHandle;
    }

    private void Update()
    {
        if (!UnInputCheck())
            InputReset();
        Move();
        Rotation();

    }

    private void OnCanceledHandle()
    {
        if (!_flickDetector.IsFlicking)
            WalkPower = _defaultWalkPower;
    }

    private bool UnInputCheck()
    {
        // 現在の入力値を取得
        Vector2 input = _moveAction.ReadValue<Vector2>();
        _inputX = input.x;
        _inputY = input.y;

        bool check = true;
        check = check && _state.InputState.Value == EInputState.Control;
        //check = check && _state.UnitState.Value == EUnitState.Free;
        return check;
    }

    /// <summary>
    /// ユニットの移動制御
    /// </summary>
    private void Move()
    {
        // 倒した方向が歩ける角度
        if (CheckSloopAngle())
        {
            Vector3 velocity = new Vector3(_inputVelocity.x * WalkPower, 0, _inputVelocity.z * WalkPower);
            _gravity.SetInput(velocity);
        }
    }
    /// <summary>
    /// ユニットの回転制御
    /// </summary>
    private void Rotation()
    {
        float rotY = _camera.transform.rotation.eulerAngles.y;

        // カメラから見て向く方角を計算
        var tpsHorizontalRotation = Quaternion.AngleAxis(rotY, Vector3.up);
        //_inputVelocity = tpsHorizontalRotation * new Vector3(_inputX, 0, _inputY).normalized;
        _inputVelocity = tpsHorizontalRotation * new Vector3(_inputX, 0, 0).normalized;
        var rotationSpeed = _rotateSpeed * Time.deltaTime;

        // 移動方向を向く
        if (_inputVelocity.magnitude > 0.5f)
            _targetRotation = Quaternion.LookRotation(_inputVelocity, Vector3.up);

        // 速度をAnimatorに反映する
        _animator?.SetFloat("Speed", _inputVelocity.magnitude, 0.1f, Time.deltaTime);

        if (Mathf.Abs(_inputX) > 0)
            // なめらかに振り向く
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, rotationSpeed);
    }

    /// <summary>
    /// 角度を検知して移動可能か判定
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public bool CheckSloopAngle()
    {
        float maxSlopeAngle = _maxSlopeAngle;
        float downwardAngle = 25f; // この値を調整して、Rayの下向きの角度を変更
        Vector3 forwardDown = (transform.forward - Vector3.up * Mathf.Tan(downwardAngle * Mathf.Deg2Rad)).normalized;
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, forwardDown);

        float rayDistance = _slopeDistance; // Rayの長さ
        Color rayColor = Color.blue; // Rayの色
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, rayColor);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            return angle <= maxSlopeAngle ? true : false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 入力値をすべてリセット
    /// </summary>
    private void InputReset()
    {
        _inputX = 0;
        _inputY = 0;
        _inputVelocity = Vector3.zero;
    }
}
