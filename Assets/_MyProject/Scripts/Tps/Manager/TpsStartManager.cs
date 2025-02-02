using Cysharp.Threading.Tasks;
using UnityEngine;

public class TpsStartManager : MonoBehaviour
{
    private async void Start()
    {
        var player = TpsGameManager.Instance.Player;

        if (player.TryGetComponent(out StateManager state))
            state.InputState.Value = EInputState.UnControl;

        // ゲーム開始演出

        await UniTask.Delay(1000);
        state.InputState.Value = EInputState.Control;
    }
}
