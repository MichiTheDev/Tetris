using System;
using UnityEngine;

namespace Tetris
{
    public sealed class Board
    {
        public event Action OnBoardChanged;
        
        public int Width { get; }
        public int Height { get; }
        public readonly int[,] Grid;

        private Block _currentBlock;
        
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            Grid = new int[width, height];
        }

        public void SetCurrentBlock(Block block)
        {
            _currentBlock = block;
            PlaceBlock();
        }

        private void PlaceBlock()
        {
            if(Grid is null) return;
            
            Vector2Int[] tilePositions = _currentBlock?.GetTilePositions();
            if(tilePositions is null) return;

            foreach(Vector2Int position in tilePositions)
            {
                Grid[position.x, position.y] = _currentBlock.BlockData.ID;
            }
            
            OnBoardChanged?.Invoke();
        }
    }
}