using UnityEngine;
using UnityEngine.Events;

public class EnterAction : MonoBehaviour
{
    [SerializeField] private string _targetTag = "Player";
    [SerializeField] private UnityEvent _hitEvent;

    private void OnCollisionEnter(Collision collision)
    {
        OnHit(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHit(other.gameObject);
    }

    private void OnHit(GameObject hit)
    {
        if(hit.CompareTag(_targetTag))
        {
            _hitEvent?.Invoke();
        }
    }
}
