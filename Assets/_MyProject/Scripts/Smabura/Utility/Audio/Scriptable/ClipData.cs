using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAudioType
{
    Se,
    Bgm,
    Voice
}

[CreateAssetMenu(fileName = "ClipData", menuName = "develop_common / ClipData")]
public class ClipData : ScriptableObject
{
    public List<AudioClip> AudioClips;
    public EAudioType AudioType;
}
