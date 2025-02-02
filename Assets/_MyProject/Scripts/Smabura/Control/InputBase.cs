using System.Collections;
using UnityEngine;

public class InputBase : MonoBehaviour
{
    [SerializeField] protected InputReaderInitializer _inputReaderInitializer;
    protected InputReader _inputReader;

    protected virtual void Start()
    {
        _inputReader = _inputReaderInitializer.InputReader;
    }
}