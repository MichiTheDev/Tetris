using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Tetris
{
    public sealed class Block
    {
        public Vector2Int Position { get; private set; }
        public int ID => _blockData.ID;
        
        private readonly BlockData _blockData;
        private int _rotationState = 0;

        public Block(BlockData blockData, Vector2Int position, int rotationState = 0)
        {
            _blockData = blockData;
            Position = _blockData.StartOffset + position;
            _rotationState = rotationState;
        }

        public void Move(Vector2Int direction)
        {
            Position += direction;
        }

        public void Rotate(int direction)
        {
            int rotationStates = _blockData.TilePositions!.Length;
            
            _rotationState += direction;
            if(_rotationState < 0) _rotationState = rotationStates - 1;
            else if(_rotationState >= rotationStates) _rotationState = 0;
        }
        
        [NotNull] public Vector2Int[] GetTilePositions()
        {
            Vector2Int[][] tilePositions = _blockData.TilePositions;
            Vector2Int[] currentTilePositions = tilePositions?[_rotationState];
            Vector2Int[] result = new Vector2Int[currentTilePositions!.Length];

            for(int i = 0; i < currentTilePositions!.Length; i++)
            {
                result[i] = Position + currentTilePositions[i];
            }

            return result;
        }
    }
    
    public struct BlockData
    {
        public int ID;
        public Vector2Int[][] TilePositions;
        public Vector2Int StartOffset;

        public static Color GetColor(int id)
        {
            return id switch
            {
                1 => Color.red,
                2 => Color.cyan,
                3 => Color.blue,
                4 => new Color(1f, 0.5f, 0f, 1f),
                5 => Color.yellow,
                6 => Color.green,
                7 => new Color(0.5f, 0f, 1f, 1f),
                _ => new Color(0f, 0f, 0f, 0f)
            };
        }
        
        [NotNull] public static BlockData[] LoadBlockData(string json)
        {
            SerializedBlockDataArray serializableBlockDataArray = JsonUtility.FromJson<SerializedBlockDataArray>(json);
            
            SerializableBlockData[] serializableBlockData = serializableBlockDataArray.Array;
            BlockData[] blockData = new BlockData[serializableBlockData.Length];

            for (int i = 0; i < serializableBlockData.Length; i++)
            {
                SerializableBlockData data = serializableBlockData[i];
                blockData[i] = new BlockData
                {
                    ID = data.ID,
                    TilePositions = ConvertPositions(data.TilePositions),
                    StartOffset = new Vector2Int(data.StartOffset.X, data.StartOffset.Y)
                };
            }

            return blockData;
        }

        private static Vector2Int[][] ConvertPositions(TilePositionArray[] tilePositionArrays)
        {
            if (tilePositionArrays == null) return null;

            Vector2Int[][] convertedTilePositions = new Vector2Int[tilePositionArrays.Length][];
            for (int i = 0; i < tilePositionArrays.Length; i++)
            {
                Position[] positions = tilePositionArrays[i].Positions;
                if (positions == null) continue;

                convertedTilePositions[i] = new Vector2Int[positions.Length];
                for (int j = 0; j < positions.Length; j++)
                {
                    convertedTilePositions[i][j] = new Vector2Int(positions[j].X, positions[j].Y);
                }
            }

            return convertedTilePositions;
        }

        [Serializable]
        private struct SerializedBlockDataArray
        {
            public SerializableBlockData[] Array;
        }

        [Serializable]
        private struct SerializableBlockData
        {
            public int ID;
            public TilePositionArray[] TilePositions;
            public Position StartOffset;
        }

        [Serializable]
        private struct TilePositionArray
        {
            public Position[] Positions;
        }

        [Serializable]
        private struct Position
        {
            public int X;
            public int Y;
        }
    }
}