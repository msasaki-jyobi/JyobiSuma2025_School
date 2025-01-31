using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Windows;

public class Attack : InputBase
{
    [SerializeField] private StateManager _state;
    [SerializeField] private Motion _motion;
    [SerializeField] private Gravity _gravity;
    [SerializeField] private FlickDetector _flickDetector;
    [SerializeField] private GameObject _smashEffect;

    [SerializeField] private List<GameObject> _attackCollider;

    private ReactiveProperty<bool> _isSmash = new ReactiveProperty<bool>();
    private Vector2 _direction;
    private int _smashState = 1;

    private float _inputX;
    private float _inputY;

    protected override void Start()
    {
        base.Start();
        _inputReader.MoveEvent += OnMoveHandle;
        _inputReader.PrimaryAttackEvent += OnPrimaryAttackHandle;

        _flickDetector.OnFlickDetected += OnFlickHandle;

        _isSmash
            .Subscribe((x) =>
            {
                if (x)
                    _state.InputState.Value = EInputState.UnControl;

            });
    }

    private void OnMoveHandle(Vector2 vector)
    {
        _inputX = vector.x;
        _inputY = vector.y;
    }

    private void OnPrimaryAttackHandle(bool input)
    {
        // 攻撃ボタン押しました
        // 攻撃可能です
        // はじき入力中？
        if (input)
        {
            if (_state.InputState.Value == EInputState.UnControl) return; // 操作不能ならReturn
            if (_state.UnitState.Value != EUnitState.Free) return; // 自由操作可能じゃなければReturn

            PlayAction("A");
        }
        else
        {
            if (_isSmash.Value) // スマッシュアクション中にキーを離したら
            {
                _motion.SetState(_smashState); // スマッシュモーション
                _isSmash.Value = false;
                _smashEffect.SetActive(false);
            }
        }
    }
    private void OnFlickHandle(Vector2 vector)
    {
        _direction = vector;
    }

    private void PlayAction(string keyType)
    {
        var state = GetAttackState(keyType);

        if (_state.CanJump.Value)
        {
            _motion.SetApplyRootMotion(true);
            _gravity.IsUnGravity = true;
        }

        if (!_isSmash.Value) // スマッシュアクションじゃない
        {
            if (_flickDetector.IsFlicking) // はじき入力中
            {
                _isSmash.Value = true; // スマッシュアクションフラグON
                _motion.SetState(500); // スマッシュアクション実施
                _smashState = state; // スマッシュ技を確定させる
                _state.InputState.Value = EInputState.UnControl;
                if (_smashEffect != null)
                    _smashEffect.SetActive(true);
            }
            else // 通常攻撃アクション
            {
                _motion.SetState(state, EUnitState.Action);
                if (_state.CanJump.Value)
                    _state.InputState.Value = EInputState.UnControl;
            }
        }
    }
    /// <summary>
    /// 入力に応じた技を確定
    /// </summary>
    /// <param name="keyType"></param>
    /// <returns></returns>
    private int GetAttackState(string keyType)
    {
        var moveDirection = GetDominantDirection();
        var state = 0;

        // A or B
        if(moveDirection == "Idle")
           state = keyType == "A" ? 0 : 5;
        else if (moveDirection == "Down")
            state = keyType == "A" ? 1 : 6;
        else if (moveDirection == "Left")
            state = keyType == "A" ? 2 : 7;
        else if (moveDirection == "Up")
            state = keyType == "A" ? 3 : 8;
        else if (moveDirection == "Right")
            state = keyType == "A" ? 4 : 9;

        // 空中なら+10
        if (!_state.CanJump.Value)
            state += 10;
        else if(_flickDetector.IsFlicking)
        {
            // 地面かつスマッシュなら+15
            state += 25;
            if (moveDirection == "Idle")
                state -= 25;
        }

        return state;

    }
    /// <summary>
    /// 入力方向の中で一番大きな方向を取得
    /// </summary>
    /// <returns></returns>
    private string GetDominantDirection()
    {
        // 入力値の絶対値を取得
        float absX = Mathf.Abs(_inputX);
        float absY = Mathf.Abs(_inputY);

        // 無入力（Idle）判定
        if (absX < 0.1f && absY < 0.1f) // 小さな値を無視
            return "Idle";

        // 上下左右の比較
        if (absY >= absX) // 上下が優勢
            return _inputY > 0 ? "Up" : "Down";
        else // 左右が優勢
            return _inputX > 0 ? "Right" : "Left";
    }

    /// <summary>
    /// AnimationEvent用：指定スロットの攻撃判定ON
    /// </summary>
    /// <param name="slotNum"></param>
    private void Hit(int slotNum)
    {

        if (slotNum < _attackCollider.Count)
            _attackCollider[slotNum].SetActive(true);

    }
    /// <summary>
    /// AnimationEvent用：指定スロットの攻撃判定OFF
    /// </summary>
    private void HitEnd(int slotNum)
    {
        if (slotNum < _attackCollider.Count)
            _attackCollider[slotNum].SetActive(false);
    }
    /// <summary>
    /// AnimationEvent用：アニメーションの終了
    /// </summary>
    private void AtkEnd()
    {
        _motion.SetState(800);
        _gravity.IsUnGravity = false;
        _state.OnFreeControl();
        _motion.SetApplyRootMotion(false);
    }
}
