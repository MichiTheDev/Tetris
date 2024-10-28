using TwodeUtils;
using UnityEngine;

namespace Tetris
{
    public sealed class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private int _boardWidth = 10;
        [SerializeField] private int _boardHeight = 18;

        private Board _board;

        protected override void Awake()
        {
            base.Awake();
            _board = new Board(_boardWidth, _boardHeight);
        }

        private void Start()
        {
            
            BoardVisualizer.Instance?.SetBoard(_board);
        }
    }
}