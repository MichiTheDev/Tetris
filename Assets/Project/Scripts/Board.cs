using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Tetris
{
    public sealed class Board
    {
        public event Action<Dictionary<Vector2Int, int>> OnBoardChanged;
        
        public int Width { get; }
        public int Height { get; }

        [NotNull] private readonly int[,] _grid;
        [NotNull] private readonly BlockQueue _blockQueue;
        [NotNull] private Block _currentBlock;

        [NotNull] private Dictionary<Vector2Int, int> changedTiles = new();
        
        public Board(int width, int height)
        {
            Width = width;
            Height = height;

            _blockQueue = new BlockQueue(new Vector2Int(Width / 2, Height));
            _grid = new int[width, height];

            SetNewBlock(_blockQueue.GetNextBlock()!);
        }

        public void Tick()
        {
            MoveBlock(new Vector2Int(0, -1));
        }

        public void MoveBlock(Vector2Int direction)
        {
            ClearTiles(_currentBlock.GetTilePositions());
            _currentBlock.Move(direction);
            Vector2Int[] newTilePositions = _currentBlock.GetTilePositions();
            
            if(!InBounds(newTilePositions) || HasCollision(newTilePositions))
            {
                _currentBlock.Move(-direction);
                newTilePositions = _currentBlock.GetTilePositions();

                // Only place block when hitting the ground
                if(direction.y != 0)
                {
                    PlaceBlock();
                    return;
                }
            }
            
            SetTiles(newTilePositions, _currentBlock.ID);
            BroadcastBoardChange();
        }

        public void RotateBlock(int direction)
        {
            ClearTiles(_currentBlock.GetTilePositions());
            _currentBlock.Rotate(direction);
            Vector2Int[] newTilePositions = _currentBlock.GetTilePositions();

            if(!InBounds(newTilePositions) || HasCollision(newTilePositions))
            {
                _currentBlock.Rotate(-direction);
                return;
            }
            
            SetTiles(_currentBlock.GetTilePositions(), _currentBlock.ID);
            BroadcastBoardChange();
        }

        private void SetNewBlock([NotNull] Block block)
        {
            _currentBlock = block;
            UpdateChangedTiles(_currentBlock.GetTilePositions(), _currentBlock.ID);
            BroadcastBoardChange();
        }
        
        private void ClearTiles([NotNull] Vector2Int[] tilePositions)
        {
            foreach(Vector2Int position in tilePositions)
            {
                _grid[position.x, position.y] = 0;
            }
            UpdateChangedTiles(tilePositions, 0);
        }

        private void SetTiles([NotNull] Vector2Int[] tilePositions, int id)
        {
            foreach(Vector2Int position in tilePositions)
            {
                _grid[position.x, position.y] = id;
            }
            UpdateChangedTiles(tilePositions, id);
        }
        
        private void PlaceBlock()
        {
            Vector2Int[] tilePositions = _currentBlock.GetTilePositions();

            foreach(Vector2Int position in tilePositions)
            {
                _grid[position.x, position.y] = _currentBlock.ID;
            }
            
            UpdateChangedTiles(tilePositions, _currentBlock.ID);
            CheckForFullRows();
            SetNewBlock(_blockQueue.GetNextBlock());
        }

        private bool IsRowEmpty(int row)
        {
            for(int x = 0; x < Width; x++)
            {
                if(_grid[x, row] != 0) return false;
            }
            return true;
        }

        private bool IsRowFull(int row)
        {
            for(int x = 0; x < Width; x++)
            {
                if(_grid[x, row] == 0) return false;
            }
            return true;
        }

        private bool InBounds([NotNull] Vector2Int[] tilePositions)
        {
            foreach(Vector2Int position in tilePositions)
            {
                if(position.x < 0 || position.x >= Width || position.y < 0 || position.y >= Height) return false;
            }
            return true;
        }

        private bool HasCollision([NotNull] Vector2Int[] tilePositions)
        {
            foreach(Vector2Int tilePosition in tilePositions)
            {
                if(_grid[tilePosition.x, tilePosition.y] != 0) return true;
            }
            return false;
        }

        private void MoveRowDown(int row, int steps)
        {
            if(steps < 1) return;
            
            Vector2Int[] movedTilePositions = new Vector2Int[Width];
            
            for(int x = 0; x < Width; x++)
            {
                int newRow = row - steps;
                _grid[x, newRow] = _grid[x, row];
                _grid[x, row] = 0;

                movedTilePositions[x] = new Vector2Int(x, row);
                
                UpdateChangedTiles(new[]{ new Vector2Int(x,newRow) }, _grid[x, newRow]);
            }
            
            UpdateChangedTiles(movedTilePositions, 0);
        }

        private void ClearRow(int row)
        {
            Vector2Int[] tilePositions = new Vector2Int[Width];
            for(int x = 0; x < Width; x++)
            {
                _grid[x, row] = 0;
                tilePositions[x] = new Vector2Int(x, row);
            }
            
            UpdateChangedTiles(tilePositions, 0);
        }
        
        private void CheckForFullRows()
        {
            int cleared = 0;
            
            for(int y = 0; y < Height; y++)
            {
                if(IsRowFull(y))
                {
                    cleared++;
                    ClearRow(y);
                    continue;
                }

                if(IsRowEmpty(y)) return;
                
                MoveRowDown(y, cleared);
            }
        }
        
        private void BroadcastBoardChange()
        {
            OnBoardChanged?.Invoke(changedTiles);
            changedTiles.Clear();
        }
        
        private void UpdateChangedTiles([NotNull] Vector2Int[] tilePositions, int id)
        {
            foreach(Vector2Int position in tilePositions)
            {
                changedTiles[position] = id;
            }
        }
    }
}