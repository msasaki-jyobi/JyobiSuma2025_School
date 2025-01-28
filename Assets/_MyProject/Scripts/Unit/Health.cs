using UniRx;
using UnityEngine;

public class Health : MonoBehaviour
{
    public ReactiveProperty<float> UnitHealht = new ReactiveProperty<float>();

    public void TakeDamage(float amount)
    {
        UnitHealht.Value += amount;
    }
}
