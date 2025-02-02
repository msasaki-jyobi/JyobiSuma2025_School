using UnityEngine;

public class DeadCollider : MonoBehaviour
{
    public AudioClip HitSE;
    public GameObject HitEffect;

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
        if(hit.gameObject.TryGetComponent(out Health health))
        {
            AudioManager.Instance.PlayOneShot(HitSE, EAudioType.Se);
            UtilityFunction.PlayEffect(hit, HitEffect);

            health.SmashDead();
        }
    }
}
