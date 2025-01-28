using UnityEngine;
using UniRx;

public class Gravity : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Health _health;

    [SerializeField] private float _weight = 600f;
    [SerializeField] private float _dampingSpeed = 5f; // 外力の減衰スピード
    [SerializeField] private float _forceMultiplier = 2f; // 外力の強さ(高いほど吹き飛ぶ）
    [SerializeField] private float _forceResistance = 12f; // 吹き飛びの抵抗力


    private float _defaultForceMultiplier;
    private Vector3 _externalForce; // 外力を保持する変数
    private Vector3 _inputVelocity; // 入力による移動ベクトル

    private void Start()
    {
        _defaultForceMultiplier = _forceMultiplier; // 初期の吹き飛びやすさを保持

        if (_health != null)
            _health.UnitHealht
                .Subscribe((x) =>
                {
                    // 吹き飛びやすさをデフォルト値＋ダメージ量に変更
                    _forceMultiplier =
                     _defaultForceMultiplier + (_health.UnitHealht.Value / _forceResistance);
                });
    }

    private void FixedUpdate()
    {
        // 外力を減衰させる（徐々に0に近づく）
        _externalForce = Vector3.Lerp(_externalForce, Vector3.zero, Time.fixedDeltaTime * _dampingSpeed);

        // 入力による移動と外力を組み合わせてVelocityを設定
        Vector3 newVelocity =
            _inputVelocity + _externalForce;
        _rigidbody.linearVelocity = newVelocity;

        // 重力を追加
        if (_weight > 0)
        {
            _rigidbody.AddForce(Vector3.down * _weight * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    // 外力を加えるためのメソッド
    public void AddExternalForce(Vector3 force, float upMultiplier = 1f)
    {
        _externalForce += force * _forceMultiplier;
        _externalForce.y = 0;
        var upForce =
            transform.up * force.y * upMultiplier * (_health.UnitHealht.Value / _forceResistance);

        _rigidbody.AddForce(upForce, ForceMode.Impulse);
    }

    public void SetInput(Vector3 input)
    {
        _inputVelocity = input;
    }
}
