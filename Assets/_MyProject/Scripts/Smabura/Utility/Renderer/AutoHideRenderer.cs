using UnityEngine;

public class AutoHideRenderer : MonoBehaviour
{
    private void Start()
    {
        if(TryGetComponent(out Renderer renderer))
            renderer.enabled = false;
    }
}
