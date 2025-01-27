using UniRx;
using UnityEngine;

public enum EUnitType
{
    Playable,
    Object,
}

public enum EInputState
{
    Control,
    UnControl,
}
public enum EUnitState
{
    Free,
    Move,
    Damage,
    Dead,
    Cliff,
    CliffUp,
    Guard,
    Escape,
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
