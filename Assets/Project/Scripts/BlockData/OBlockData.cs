using UnityEngine;

namespace Tetris
{
    public sealed class OBlockData : BlockData
    {
        public override Color Color => Color.yellow;
        public override Vector2Int[][] TilePositions => new[]
        {
            new Vector2Int[] { new(1, 0), new(2, 0), new(1, 1), new(2, 1) }
        };
        
        public override Vector2Int StartPosition => new(3, 17);
    }
}