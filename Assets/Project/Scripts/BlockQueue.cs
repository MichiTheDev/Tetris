using UnityEngine;

namespace Tetris
{
    public sealed class BlockQueue
    {
        public Block NextBlock { get; private set; }

        private readonly BlockData[] _blockDataArray;
        
        public BlockQueue(string json)
        {
            _blockDataArray = BlockData.LoadBlockData(json);
            NextBlock = GetRandomBlock();
        }

        public Block GetNextBlock()
        {
            if(NextBlock is null) return null;
            
            Block nextBlock = NextBlock;

            do NextBlock = GetRandomBlock();
            while(nextBlock.BlockData.ID != NextBlock!.BlockData.ID);
            
            return nextBlock;
        }

        private Block GetRandomBlock()
        {
            if(_blockDataArray is null) return null;
            
            int randomIndex = Random.Range(0, _blockDataArray.Length);
            BlockData blockData = _blockDataArray[randomIndex];
            return new Block(blockData, Random.Range(0, blockData.TilePositions.Length));
        }
    }
}