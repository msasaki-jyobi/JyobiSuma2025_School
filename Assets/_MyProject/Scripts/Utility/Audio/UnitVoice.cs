using develop_common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UnitVoice : MonoBehaviour
{
    //public float VoiceSpan = 0.2f;
    ////public List<VoiceKind> VoiceClips = new List<VoiceKind>();

    //public event Func<bool> VoicePlayCheckEvent;

    //private float _voiceTimer;

    //private void Update()
    //{
    //    if (_voiceTimer > 0) { _voiceTimer -= Time.deltaTime; }
    //}

    //public void PlayVoice(string keyName, bool ignoreTimer = false, bool checkIgnore = false)
    //{
    //    // 再生できない条件を満たす場合Return
    //    bool check = VoicePlayCheckEvent?.Invoke() ?? false;
    //    if (check && !checkIgnore)
    //        return;

    //    if (!ignoreTimer)
    //        if (_voiceTimer > 0) return;

    //    foreach (var voice in VoiceClips)
    //    {
    //        if (keyName == voice.ClipName)
    //        {
    //            _voiceTimer = VoiceSpan;
    //            AudioManager.Instance.PlayOneShotClipData(voice.ClipData);
    //        }
    //    }
    //}
}