using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] private Image _characterFaceImage;
    [SerializeField] private Image _characterIconImage;
    [SerializeField] private Image _backImageA;
    [SerializeField] private Image _backImageB;
    [SerializeField] private TextMeshProUGUI _healthTextUGUI;
    [SerializeField] private TextMeshProUGUI _nameTextGUI;

    public void SetUnitInfo(UnitInfo unitInfo)
    {
        _characterFaceImage.sprite = unitInfo.FaceSprite;
        _characterIconImage.sprite = unitInfo.IconSprite;
        _backImageA.color = unitInfo.UnitColor;
        _backImageB.color = unitInfo.UnitColor;
        _nameTextGUI.text = unitInfo.UnitName;
    }

    public void UpdateHealthGUI(float value)
    {
        // 整数部分と小数第一位を抽出
        int integerPart = (int)value;
        int decimalPart = (int)((value - integerPart) * 10);  // 小数第一位 （整数で切り捨て）

        // リッチテキストでサイズを分けて表示
        _healthTextUGUI.text = $"<size=50>{integerPart}</size>.{decimalPart}%";

        if(value >= 80)
            _healthTextUGUI.color = Color.red;
        else if(value >= 50)
            _healthTextUGUI.color = Color.cyan;
        else if(value >= 30)
            _healthTextUGUI.color = Color.yellow;
        else
            _healthTextUGUI.color = Color.white;

    }
}
