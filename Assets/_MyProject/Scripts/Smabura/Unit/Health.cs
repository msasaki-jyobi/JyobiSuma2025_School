using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static int DownFront = 400;


    [SerializeField] private StateManager _state;
    [SerializeField] private Motion _motion;
    [SerializeField] private UnitVoice _unitVoice;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private GameObject _character;

    [field: SerializeField] public int MaxHealth = 100;
    public ReactiveProperty<float> UnitHealht = new ReactiveProperty<float>();
    public ReactiveProperty<int> UnitLife = new ReactiveProperty<int>(); // 残機用

    private int _getupState;
    private bool _isDead;

    private void Start()
    {
        UnitHealht.Value = MaxHealth;

        UnitHealht
            .Subscribe(async (health) => 
            {
               
            });
    }

    public void TakeDamage(float amount, Vector3 hitPoint, int damageState)
    {
        UnitHealht.Value -= amount; // ダメージ
        _unitVoice.PlayVoice(EVoiceKind.Damage_A); // ボイス再生

        // hitPointに応じてダメージモーション変更？

        //OnLook(hitPoint); // ダメージの方角を向く
        SetDamageState(damageState); // ダメージステート処理
    }

    private void SetDamageState(int damageState)
    {
        _motion.SetState(damageState, EUnitState.Damage);

        if (_isDead) // 死亡中
        {

        }

        switch (damageState)
        {
            case 1000: // Damage1
                break;
            case 1001: // Damage
                break;
            case 1100: // Down1(Front)
                _getupState = DownFront;
                break;
            case 1101: // Down1(Front)
                _getupState = DownFront;
                break;
        }
    }

    /// <summary>
    /// AnimationEvent用：起き上がる
    /// </summary>
    private async void GetUp(int seconds)
    {
        if (_isDead) return;
        InitDead();

        await UniTask.Delay(seconds);
        // 状態に応じて、ダウンやFreeに切り替え
        _motion.SetState(_getupState);
    }
    /// <summary>
    /// AnimationEvent用：操作可能状態に戻る
    /// </summary>
    private void DmgEnd()
    {
        if (_isDead) return;
        InitDead();

        // 状態に応じて、ダウンやFreeに切り替え
        _state.OnFreeControl();
        _motion.SetState(StateManager.IdleState);
    }

    private async void InitDead()
    {
        if (UnitHealht.Value <= 0)
        {
            if (!_isDead) // 初死亡時
            {
                _isDead = true;
                if (_state.UnitType.Value == EUnitType.Playable)
                {
                    await UniTask.Delay(1000);
                    // のけぞり系ダメージなら強制的にダウンダメージ
                    SetDamageState(1100);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
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

    
    public async void SmashDead()
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

            if (isDead)
            {
                GameManager.Instance.GameEnd();
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
