using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tetris
{
    public sealed class GameBoard : MonoBehaviour
    {
        public const int GRID_WIDTH = 10;
        public const int GRID_HEIGHT = 18;
        
        [SerializeField] private int _tickRate = 4;
        [SerializeField] private TileBase _block;
        
        private Tilemap _map;
        private Block _currentBlock;
        private BlockQueue _blockQueue;
        private float _tickTime;
        private float _tickTimer;
        private bool _isGameOver;

        private void Awake()
        {
            _map = GetComponent<Tilemap>();
            _blockQueue = new BlockQueue();
            
            _tickTime = 1.0f / _tickRate;
            _currentBlock = _blockQueue!.GetBlock();
        }

        private void Update()
        {
            if(_isGameOver) return;
            
            _tickTimer += Time.deltaTime;
            if(_tickTimer >= _tickTime)
            {
                _tickTimer = 0f;
                Tick();
            }
        }

        private void Tick()
        {
            MoveBlock(new Vector2Int(0, -1));
        }
        
        private void MoveBlock(Vector2Int direction)
        {
            Vector2Int[] tilePositions = _currentBlock!.GetTilePositions();
            
            ClearTiles(tilePositions);
            _currentBlock?.Move(direction);
            if(!DoesBlockFit())
            {
                _currentBlock!.Move(-direction);
                PlaceBlock();
                return;
            }
            
            ClearTiles(tilePositions);
            DrawBlock();
        }

        private bool DoesBlockFit()
        {
            if(_currentBlock is null) return false;
            
            Vector2Int[] tilePositions = _currentBlock.GetTilePositions()!;
            if(!InBounds(tilePositions)) return false;
            
            foreach(Vector2Int position in tilePositions)
            {
                if(_map!.GetTile((Vector3Int) position) is not null)
                {
                    return false;
                }
            }
            return true;
        }

        private void DrawBlock()
        {
            foreach(Vector2Int position in _currentBlock!.GetTilePositions()!)
            {
                _map!.SetTile((Vector3Int) position, _block);
                _map.SetColor((Vector3Int) position, _currentBlock.BlockData!.Color);
            }
        }

        private void PlaceBlock()
        {
            DrawBlock();
            _currentBlock = _blockQueue!.GetBlock()!;
            // Update and check full rows
        }
        
        private void ClearTiles(Vector2Int[] positions)
        {
            if(positions is null) return;
            
            foreach(Vector2Int position in positions)
            {
                _map!.SetTile((Vector3Int) position, null);
            }
        }

        private bool InBounds(Vector2Int[] positions)
        {
            if(positions is null) return false;

            foreach(Vector2Int position in positions)
            {
                if(position is not {x: >= 0 and < GRID_WIDTH, y: >= 0 and < GRID_HEIGHT})
                {
                    return false;
                }
            }
            return true;
        }
    }
}