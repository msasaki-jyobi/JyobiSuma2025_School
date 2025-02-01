using Cysharp.Threading.Tasks;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    [Header("下記は自動入力される為 設定不要")]
    [SerializeField] private GameObject _attaker;

    [SerializeField] private float _damageAmount;
    [SerializeField] private float _forcePower = 10f; // 吹き飛ばす力の強さ
    [SerializeField] private float _forceUpMultiplier = 2.5f; // 上へ吹き飛ばす力の強さ

    [SerializeField] private GameObject _enableEffect;
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private AudioClip _enableSE;
    [SerializeField] private AudioClip _hitSE;

    [Space(10)]
    // 生成する魔法とか
    [SerializeField] private bool _unCollider;
    [SerializeField] private GameObject _enableCreateMagic;
    [SerializeField] private float _lifeTime = 5f;


    private async void OnEnable()
    {
        // 効果音を鳴らす
        AudioManager.Instance.PlayOneShot(_enableSE, EAudioType.Se);
        UtilityFunction.PlayEffect(gameObject, _enableEffect);

        await UniTask.Delay(10);

        // 魔法生成
        var magic = UtilityFunction.PlayEffect(gameObject, _enableCreateMagic, destroyTime: _lifeTime);
        if (magic != null)
            if (magic.TryGetComponent(out HitCollider hitCollider))
            {
                if (_attaker != null)
                {
                    magic.transform.rotation = _attaker.transform.rotation;
                    hitCollider._attaker = _attaker;
                }
            }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // オブジェクトと衝突座標を渡す
        OnHit(collision.gameObject, collision.contacts[0].point);
    }

    private void OnTriggerEnter(Collider other)
    {
        // オブジェクトと衝突座標を渡す
        OnHit(other.gameObject, other.ClosestPoint(transform.position));
    }

    private void OnHit(GameObject hit, Vector3 contactPoint)
    {
        if (hit == _attaker) return; // 自身ならReturn

        if(hit.TryGetComponent(out Health health))
        {
            // ダメージ処理
            health.TakeDamage(_damageAmount, contactPoint);

            // HitEffect
            UtilityFunction.PlayEffect(hit, _hitEffect);

            // 効果音を鳴らす
            AudioManager.Instance.PlayOneShot(_hitSE, EAudioType.Se);

            // 移動可能オブジェクトなら吹き飛ばす
            if (hit.TryGetComponent(out Gravity gravity))
            {
                // 衝突位置からオブジェクトの方向を計算
                Vector3 forceDirection = (hit.transform.position - contactPoint).normalized;
                gravity.AddExternalForce(forceDirection * _forcePower, _forceUpMultiplier);
            }
        }
    }

    /// <summary>
    /// 外部からコライダー情報を上書き
    /// </summary>
    /// <param name="attackSettings"></param>
    public void OnSetParameter(AttackSettings attackSettings)
    {
        _damageAmount = attackSettings.DamageAmount;
        _forcePower = attackSettings.ForcePower;
        _forceUpMultiplier = attackSettings.ForceUpMultiplier;
        _enableEffect = attackSettings.EnableEffect;
        _hitEffect = attackSettings.HitEffect;
        _enableSE = attackSettings.EnableSE;
        _hitSE = attackSettings.HitSE;

        _unCollider = attackSettings.UnCollider;
        _enableCreateMagic = attackSettings.EnableMagicPrefab;
        _lifeTime = attackSettings.LifeTime;

        if (TryGetComponent(out Collider collider))
            collider.enabled = !_unCollider;
    }
}
