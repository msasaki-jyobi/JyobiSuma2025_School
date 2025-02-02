using System.Collections;
using UnityEngine;

[System.Serializable]
public class AttackSettings : MonoBehaviour
{
    // _activeState:1 地面技　火力、吹き飛び、ヒットエフェクト、生成エフェクト、再生コライダーSlot
    // 
    [Header("Animator State Num（変更不要）")]
    public int StateNum;

    [Header("ターゲットコライダー")]
    public HitCollider HitCollider;
    [Header("ターゲットコライダーの判定をOFFにする")]
    public bool UnCollider;
    [Header("ヒットダメージ"), Range(0, 100)]
    public float DamageAmount = 1;
    [Header("吹き飛び値(X) / 吹き飛び値(Y)"), Range(-50, 50)]
    public float ForcePower;
    public float ForceUpMultiplier;
    [Header("エフェクト： 発動時 / コライダー発生時 / ヒット時")]
    public GameObject PlayEffect;
    public GameObject EnableEffect;
    public GameObject HitEffect;
    [Header("効果音： 発動時 / コライダー発生時 / ヒット時")]
    public AudioClip PlaySE;
    public AudioClip EnableSE;
    public AudioClip HitSE;
    [Header("発動ボイス")]
    public EVoiceKind PlayVoice;
    [Header("発生時：生成魔法 / 破棄までの時間")]
    public GameObject EnableMagicPrefab;
    public float LifeTime = 5f;
}