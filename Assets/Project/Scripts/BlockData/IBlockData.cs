using UnityEngine;

namespace Tetris
{
    public sealed class IBlockData : BlockData
    {
        public override Color Color => Color.cyan;
        public override Vector2Int[][] TilePositions => new[]
        {
            new Vector2Int[] { new(0, 1), new(1, 1), new(2, 1), new(3, 1) },
            new Vector2Int[] { new(2, 0), new(2, 1), new(2, 2), new(2, 3) },
            new Vector2Int[] { new(0, 2), new(1, 2), new(2, 2), new(3, 2) },
            new Vector2Int[] { new(1, 0), new(1, 1), new(1, 2), new(1, 3) },
        };

        public override Vector2Int StartPosition => new(3, 15);
    }
}