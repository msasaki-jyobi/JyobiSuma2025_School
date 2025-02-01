using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVoiceKind
{
    None,
    Attack_A,
    Attack_B,
    Damage_A,
    Damage_B,
    Dead_A,
    Dead_B,
}

public class UnitVoice : MonoBehaviour
{
    public float VoiceSpan = 0.2f;
    public List<VoiceInfo> VoiceInfos = new List<VoiceInfo>();

    private float _voiceTimer;

    private void Update()
    {
        if (_voiceTimer > 0) { _voiceTimer -= Time.deltaTime; }
    }

    public void PlayVoice(EVoiceKind voiceKind, bool ignoreTimer = false)
    {
        if (!ignoreTimer)
            if (_voiceTimer > 0) return;

        foreach (var voice in VoiceInfos)
        {
            if (voiceKind == voice.VoiceKind)
            {
                _voiceTimer = VoiceSpan;
                int ran = UnityEngine.Random.Range(0, voice.Voices.Count);
                AudioManager.Instance.PlayOneShot(voice.Voices[ran], EAudioType.Voice);
            }
        }
    }
}

