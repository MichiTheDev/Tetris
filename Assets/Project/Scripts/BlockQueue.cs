using UnityEngine;

namespace Tetris
{
    public sealed class BlockQueue
    {
        private readonly BlockData[] _blockDataList =
        {
            new IBlockData(),
            new JBlockData(),
            new LBlockData(),
            new OBlockData(),
            new SBlockData(),
            new TBlockData(),
            new ZBlockData()
        };
        
        public Block NextBlock { get; private set; }

        public BlockQueue()
        {
            NextBlock = GetRandomBlock();
        }
        
        public Block GetBlock()
        {
            Block block = NextBlock;
            do
            {
                NextBlock = GetRandomBlock();
            } 
            while(block!.BlockData!.Color == NextBlock!.BlockData!.Color);
            
            return block;
        }

        private Block GetRandomBlock()
        {
            int randomIndex = Random.Range(0, _blockDataList!.Length);
            BlockData blockData = _blockDataList![randomIndex];
            return new Block(blockData);
        }
    }
}