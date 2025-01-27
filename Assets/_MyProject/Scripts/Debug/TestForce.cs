using System.Collections;
using UnityEngine;

public class TestForce : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    [SerializeField] private Vector3 _forcePower;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            _movement.AddExternalForce(_forcePower);
    }
}
