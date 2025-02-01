using UnityEngine;
using UnityEngine.UI;

public class Lifes : MonoBehaviour
{
    public Transform LifesContent;
    public Image BackgroundImage;
    public Color PinchColor;
    public GameObject LifeImagePrefab;

    public void OnCreateLife(int value, Sprite lifeSprite)
    {
        for (int i = 0; i < value; i++)
        {
            var image = Instantiate(LifeImagePrefab, LifesContent);
            if(image.TryGetComponent(out Image lifeImage))
                lifeImage.sprite = lifeSprite;
        }    
    }

    public bool OnDeleteLife()
    {
        if(LifesContent.childCount == 0)
        {
            return true; // 死亡
        }
        else
        {
            Destroy(LifesContent.GetChild(0).gameObject); // 残機を減らす
            BackgroundImage.color = PinchColor;
        }
        return false;
    }
}
