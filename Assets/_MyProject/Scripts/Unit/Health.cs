using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private StateManager _state;
    [SerializeField] private Motion _motion;
    [SerializeField] private UnitVoice _unitVoice;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private GameObject _character;

    public ReactiveProperty<float> UnitHealht = new ReactiveProperty<float>();
    public ReactiveProperty<int> UnitLife = new ReactiveProperty<int>();

    public void TakeDamage(float amount, Vector3 hitPoint)
    {
        UnitHealht.Value += amount;
        _unitVoice.PlayVoice(EVoiceKind.Damage_A);

        OnLook(hitPoint);

        _state.UnitState.Value = EUnitState.Damage;
        _motion.SetState(33, EUnitState.Damage);
    }

    /// <summary>
    /// AnimationEvent用
    /// </summary>
    private void GetUp()
    {
        // 状態に応じて、ダウンやFreeに切り替え
        _motion.SetState(35);
    }
    /// <summary>
    /// AnimationEvent用
    /// </summary>
    private void DmgEnd()
    {
        // 状態に応じて、ダウンやFreeに切り替え
        _state.OnFreeControl();
        _motion.SetState(800);
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

    public async void Dead()
    {
        _character.SetActive(false);
        transform.position = Vector3.zero;
        _rigidbody.isKinematic = true;
        _capsuleCollider.enabled = false;
        UnitHealht.Value = 0;
        UnitLife.Value--;
        _unitVoice.PlayVoice(EVoiceKind.Dead_A, true);

        if (gameObject.TryGetComponent(out UnitInfo unitInfo))
        {
            var isDead = unitInfo.DeadLife();

            if(isDead)
            {
                GameManager.Instance.CheckDead();
            }
            else
            {
                await UniTask.Delay(3000);
                _rigidbody.isKinematic = false;
                _capsuleCollider.enabled = true;
                _character.SetActive(true);
            }
        }
    }
}
