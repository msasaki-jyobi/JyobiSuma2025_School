using System;
using UniRx;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Aim : InputBase
{
    [SerializeField] private StateManager _state;
    [SerializeField] private Animator _animator;
    [SerializeField] private CinemachineVirtualCamera _inputVcam;
    [SerializeField] private CinemachineVirtualCamera _aimVcam;

    private CinemachinePOV _vcamPOV;
    private CinemachinePOV _aimPOV;
    private InputAction _aimAction;

    public ReactiveProperty<bool> IsAiming = new ReactiveProperty<bool>();

    protected override void Start()
    {
        base.Start();

        _aimAction = _inputReader.Control.Player.Aim;

        _vcamPOV = _inputVcam.GetCinemachineComponent<CinemachinePOV>();
        _aimPOV = _aimVcam.GetCinemachineComponent<CinemachinePOV>();

        _state.InputState
            .Subscribe((state) => 
            {
                if (state == EInputState.UnControl)
                    IsAiming.Value = false;
            });
        IsAiming
            .Subscribe((isAiming) =>
            {
                // Mask Layer
                _animator.SetLayerWeight(_animator.GetLayerIndex("Mask Layer"), isAiming ? 1 : 0);

                /// カメラに向いた方向を反映させる(AIMモード時、TPSカメラも更新する際に利用）
                _aimPOV.m_HorizontalAxis.Value = isAiming ? _vcamPOV.m_HorizontalAxis.Value : _aimPOV.m_HorizontalAxis.Value;
                _aimPOV.m_VerticalAxis.Value = isAiming ? _vcamPOV.m_VerticalAxis.Value : _aimPOV.m_VerticalAxis.Value;
                _vcamPOV.m_HorizontalAxis.Value = !isAiming ? _aimPOV.m_HorizontalAxis.Value : _vcamPOV.m_HorizontalAxis.Value;
                _vcamPOV.m_VerticalAxis.Value = !isAiming ? _aimPOV.m_VerticalAxis.Value : _vcamPOV.m_VerticalAxis.Value;
                _inputVcam.Priority = isAiming ? 0 : 100;
                _aimVcam.Priority = isAiming ? 100 : 0;
            });
    }

    private void Update()
    {
        if (_state.InputState.Value != EInputState.Control) return;
        IsAiming.Value = _aimAction.IsPressed();
    }

    //private void Animation()
    //{
    //    //オブジェクトにアタッチされているAnimatorを変数へ代入
    //    animator = GetComponent<Animator>();

    //    //"StateA"が再生中か、再生中ならtrue 違うならfalseの値
    //    if (animator.GetCurrentAnimatorStateInfo(0).IsName("StateA"))
    //    {
    //        //処理 条件："StateA"が再生中
    //    }
    //    else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("StateA"))
    //    {
    //        //処理 条件："StateA"が再生中ではない
    //    }
    //    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("StateA") == false)
    //    {
    //        //この記述でも上記と同じ意味 (条件："StateA"が再生中ではない)
    //    }
    //    //Stateが遷移中かどうか、遷移中ならtrue 違うならfalseの値
    //    if (animator.IsInTransition(0))
    //    {
    //        //処理 条件：遷移中
    //    }
    //    else if (!animator.IsInTransition(0))
    //    {
    //        //処理 条件：遷移中ではない
    //    }
    //    else if (animator.IsInTransition(0) == false)
    //    {
    //        //この記述でも上記と同じ意味 (条件：遷移中ではない)
    //    }

    //    //"Second Layer"はウェイトを変えたいレイヤー名、数字でウェイトを指定0~1
    //    animator.SetLayerWeight(animator.GetLayerIndex("Second Layer"), 1);
    //    //SetをGetに書き換えればウェイト値の取得ができる
    //    Debug.Log(animator.GetLayerWeight (animator.GetLayerIndex("Second Layer")));

    //    //Resourcesフォルダの"Animator/AnimatorA"のAnimatorControllerをアタッチする
    //    animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animator/AnimatorA");

    //    //"AnimationA"を再生
    //    animator.Play("AnimationA");
    //    //現在のアニメーションから"AnimationB"へスムーズに移行
    //    animator.CrossFade("AnimationB",0.0f,0,0.0f);
    //}
}
