using System.Collections;
using UnityEngine;

public class TestForce : MonoBehaviour
{
    [SerializeField] private Gravity _gravity;
    [SerializeField] private Vector3 _forcePower;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            _gravity.AddExternalForce(_forcePower);
    }
}
