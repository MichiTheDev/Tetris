using UnityEngine;

namespace Tetris
{
    public sealed class Block
    {
        public BlockData BlockData { get; }
        public Vector2Int Position { get; private set; }
        
        private int _rotationState;
        
        public Block(BlockData blockData)
        {
            BlockData = blockData;
            Position = blockData!.StartPosition;
            _rotationState = Random.Range(0, BlockData.TilePositions!.Length);
        }

        public Vector2Int[] GetTilePositions()
        {
            Vector2Int[] updatedTilePositions = BlockData!.TilePositions![_rotationState]!;
            for(int i = 0; i < updatedTilePositions.Length; i++)
            {
                updatedTilePositions[i] += Position;
            }
            return updatedTilePositions;
        }

        public void Move(Vector2Int direction)
        {
            Position += direction;
        }

        public void Rotate(int direction)
        {
            if(_rotationState == 0)
            {
                _rotationState = BlockData!.TilePositions!.Length - 1;
                return;
            }
            _rotationState = (_rotationState + direction) % BlockData!.TilePositions!.Length;
        }
    }
}