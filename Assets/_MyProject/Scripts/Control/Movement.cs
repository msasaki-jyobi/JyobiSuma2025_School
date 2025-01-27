using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private StateManager _state;

    [field: SerializeField] public float WalkPower { get; private set; } = 6f;
    [SerializeField] private float _weight = 600f;
    [SerializeField] private float _forceMultiplier = 2f; // 外力の強さ(高いほど吹き飛ぶ）
    [SerializeField] private float _dampingSpeed = 5f; // 外力の減衰スピード
    [SerializeField] private float _maxSlopeAngle = 45f;
    [SerializeField] private float _slopeDistance = 0.2f;

    private Camera _camera;
    private float _inputX;
    private float _inputY;
    private Vector3 _inputVelocity; // 入力による移動ベクトル
    private Quaternion _targetRotation; // 入力による回転ベクトル
    private Vector3 _externalForce; // 外力を保持する変数
    private float _rotateSpeed = 20000f;

    private void Start()
    {
        _camera = Camera.main;
        _inputReader.MoveEvent += OnMoveHandle;
    }
    private void Update()
    {
        Rotation();
    }
    private void FixedUpdate()
    {
        Move();
    }
    /// <summary>
    /// キー入力の移動値を取得するメソッド
    /// </summary>
    /// <param name="input"></param>
    private void OnMoveHandle(Vector2 input)
    {
        if (_state.InputState.Value == EInputState.UnControl) return;
        _inputX = input.x;
        _inputY = input.y;
    }
    /// <summary>
    /// ユニットの移動制御
    /// </summary>
    private void Move()
    {
        // 外力を減衰させる（徐々に0に近づく）
        _externalForce = Vector3.Lerp(_externalForce, Vector3.zero, Time.fixedDeltaTime * _dampingSpeed);
        // 移動可能な角度かチェック
        if (CheckSloopAngle())
        {
            // 入力による移動と外力を組み合わせてVelocityを設定
            Vector3 newVelocity =
                new Vector3(_inputVelocity.x * WalkPower, _rigidbody.velocity.y, _inputVelocity.z * WalkPower) + _externalForce;
            _rigidbody.velocity = newVelocity;
        }
        else
        {
            // 角度が不適切な場合、外力のみ適用して移動なし
            _rigidbody.velocity = new Vector3(_externalForce.x, _rigidbody.velocity.y, _externalForce.z);
        }
        // 重力を追加（必要であれば）
        if (_weight > 0)
        {
            _rigidbody.AddForce(Vector3.down * _weight * Time.fixedDeltaTime, ForceMode.Acceleration);
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
        // なめらかに振り向く
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, rotationSpeed);

        // 速度をAnimatorに反映する
        _animator?.SetFloat("Speed", _inputVelocity.magnitude, 0.1f, Time.deltaTime);
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

    // 外力を加えるためのメソッド
    public void AddExternalForce(Vector3 force)
    {
        _externalForce += force * _forceMultiplier;
        _externalForce.y = 0;

        _rigidbody.AddForce(transform.up * force.y, ForceMode.Impulse);
    }

    /// <summary>
    /// 入力値をすべてリセット
    /// </summary>
    private void InputReset()
    {
        _inputVelocity = Vector3.zero;
    }
}
