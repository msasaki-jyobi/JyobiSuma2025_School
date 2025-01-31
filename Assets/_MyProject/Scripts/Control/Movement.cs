using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;

public class Movement : InputBase
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private StateManager _state;
    [SerializeField] private Gravity _gravity;

    [field: SerializeField] public float WalkPower { get; private set; } = 6f;
    [SerializeField] private float _maxSlopeAngle = 45f;
    [SerializeField] private float _slopeDistance = 0.2f;

    private Camera _camera;
    private float _inputX;
    private float _inputY;

    private Vector3 _inputVelocity; // 入力による移動ベクトル
    private Quaternion _targetRotation; // 入力による回転ベクトル

    private float _rotateSpeed = 20000f;

    private InputAction _moveAction;

    protected override void Start()
    {
        base.Start();
        _camera = Camera.main;
        _moveAction = _inputReader.Control.Player.Move;
    }
    private void Update()
    {
        if (!OnInputCheck())
            InputReset();
        Move();
        Rotation();

    }
    /// <summary>
    /// キー入力の移動値を取得するメソッド
    /// </summary>
    /// <param name="input"></param>
    private void OnMoveHandle(Vector2 input)
    {
        _inputX = input.x;
        _inputY = input.y;
    }

    private bool OnInputCheck()
    {
        // 現在の入力値を取得
        Vector2 input = _moveAction.ReadValue<Vector2>();
        OnMoveHandle(input);

        bool check = true;
        check = check && _state.InputState.Value == EInputState.Control;
        check = check && _state.UnitState.Value == EUnitState.Free;
        return check;
    }

    /// <summary>
    /// ユニットの移動制御
    /// </summary>
    private void Move()
    {
        // 倒した方向が歩ける角度なら
        if (CheckSloopAngle())
        {
            Vector3 velocity = new Vector3(_inputVelocity.x * WalkPower, _rigidbody.linearVelocity.y, _inputVelocity.z * WalkPower);
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
        _inputVelocity = tpsHorizontalRotation * new Vector3(_inputX, 0, _inputY).normalized;
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
