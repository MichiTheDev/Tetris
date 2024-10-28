using System;
using JetBrains.Annotations;
using TwodeUtils;
using UnityEngine;

namespace Tetris
{
    public sealed class LevelManager : Singleton<LevelManager>
    {
        [Header("Board Settings")]
        [SerializeField] private int _boardWidth = 10;
        [SerializeField] private int _boardHeight = 18;

        [Header("Game Settings")]
        [SerializeField] private int _tickRate = 4;
        
        [NotNull] private Board _board = null!;
        private float _tickRateTimer;
        private float _tickRateTime;

        protected override void Awake()
        {
            base.Awake();
            _board = new Board(_boardWidth, _boardHeight);
            _tickRateTime = 1f / _tickRate;
        }

        private void Start()
        {
            BoardVisualizer.Instance?.SetBoard(_board);
        }

        private void Update()
        {
            _tickRateTimer += Time.deltaTime;
            if(_tickRateTimer >= _tickRateTime)
            {
                _tickRateTimer = 0f;
                _board.Tick();
            }
        }
    }
}