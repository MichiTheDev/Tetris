using JetBrains.Annotations;
using UnityEngine;

namespace Tetris
{
    public sealed class BlockQueue
    {
        [NotNull] public Block NextBlock { get; private set; } = null!;

        [NotNull] private readonly BlockData[] _blockDataArray = null!;

        private readonly Vector2Int _spawnPosition;
        
        public BlockQueue(Vector2Int spawnPosition)
        {
            TextAsset json = Resources.Load<TextAsset>("BlockData");
            if(json is null)
            {
                Debug.Log("Json file for BlockData couldn't be loaded.");
                return;
            }

            _spawnPosition = spawnPosition;
            _blockDataArray = BlockData.LoadBlockData(json.text);
            NextBlock = GetRandomBlock();
        }

        [NotNull] public Block GetNextBlock()
        {
            Block block = NextBlock;

            do NextBlock = GetRandomBlock();
            while(block.ID == NextBlock!.ID);
            
            return block;
        }

        [NotNull] private Block GetRandomBlock()
        {
            int randomIndex = Random.Range(0, _blockDataArray.Length);
            BlockData blockData = _blockDataArray[randomIndex];
            return new Block(blockData, _spawnPosition, Random.Range(0, blockData.TilePositions.Length));
        }
    }
}