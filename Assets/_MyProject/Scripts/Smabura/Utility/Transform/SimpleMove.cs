using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public Vector3 MoveDirection;

    private void Update()
    {
        transform.Translate(MoveDirection * Time.deltaTime);
    }
}
