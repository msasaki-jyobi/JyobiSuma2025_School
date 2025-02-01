using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject CharacterUIPrefab;
    public Transform CharacterContent;
    public List<UnitInfo> Players = new List<UnitInfo>();

    private void Start()
    {
        //foreach (UnitInfo info in Players)
        //{
        //    var characterUIObject = Instantiate(CharacterUIPrefab, CharacterContent);
        //    if (characterUIObject.gameObject.TryGetComponent(out CharacterUI characterUI))
        //    {
        //        //info.SetCharacterUI(characterUI);
        //        //characterUI.SetUnitInfo(info);
        //    }
        //}
    }
}
