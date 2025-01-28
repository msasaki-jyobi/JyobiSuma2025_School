using UniRx;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private StateManager _state;
    [SerializeField] private Motion _motion;

    public ReactiveProperty<float> UnitHealht = new ReactiveProperty<float>();

    public void TakeDamage(float amount, Vector3 hitPoint)
    {
        UnitHealht.Value += amount;

        OnLook(hitPoint);

        _state.UnitState.Value = EUnitState.Damage;
        _motion.SetState(23, EUnitState.Damage);
    }

    /// <summary>
    /// AnimationEvent用
    /// </summary>
    public void OnDamageEnd()
    {
        // 状態に応じて、ダウンやFreeに切り替え
        _motion.SetState(800, EUnitState.Free);

    }

    private void OnLook(Vector3 hitPoint)
    {
        // 自分の位置とhitPointから水平方向の角度を計算
        Vector3 direction = hitPoint - transform.position;
        direction.y = 0; // Y軸成分を無視して水平方向のみを考慮

        if (direction.sqrMagnitude > 0.001f) // 距離がほぼ0でない場合
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0); // Y軸の回転だけ適用
        }
    }
}
