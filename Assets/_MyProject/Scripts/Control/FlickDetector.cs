using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

/// <summary>
/// はじき入力を検知するクラス
/// </summary>
public class FlickDetector : InputBase
{
    [SerializeField] private bool _isLog;

    private float _flickThreshold = 0.8f; // スティックのはじき判定しきい値
    private float _flickTimeLimit = 0.1f; // はじき受付時間（共通）

    private Dictionary<Key, float> _keyPressTimes = new Dictionary<Key, float>(); // キーの押下時間管理
    private Vector2 _previousInput = Vector2.zero;
    private float _flickStartTime = -1f; // はじき受付開始時間
    public bool IsFlicking { get; private set; } = false; // スマッシュ技受付

    public event Action<Vector2> OnFlickDetected; // はじきが発生した時のイベント

    protected override void Start()
    {
        base.Start();
        // InputReader の MoveEvent を購読
        _inputReader.MoveEvent += DetectFlick;
    }

    private void Update()
    {
        if(_isLog)
        {
            Debug.Log($"Name:{gameObject.name}, Gamepad.current: {(Gamepad.current != null ? "接続中" : "未接続")}");
            Debug.Log($"Name:{gameObject.name}, Keyboard.current: {(Keyboard.current != null ? "接続中" : "未接続")}");
        }

        if (IsFlicking && Time.time - _flickStartTime > _flickTimeLimit)
        {
            IsFlicking = false;
            Debug.Log("はじき受付終了");
        }
    }

    private void OnDestroy()
    {
        if (_inputReader != null)
        {
            _inputReader.MoveEvent -= DetectFlick;
        }
    }

    /// <summary>
    /// はじき入力を検知する
    /// </summary>
    private void DetectFlick(Vector2 input)
    {
        // スティックのはじき判定
        if (Gamepad.current != null) // ゲームパッドが接続されている場合
        {
            Vector2 delta = input - _previousInput;
            if (delta.magnitude / Time.deltaTime > _flickThreshold)
            {
                StartFlick(delta);
            }
        }

        // キーボードのはじき判定
        if (Keyboard.current != null)
        {
            foreach (Key key in new[] { Key.W, Key.A, Key.S, Key.D, Key.UpArrow, Key.DownArrow, Key.LeftArrow, Key.RightArrow })
            {
                if (Keyboard.current[key].wasPressedThisFrame)
                {
                    _keyPressTimes[key] = Time.time;
                }
                if (Keyboard.current[key].wasReleasedThisFrame)
                {
                    if (_keyPressTimes.TryGetValue(key, out float pressTime))
                    {
                        if (Time.time - pressTime <= _flickTimeLimit)
                        {
                            Vector2 flickDirection = GetFlickDirection(key);
                            StartFlick(flickDirection);
                        }
                        _keyPressTimes.Remove(key);
                    }
                }
            }
        }

        _previousInput = input;
    }

    /// <summary>
    /// はじき入力を開始する
    /// </summary>
    private void StartFlick(Vector2 direction)
    {
        _flickStartTime = Time.time;
        IsFlicking = true;
        OnFlickDetected?.Invoke(direction);
        Debug.Log($"はじき検知！方向: {direction}");
    }

    /// <summary>
    /// キーボードのキーに対応するはじき方向を取得
    /// </summary>
    private Vector2 GetFlickDirection(Key key)
    {
        return key switch
        {
            Key.W or Key.UpArrow => Vector2.up,
            Key.A or Key.LeftArrow => Vector2.left,
            Key.S or Key.DownArrow => Vector2.down,
            Key.D or Key.RightArrow => Vector2.right,
            _ => Vector2.zero
        };
    }
}
