using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TwodeUtils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tetris
{
    public sealed class BoardVisualizer : Singleton<BoardVisualizer>
    {
        [Header("Tilemaps")] 
        [SerializeField, NotNull] private Tilemap _backgroundMap = null!;
        [SerializeField, NotNull] private Tilemap _blockMap = null!;
        
        [Header("Tile Assets")]
        [SerializeField] private Tile _borderTile;
        [SerializeField] private Tile _gridTile;
        [SerializeField] private Tile _blockTile;

        [NotNull] private Board _board = null!;
        private int _boardHalfWidth;
        private int _boardHalfHeight;
        
        public void SetBoard(Board board)
        {
            if(board is null) return;
            
            _board = board;
            _boardHalfWidth = _board.Width / 2;
            _boardHalfHeight = _board.Height / 2;
            _board.OnBoardChanged += BoardChanged;
            
            DrawBoard();
        }

        private void BoardChanged([NotNull] Dictionary<Vector2Int, int> changedTiles)
        {
            foreach(KeyValuePair<Vector2Int, int> changedTile in changedTiles)
            {
                Vector2Int changedTilePosition = changedTile.Key;
                Vector3Int tilemapPosition = ConvertToTilemapPosition(changedTilePosition.x, changedTilePosition.y);
                _blockMap.SetTile(tilemapPosition, _blockTile);
                _blockMap.SetColor(tilemapPosition, BlockData.GetColor(changedTile.Value));
            }
        }

        private void DrawBoard()
        {
            _backgroundMap.ClearAllTiles();
            for(int y = -1; y <= _board.Height; y++)
            {
                for(int x = -1; x <= _board!.Width; x++)
                {
                    if(y == -1 || y == _board.Height || x == -1 || x == _board.Width)
                    {
                        _backgroundMap!.SetTile(ConvertToTilemapPosition(x, y), _borderTile);
                        continue;
                    }
                    
                    _backgroundMap!.SetTile(ConvertToTilemapPosition(x, y), _gridTile);
                }
            }
        }

        private Vector3Int ConvertToTilemapPosition(int x, int y)
        {
            return new Vector3Int(x - _boardHalfWidth, y - _boardHalfHeight);
        }
    }
}   