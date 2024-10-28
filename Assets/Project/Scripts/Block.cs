using System;
using UnityEngine;

namespace Tetris
{
    public sealed class Block
    {
        public Vector2Int Position { get; private set; }
        public readonly BlockData BlockData;
        
        private int _rotationState = 0;

        public Block(BlockData blockData, int rotationState = 0)
        {
            BlockData = blockData;
            Position = BlockData.StartOffset;
            _rotationState = rotationState;
        }

        public Vector2Int[] GetTilePositions()
        {
            Vector2Int[][] tilePositions = BlockData.TilePositions;
            
            Vector2Int[] currentTilePositions = tilePositions?[_rotationState];
            if(currentTilePositions is null) return null;
            
            for(int i = 0; i < currentTilePositions.Length; i++)
            {
                currentTilePositions[i] += Position;
            }

            return currentTilePositions;
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
                _ => Color.black
            };
        }
        
        public static BlockData[] LoadBlockData(string json)
        {
            SerializedBlockDataArray serializableBlockDataArray = JsonUtility.FromJson<SerializedBlockDataArray>(json);
            
            if (serializableBlockDataArray.Array is null) return null;

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