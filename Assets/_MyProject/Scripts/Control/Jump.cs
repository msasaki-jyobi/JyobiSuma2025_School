using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;

public class Jump : InputBase
{


    [SerializeField] private Animator _animator;
    [SerializeField] private StateManager _state;
    [SerializeField] private Rigidbody _rigidbody;
    public ReactiveProperty<bool> IsJump = new ReactiveProperty<bool>();
    [Header("ジャンプ回数の上限 / ジャンプ力")]
    [SerializeField] private int _jumpLimit = 2;
    [SerializeField] private float _jumpPower = 10f;


    private int _jumpCount;

    protected override void Start()
    {
        base.Start();
        _inputReader.StartedJumpEvent += OnStartedJumpEvent;
        _inputReader.PrimaryJumpEvent += OnPrimaryJumpEvent;
    }
    private void Update()
    {
        if (_state.CanJump.Value)
        {
            if (!IsJump.Value)
                _jumpCount = 0;
        }
    }
    private void OnStartedJumpEvent()
    {
        OnJump();
    }
    private void OnPrimaryJumpEvent(bool input)
    {

    }

    /// <summary>
    /// ユニットのジャンプを制御
    /// </summary>
    private async void OnJump()
    {
        if (_jumpCount < _jumpLimit)
        {
            IsJump.Value = true;
            // ジャンプ回数加算
            _jumpCount++;
            // Y軸速度リセット
            Vector3 velocity = _rigidbody.linearVelocity;
            velocity.y = 0;
            _rigidbody.linearVelocity = velocity;
            // ジャンプ処理
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.AddForce(transform.up * _jumpPower, ForceMode.Impulse);

            // ジャンプ音声
            //AudioManager.Instance.PlayOneShotClipData(JumpVoice);

            await UniTask.Delay(100);
            IsJump.Value = false;
        }
    }
}
