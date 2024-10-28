using TwodeUtils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tetris
{
    public sealed class BoardVisualizer : Singleton<BoardVisualizer>
    {
        [Header("Tilemaps")] 
        [SerializeField] private Tilemap _backgroundMap;
        [SerializeField] private Tilemap _blockMap;
        
        [Header("Tile Assets")]
        [SerializeField] private Tile _borderTile;
        [SerializeField] private Tile _gridTile;
        [SerializeField] private Tile _blockTile;

        private Board _board;
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

        private void BoardChanged()
        {
            if(_blockMap is null || _board is null) return;
            
            _blockMap.ClearAllTiles();
            for(int y = 0; y < _board.Height; y++)
            {
                for(int x = 0; x < _board.Width; x++)
                {
                    if(_board.Grid[x, y] != 0)
                    {
                        _blockMap.SetTile(new Vector3Int(x, y), _blockTile);
                        _blockMap.SetColor(new Vector3Int(x, y), BlockData.GetColor(_board.Grid[x, y]));
                    }
                }
            }
        }

        private void DrawBoard()
        {
            if(_backgroundMap is null || _board is null) return;
            
            _backgroundMap.ClearAllTiles();
            for(int y = 0; y < _board.Height; y++)
            {
                for(int x = 0; x < _board!.Width; x++)
                {
                    if(y == 0 || y == _board.Height - 1 || x == 0 || x == _board.Width - 1)
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