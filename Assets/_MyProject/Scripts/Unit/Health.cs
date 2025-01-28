using UniRx;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private StateManager _state;
    [SerializeField] private Motion _motion;

    public ReactiveProperty<float> UnitHealht = new ReactiveProperty<float>();

    public void TakeDamage(float amount)
    {
        UnitHealht.Value += amount;

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
}
