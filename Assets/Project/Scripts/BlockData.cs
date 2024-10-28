using System;
using UnityEngine;

namespace Tetris
{
    [Serializable]
    public struct BlockData
    {
        public int ID;
        public Vector2Int StartOffset;
        private int2[][] TilePositions;

        public Vector2Int[][] GetTilePositions()
        {
            Vector2Int[][] convertedPositions = new Vector2Int[TilePositions!.Length][];

            for (int x = 0; x < TilePositions.Length; x++)
            {
                convertedPositions[x] = new Vector2Int[TilePositions[x]!.Length];
            
                for (int y = 0; y < TilePositions[x].Length; y++)
                {
                    int2 tile = TilePositions[x][y];
                    convertedPositions[x][y] = new Vector2Int(tile.X, tile.Y);
                }
            }

            return convertedPositions;
        }
    }

    [Serializable]
    public struct int2
    {
        public int X;
        public int Y;

        public int2(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}