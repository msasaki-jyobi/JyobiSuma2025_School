using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Attack : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private StateManager _state;
    [SerializeField] private Motion _motion;
    [SerializeField] private List<GameObject> _attackCollider;

    private float _inputX;
    private float _inputY;

    private void Start()
    {
        _inputReader.MoveEvent += OnMoveHandle;
        _inputReader.PrimaryAttackEvent += OnPrimaryAttackHandle;
    }

    private void OnMoveHandle(Vector2 vector)
    {
        vector.x = _inputX;
        vector.y = _inputY;
    }

    private void OnPrimaryAttackHandle(bool input)
    {
        if (input)
        {
            PlayAction("A");
        }
    }

    private void Update()
    {

    }

    private void PlayAction(string keyType)
    {
        // 攻撃発動可能か？
        bool check = true;
        check = _state.UnitState.Value == EUnitState.Free; // 自由操作受付状態か？

        if (check) // 条件を満たした場合
        {
            var state = GetAttackState(keyType);
            _motion.SetState(state, EUnitState.Action);

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

        // 10加算で空中攻撃
        if (!_state.CanJump.Value)
            state += 10;

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
    private void OnAtkStart(int slotNum)
    {
        if (slotNum < _attackCollider.Count)
            _attackCollider[slotNum].SetActive(true);

    }
    /// <summary>
    /// AnimationEvent用：指定スロットの攻撃判定OFF
    /// </summary>
    private void OnAtkEnd(int slotNum)
    {
        if (slotNum < _attackCollider.Count)
            _attackCollider[slotNum].SetActive(false);
    }
    /// <summary>
    /// AnimationEvent用：アニメーションの終了
    /// </summary>
    private void OnFinish()
    {
        // 操作可能状態に戻る
        _motion.SetState(800);
        _state.UnitState.Value = EUnitState.Free;
    }
}
