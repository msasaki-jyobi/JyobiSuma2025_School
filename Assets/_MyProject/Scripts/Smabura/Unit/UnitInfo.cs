using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UniRx;

public class UnitInfo : MonoBehaviour
{
    [SerializeField] private Health _health;
    [field: SerializeField, Header("表示名 / 顔・残機アイコン / ロゴ / キャラクターUIの色")] public string UnitName { get; private set; }
    [field: SerializeField] public Sprite FaceSprite { get; private set; }
    [field: SerializeField] public Sprite IconSprite { get; private set; }
    [field: SerializeField] public Color UnitColor { get; private set; } = Color.white;

    private CharacterUI _characterUI;

    private void Start()
    {
        _health.UnitHealht
            .Subscribe((value) =>
            {
                _characterUI?.UpdateHealthGUI(value);
            });
    }

    public void SetCharacterUI(CharacterUI characterUI)
    {
        _characterUI = characterUI;
    }

    public bool DeadLife()
    {
        return _characterUI.Lifes.OnDeleteLife();
    }
}