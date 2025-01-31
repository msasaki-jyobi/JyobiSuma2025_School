using System.Collections;
using UnityEngine;
public class InputReaderInitializer : MonoBehaviour
{
    [field:SerializeField] public InputReader InputReader { get; private set; }
    [SerializeField] private bool _useKeyboardOnly;

    private void Awake()
    {
        if (InputReader != null)
            InputReader.KeyboardOnly = _useKeyboardOnly;
    }
}