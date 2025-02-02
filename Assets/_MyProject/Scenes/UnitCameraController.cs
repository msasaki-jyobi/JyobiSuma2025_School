//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using Unity.VisualScripting;
//using UnityEngine.Rendering.PostProcessing;
//using System;
//using TMPro;
//using UnityEngine.UI;
//using Unity.Cinemachine;

//namespace Common.Unit
//{
//    /// <summary>
//    /// ユニットのカメラ制御を管理するクラス
//    /// </summary>
//    [AddComponentMenu("UnitCameraController：カメラ制御")]
//    public class UnitCameraController : MonoBehaviour
//    {
//        [SerializeField] private InputReader _inputReader;
//        [Header("キャラクターに追跡するカメラを制御・生成するクラス")]
//        [Tooltip("カメラタイプ")]
//        [SerializeField] private Image _reticleImage;
//        private Camera _camera;
//        private CinemachineBrain _cinemachineBrain;

//        [Header("===必須設定===")]
//        [SerializeField] private float _defaultBlendTime = 0.1f;
//        public InputActionReference AxisReference;
//        public CinemachineFreeLook DeadCamera;


//        [Tooltip("カメラの制御")]
//        // AIMカメラ用AxisState
//        private AxisState _axisHorizontal;
//        private AxisState _axisVertical;


//        public CinemachineVirtualCamera InputVcam;
//        public CinemachinePOV VcamPOV;
//        public CinemachineVirtualCamera AimVcam;
//        public CinemachinePOV AimPOV;
//        //private CinemachineVirtualCamera _aimCamera;
//        private Transform _headEye;
//        private CinemachineInputProvider _provider;

//        private bool _isFreeCamera;
//        private CinemachineFramingTransposer _framingTransposer;

//        public void Init()
//        {
//            _camera = GetComponent<UnitComponents>().Camera;
//            _cinemachineBrain = GetComponent<UnitComponents>().CinemachineBrain;

//            // カメラを生成
//            if (InputVcam == null)
//                CreateCamera();
//            // 切り替え速度を初期化
//            _cinemachineBrain.m_DefaultBlend.m_Time = _defaultBlendTime;
//            //SetActivePlayer(_inputController.DefaultActivePlayer);
//            // FreeCamera処理登録

//            if (_inputReader == null) return;
//            _inputReader.StartedFreeCameraEvent += OnChangeCameraHandle;
//            InputVcam.m_Priority = 15;
//        }



//        private void Start()
//        {
//            // カメラ感度変更：Gamepad丁度良い値（マウスは速い）
//            // ChangeDelta(1.5f);
//            if (_inputReader == null) return;

//            _inputReader.OnChangeDevice += OnChangeDevice;
//            OnChangeDevice("Keyboard");

//            _inputReader.StartedSubFireEvent += OnStartedSubFireHandle;
//            _inputReader.PrimarySubFireEvent += OnPrimarySubFireHandle;

//            VcamPOV = InputVcam.GetCinemachineComponent<CinemachinePOV>();
//            AimPOV = AimVcam.GetCinemachineComponent<CinemachinePOV>();
//        }

//        private void Update()
//        {

//        }

//        private void OnDestroy()
//        {
//            if(_inputReader != null)
//            {
//                _inputReader.StartedSubFireEvent -= OnStartedSubFireHandle;
//                _inputReader.PrimarySubFireEvent -= OnPrimarySubFireHandle;
//            }
//        }

//        private void OnStartedSubFireHandle()
//        {


//            AimPOV.m_HorizontalAxis.Value = VcamPOV.m_HorizontalAxis.Value;
//            AimPOV.m_VerticalAxis.Value = VcamPOV.m_VerticalAxis.Value;
//            // カメラの同期を強制的に更新するために以下の行を追加
//            AimPOV.m_HorizontalAxis.Update(Time.deltaTime);
//            AimPOV.m_VerticalAxis.Update(Time.deltaTime);

//            // レーティクルON
//            _reticleImage.enabled = true;

//        }
//        private void OnPrimarySubFireHandle(bool flg)
//        {
//            //_framingTransposer.m_CameraDistance = !flg ? _defaultDistance : 2;
//            InputVcam.m_Priority = flg ? 0 : 10;
//            AimVcam.m_Priority = flg ? 10 : 0;

//            if (!flg) // 離された時
//            {

//                VcamPOV.m_HorizontalAxis.Value = AimPOV.m_HorizontalAxis.Value;
//                VcamPOV.m_VerticalAxis.Value = AimPOV.m_VerticalAxis.Value;

//                // カメラの同期を強制的に更新するために以下の行を追加
//                VcamPOV.m_HorizontalAxis.Update(Time.deltaTime);
//                VcamPOV.m_VerticalAxis.Update(Time.deltaTime);

//                // レーティクルOFF
//                _reticleImage.enabled = false;
//            }
//        }




//        /// <summary>
//        /// AxisStateを更新
//        /// </summary>
//        public void UpdateAxis()
//        {
//            // axisStateを更新（AIMカメラに利用するaxis用）
//            _axisVertical.Update(Time.deltaTime);
//            _axisHorizontal.Update(Time.deltaTime);
//            _axisVertical.Value = VcamPOV.m_HorizontalAxis.Value;
//            _axisHorizontal.Value = VcamPOV.m_VerticalAxis.Value;
//        }


//        /// <summary>
//        /// Axisの値を返す(AIM時にカメラの向きにプレイヤーを向かせる時に利用）
//        /// </summary>
//        /// <param name="parallel">水平・垂直</param>
//        /// <returns></returns>
//        public float GetAxisValue(EDirection parallel)
//        {
//            switch (parallel)
//            {
//                case EDirection.Horizontal:
//                    return _axisHorizontal.Value;

//                case EDirection.Vertical:
//                    return _axisVertical.Value;
//            }
//            return 0;
//        }

//        /// <summary>
//        /// カメラに向いた方向を反映させる(AIMモード時、TPSカメラも更新する際に利用）
//        /// </summary>
//        public void SetCameraAxis()
//        {
//            VcamPOV.m_HorizontalAxis.Value = _axisVertical.Value;
//            VcamPOV.m_VerticalAxis.Value = _axisHorizontal.Value;
//            //headEye.localRotation = horizontalAimRotation;
//        }

//        /// <summary>
//        /// カメラの優先度を切り替える
//        /// </summary>
//        public void SetPriority()
//        {
//            // カメラをエイムカメラに切り替え
//            InputVcam.m_Priority = 10;
//            //freeCamera.m_Priority = 10;
//            //aimCamera.m_Priority = 50;
//        }

//        /// <summary>
//        /// カメラタイプに応じてカメラを生成する
//        /// </summary>
//        protected void CreateCamera()
//        {
//            // Typeに応じて値を読み込み
//            CameraValue cameraValue = new CameraValue();
//            switch (_cameraType)
//            {
//                case ECameraType.Tps:
//                    cameraValue = TpsCameraValue;
//                    break;
//                case ECameraType.Side:
//                    cameraValue = SideCameraValue;
//                    break;
//                case ECameraType.Top:
//                    cameraValue = TopCameraValue;
//                    break;
//                case ECameraType.Fps:
//                    cameraValue = FpsCameraValue;
//                    break;
//            }
//            // HeadEye生成
//            GameObject eye = new GameObject();
//            eye.name = "HeadEye";
//            eye.transform.parent = transform;
//            eye.transform.localPosition = Vector3.zero;
//            eye.transform.localRotation = Quaternion.Euler(Vector3.zero);
//            _headEye = eye.transform;

//            // カメラを生成
//            GameObject vcmObject = new GameObject();
//            CinemachineVirtualCamera vcam = vcmObject.AddComponent<CinemachineVirtualCamera>();
//            _framingTransposer = vcam.AddCinemachineComponent<CinemachineFramingTransposer>();

//            vcmObject.name = "InputCamera";
//            vcam.m_Priority = 10;
//            vcam.m_Follow = _headEye;
//            vcam.m_Lens.FieldOfView = 40;
//            vcam.m_Lens.NearClipPlane = 0.1f;
//            vcam.m_Lens.FarClipPlane = 200f;

//            _framingTransposer.m_TrackedObjectOffset = cameraValue.trackedOffset;
//            _framingTransposer.m_XDamping = 0;
//            _framingTransposer.m_YDamping = 0;
//            _framingTransposer.m_ZDamping = 0;
//            _framingTransposer.m_ScreenX = cameraValue.screenX;
//            _framingTransposer.m_ScreenY = cameraValue.screenY;
//            _framingTransposer.m_TargetMovementOnly = true;
//            _framingTransposer.m_CameraDistance = cameraValue.distance;
//            _framingTransposer.m_DeadZoneWidth = cameraValue.m_DeadZoneWidth;
//            _framingTransposer.m_DeadZoneHeight = cameraValue.m_DeadZoneHeight;
//            _framingTransposer.m_SoftZoneWidth = cameraValue.m_SoftZoneWidth;
//            _framingTransposer.m_SoftZoneHeight = cameraValue.m_SoftZoneHeight;

//            if (_cameraType == ECameraType.Tps || _cameraType == ECameraType.Fps) // TPS用のカメラオプション追加
//            {
//                VcamPOV = vcam.AddCinemachineComponent<CinemachinePOV>();
//                VcamPOV.m_VerticalAxis.Value = 0;
//                VcamPOV.m_VerticalAxis.m_MinValue = -cameraValue.limitX;
//                VcamPOV.m_VerticalAxis.m_MaxValue = cameraValue.limitX;
//                VcamPOV.m_VerticalAxis.m_MaxSpeed = 1;
//                VcamPOV.m_VerticalAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;
//                VcamPOV.m_VerticalAxis.m_AccelTime = 0.1f;
//                VcamPOV.m_VerticalAxis.m_InvertInput = true;

//                VcamPOV.m_HorizontalAxis.Value = 0;
//                VcamPOV.m_HorizontalAxis.m_MinValue = -cameraValue.limitY;
//                VcamPOV.m_HorizontalAxis.m_MaxValue = cameraValue.limitY;
//                VcamPOV.m_HorizontalAxis.m_MaxSpeed = 2;
//                VcamPOV.m_HorizontalAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;
//                VcamPOV.m_HorizontalAxis.m_AccelTime = 0.1f;
//                VcamPOV.m_HorizontalAxis.m_InvertInput = false;
//            }

//            if (_cameraType == ECameraType.Fps)
//            {
//                VcamPOV.m_VerticalAxis.m_MinValue = -90;
//                VcamPOV.m_VerticalAxis.m_MaxValue = 90;
//                VcamPOV.m_VerticalAxis.m_AccelTime = 0.03f;
//                _framingTransposer.m_XDamping = 0.1f;
//                _framingTransposer.m_YDamping = 0.1f;
//                _framingTransposer.m_ZDamping = 0.1f;
//                _framingTransposer.m_UnlimitedSoftZone = true;
//            }

//            if (_cameraType != ECameraType.Fps)
//            {
//                // カメラの当たり判定
//                CinemachineCollider cameraCollider = vcam.gameObject.AddComponent<CinemachineCollider>();
//                cameraCollider.m_CollideAgainst = LayerMask.GetMask(cameraValue.ColliderName);
//                cameraCollider.m_Strategy = CinemachineCollider.ResolutionStrategy.PullCameraForward;
//                cameraCollider.m_SmoothingTime = 0.1f;
//                cameraCollider.m_Damping = 0.1f;
//                cameraCollider.m_DampingWhenOccluded = 0.1f;
//                vcam.AddExtension(cameraCollider);
//                vcam.transform.rotation = Quaternion.Euler(cameraValue.cameraRotation);
//            }
//            InputVcam = vcam;

//            if (GetComponent<UnitCondition>().UnitType == EUnitType.Player)
//                vcmObject.name = "InputCameraPlayer";
//        }

//        public void SetActivePlayer(bool flg)
//        {
//            InputVcam.m_Priority = flg ? 15 : 0;
//        }

//        public void SetInputProvider()
//        {
//            if (_provider == null)
//                if(InputVcam != null)
//                    _provider = InputVcam.AddComponent<CinemachineInputProvider>();

//            if(_provider != null)
//            {
//                _provider.XYAxis = AxisReference;
//                _provider.ZAxis = AxisReference;
//                _provider.gameObject.SetActive(false);
//                _provider.gameObject.SetActive(true);
//            }
//        }
//        public void ChangeDelta(float value)
//        {
//            if (VcamPOV == null) return;
//            VcamPOV.m_HorizontalAxis.m_MaxSpeed = value;
//            VcamPOV.m_VerticalAxis.m_MaxSpeed = value * 0.5f;
//        }

//        public void ChangeDeadCamera(bool free = false)
//        {
//            if (DeadCamera == null) return;
//            if (free)
//                DeadCamera.m_Priority = 200;
//            else
//                InputVcam.m_Lens.FieldOfView /= 1.3f;
//        }

//        private void OnChangeCameraHandle()
//        {
//            _isFreeCamera = !_isFreeCamera;

//            DeadCamera.m_Priority = _isFreeCamera ? 200 : 0;
//            string value = _isFreeCamera ? "FreeCamera:ON" : "NormalCamera:ON";
//            UIManager.Instance.OnSetSubMessage(value);

//        }

//        private void OnChangeDevice(string device)
//        {
//            return;
//            switch (device)
//            {
//                case "Gamepad": // 3 2
//                    VcamPOV.m_VerticalAxis.m_MaxSpeed = 3;
//                    VcamPOV.m_HorizontalAxis.m_MaxSpeed = 2;
//                    break;
//                case "Keyboard":// 0.1 0.1
//                    VcamPOV.m_VerticalAxis.m_MaxSpeed = 0.4f;
//                    VcamPOV.m_HorizontalAxis.m_MaxSpeed = 0.4f;
//                    break;
//            }
//        }

//        public void OnSetUpdateY(float rotY)
//        {
//            Debug.Log(rotY);
//            //// 角度を正規化（0〜360度の範囲に収める）
//            //rotY = rotY % 360;
//            //if (rotY < 0) rotY += 360;
//            // 回転を設定
//            VcamPOV.m_HorizontalAxis.Value = rotY;

//        }
//    }
//}
