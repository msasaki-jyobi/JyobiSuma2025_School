using UnityEngine;

public class LoopRotate : MonoBehaviour
{
    [SerializeField] private Vector3 _lotatePower = new Vector3(0,100,0);
    [SerializeField] private bool _isRotate = true;


    private void Update()
    {
        transform.Rotate(_lotatePower * Time.deltaTime);
    }

    public void OnChangeRotate(bool isRotate)
    {
        _isRotate = isRotate;
    }
}
