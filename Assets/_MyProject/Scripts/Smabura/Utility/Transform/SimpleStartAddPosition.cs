using UnityEngine;

public class SimpleStartAddPosition : MonoBehaviour
{
    public Vector3 WarpPos;

    private void Start()
    {
        transform.position += transform.right * WarpPos.x + transform.up * WarpPos.y + transform.forward * WarpPos.z;
    }
}
