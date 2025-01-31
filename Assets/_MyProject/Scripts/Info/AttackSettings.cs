using System.Collections;
using UnityEngine;

[System.Serializable]
public class AttackSettings : MonoBehaviour
{
    // _activeState:1 地面技　火力、吹き飛び、ヒットエフェクト、生成エフェクト、再生コライダーSlot
    // 
    public int StateNum;
    public HitCollider HitCollider;

    public float DamageAmount = 1;
    public float ForcePower;
    public float ForceUpMultiplier;
    public GameObject EnableEffect;
    public GameObject HitEffect;
    public AudioClip EnableSE;
    public AudioClip HitSE;
}