using UnityEngine;

public class EnableCursolChanger : MonoBehaviour
{
    public bool EnableVisible = true;

    private void OnEnable()
    {
        Cursor.visible = EnableVisible;
        Cursor.lockState = EnableVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.visible = !EnableVisible;
        Cursor.lockState = !EnableVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
