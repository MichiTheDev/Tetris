using TwodeUtils;
using UnityEngine;

namespace Tetris
{
    public sealed class LevelManager : Singleton<LevelManager>
    {
        [Header("Board Settings")]
        [SerializeField] private int _boardWidth = 10;
        [SerializeField] private int _boardHeight = 18;

        [Header("Block Data")]
        [SerializeField] private TextAsset _blockDataJson;

        private Board _board;
        private BlockQueue _blockQueue;

        protected override void Awake()
        {
            base.Awake();
            _board = new Board(_boardWidth, _boardHeight);
            _blockQueue = new BlockQueue(_blockDataJson!.text);
        }

        private void Start()
        {
            BoardVisualizer.Instance?.SetBoard(_board);
            _board.SetCurrentBlock(_blockQueue.GetNextBlock());
        }
    }
}