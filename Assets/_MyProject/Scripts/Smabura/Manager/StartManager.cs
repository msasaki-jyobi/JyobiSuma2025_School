using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _centerTextUGUI;
    public AudioClip StartVoice_Three;
    public AudioClip StartVoice_Two;
    public AudioClip StartVoice_One;
    public AudioClip StartVoice_Go;

    private async void Start()
    {
        await UniTask.Delay(1000);
        AudioManager.Instance.PlayOneShot(StartVoice_Three, EAudioType.Se);
        _centerTextUGUI.text = "3";

        await UniTask.Delay(1000);
        AudioManager.Instance.PlayOneShot(StartVoice_Two, EAudioType.Se);
        _centerTextUGUI.text = "2";

        await UniTask.Delay(1000);
        AudioManager.Instance.PlayOneShot(StartVoice_One, EAudioType.Se);
        _centerTextUGUI.text = "1";

        await UniTask.Delay(1000);
        AudioManager.Instance.PlayOneShot(StartVoice_Go, EAudioType.Se);
        _centerTextUGUI.text = "Go!!";
        GameManager.Instance.OnChangeControlPlayers(true);

        await UniTask.Delay(1000);
        _centerTextUGUI.text = "";
    }
}
