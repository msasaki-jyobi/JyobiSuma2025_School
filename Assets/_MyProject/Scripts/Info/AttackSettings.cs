using System.Collections;
using UnityEngine;

[System.Serializable]
public class AttackSettings : MonoBehaviour
{
    // _activeState:1 地面技　火力、吹き飛び、ヒットエフェクト、生成エフェクト、再生コライダーSlot
    // 
    [Header("Animator State Num")]
    public int StateNum;
    [Header("ターゲットとなるコライダー")]
    public HitCollider HitCollider;

    [Header("ダメージ"), Range(0, 100)]
    public float DamageAmount = 1;
    [Header("吹き飛び値(X)"), Range(-50, 50)]
    public float ForcePower;
    [Header("吹き飛び値(Y)"), Range(-50, 50)]
    public float ForceUpMultiplier;
    [Header("発動時：エフェクト")]
    public GameObject PlayEffect;
    [Header("コライダー発生時：エフェクト")]
    public GameObject EnableEffect;
    [Header("技ヒット時：エフェクト")]
    public GameObject HitEffect;
    [Header("発動時時：効果音")]
    public AudioClip PlaySE;
    [Header("コライダー発生時：効果音")]
    public AudioClip EnableSE;
    [Header("技ヒット時：効果音")]
    public AudioClip HitSE;

    [Header("発動ボイス")]
    public EVoiceKind PlayVoice;

    [Header("生成魔法")]
    public bool UnCollider;
    public GameObject EnableMagicPrefab;
    public float LifeTime = 5f;
}