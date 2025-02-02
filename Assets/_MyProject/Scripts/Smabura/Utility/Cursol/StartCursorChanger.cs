using UnityEngine;

public class StartCursorChanger : MonoBehaviour
{
    public bool IsVisible;

    private void Start()
    {
        Cursor.visible = IsVisible;
        Cursor.lockState = IsVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
