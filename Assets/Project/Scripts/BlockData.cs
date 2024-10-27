using UnityEngine;

namespace Tetris
{
    public abstract class BlockData
    {
        public abstract Color Color { get; }
        public abstract Vector2Int[][] TilePositions { get; }
        public abstract Vector2Int StartPosition { get; }
    }
}