using UnityEngine;

public class HitCollider : MonoBehaviour
{
    [SerializeField] private GameObject _attaker;

    [SerializeField] private float _forcePower = 10f; // 吹き飛ばす力の強さ
    [SerializeField] private float _forceUpMultiplier = 2.5f; // 上へ吹き飛ばす力の強さ
    [SerializeField] private float _damageAmount;

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

            // 移動可能オブジェクトなら吹き飛ばす
            if (hit.TryGetComponent(out Gravity gravity))
            {
                // 衝突位置からオブジェクトの方向を計算
                Vector3 forceDirection = (hit.transform.position - contactPoint).normalized;
                gravity.AddExternalForce(forceDirection * _forcePower, -_forceUpMultiplier);
            }
        }
    }
}
