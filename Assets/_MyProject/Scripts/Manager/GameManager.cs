using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private TextMeshProUGUI _centerTextUGUI;
    public Transform CharacterContent;
    public GameObject CharacterUIPrefab;
    public List<UnitInfo> UnitInfos = new List<UnitInfo>();
    public int LifeNum = 3;
    public AudioClip StageBGM;
    public AudioClip FinishVoice;


    private void Start()
    {
        AudioManager.Instance.ChangeBGM(StageBGM);
        OnChangeControlPlayers(false);

        foreach (UnitInfo info in UnitInfos)
        {
            var characterUIObject = Instantiate(CharacterUIPrefab, CharacterContent);
            if (characterUIObject.gameObject.TryGetComponent(out CharacterUI characterUI))
            {
                info.SetCharacterUI(characterUI);
                characterUI.SetUnitInfo(info, LifeNum);
                if (info.TryGetComponent(out Health health))
                    health.UnitLife.Value = LifeNum;
            }
        }
    }

    public void OnChangeControlPlayers(bool flg)
    {
        foreach (UnitInfo info in UnitInfos)
        {
            if(info.TryGetComponent(out StateManager state))
            {
                state.InputState.Value = flg ? EInputState.Control: EInputState.UnControl;
            }
        }
    }

    public async void CheckDead()
    {
        var winPlayers = "";
        foreach (UnitInfo info in UnitInfos)
        {
            if(info.TryGetComponent(out Health health))
            {
                if (health.UnitLife.Value > 0) winPlayers += $"{health.name} \n ";
            }
        }
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayOneShot(FinishVoice, EAudioType.Se);
        OnChangeControlPlayers(false);
        _centerTextUGUI.text = $"Finish!!";
        await UniTask.Delay(1000);
        _centerTextUGUI.text = $"{winPlayers} Win !!";
        await UniTask.Delay(3000);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
