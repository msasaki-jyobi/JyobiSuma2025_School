//using Cysharp.Threading.Tasks;
//using System;
//using UniRx;
//using UnityEngine;

//public class Smash : InputBase
//{
//    [SerializeField] private StateManager _state;
//    [SerializeField] private FlickDetector _flickDetector;
//    [SerializeField] private Motion _motion;


//    private ReactiveProperty<bool> _isSmash = new ReactiveProperty<bool>();
//    private Vector2 _direction;


//    protected override void Start()
//    {
//        base.Start();
//        _inputReader.PrimaryAttackEvent += OnPrimaryAttackEvent;
//        _flickDetector.OnFlickDetected += OnFlickHandle;

//        _isSmash
//            .Subscribe((x) => 
//            {
//                if(x)
//                    _state.InputState.Value = EInputState.UnControl;
                
//            });
//    }



//    private async void OnPrimaryAttackEvent(bool input)
//    {
      

//        if (input)
//        {
//            if (_state.InputState.Value == EInputState.UnControl) return;

//            if (!_isSmash.Value)
//            {
//                if (_flickDetector.IsFlicking)
//                {
//                    _isSmash.Value = true;
//                    _motion.SetState(500);
//                    // ライト点滅

//                    //await UniTask.Delay(3000);
//                    //_isSmash.Value = false;
//                }
//            }
//        }
//        else
//        {
//            if (_isSmash.Value)
//            {
//                _motion.SetState(1);
//                _isSmash.Value = false;
//            }
//        }
//    }
//    private void OnFlickHandle(Vector2 vector)
//    {
//        _direction = vector;
//    }
//}
