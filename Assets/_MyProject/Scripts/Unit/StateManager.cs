using UniRx;
using UnityEngine;

public enum EUnitType
{
    Playable, // キャラクター
    Object, // オブジェクト
}

public enum EInputState
{
    Control,// 操作可能状態
    UnControl, // 操作不可能状態
}
public enum EUnitState
{
    Free, // 基本状態
    Damage, // ダメージ硬直
    Dead, // 死亡
    Cliff, // 崖捕まり中
    CliffUp, // 崖登り中
    Guard, // ガード中
    Escape, // 回避中
}

public class StateManager : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LineData _groundLineData;

    public ReactiveProperty<EInputState> InputState = new ReactiveProperty<EInputState>(); // キー入力可能な状態
    public ReactiveProperty<EUnitState> UnitState = new ReactiveProperty<EUnitState>(); // ユニットの状態
    public ReactiveProperty<bool> CanJump = new ReactiveProperty<bool>();

    private void Start()
    {
        // Events
        _inputReader.MoveEvent += OnMoveHandle;
        // Reactive
        UnitState
            .Subscribe((state) => 
            {
                switch(state)
                {
                    case EUnitState.Damage:
                        InputState.Value = EInputState.UnControl;
                        break;
                    case EUnitState.Dead:
                        InputState.Value = EInputState.UnControl;
                        break;
                }
            });
        InputState
           .Subscribe((input) =>
           {
               switch (input)
               {
                   case EInputState.Control:
                       break;
                   case EInputState.UnControl:
                       // 入力値のリセット
                       break;
               }
           });
    }

    private void Update()
    {
        CheckGround();
    }

    private void OnMoveHandle(Vector2 input)
    {
        Debug.Log($"移動 入力値:{input}");
    }

    private void CheckGround()
    {
        CanJump.Value = UtilityFunction.CheckLineData(_groundLineData, transform);
    }
}
