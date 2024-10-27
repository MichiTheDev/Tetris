using UnityEngine;

namespace Tetris
{
    public sealed class LBlockData : BlockData
    {
        public override Color Color => new(1f, 0.5f, 0f, 1f);
        public override Vector2Int[][] TilePositions => new[]
        {
            new Vector2Int[] { new(0, 1), new(1, 1), new(2, 1), new(2, 0) },
            new Vector2Int[] { new(1, 0), new(1, 1), new(1, 2), new(2, 2) },
            new Vector2Int[] { new(0, 1), new(1, 1), new(2, 1), new(0, 2) },
            new Vector2Int[] { new(0, 0), new(1, 0), new(1, 1), new(1, 2) },
        };
        
        public override Vector2Int StartPosition => new(3, 16);
    }
}