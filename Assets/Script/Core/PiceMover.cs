using UnityEngine.Assertions;

public static class PiceMover
{  
    public static void SetBlockToFloat(Pice pice)
	{
		if(pice.owner == PiceOwner.Board)
		{
			pice.ForeachPiceOfBlock(onePice =>{
				// 断言：所有 block 上的 pice 都应该在 board 上
				Assert.IsTrue(onePice.owner == PiceOwner.Board, "a pice of bloack in board which owner is not set to baord.");
				Puzzle.instance.board.Remove(onePice, onePice.boardX, onePice.boardY);
				pice.owner = PiceOwner.Floating;
			});
		}
		else if(pice.owner == PiceOwner.Side)
		{
			Assert.IsTrue(pice.linkingList.Count == 0, "a linked-pice found in side. there is something wrong.");
			Puzzle.instance.side.Remove(pice);
			pice.owner = PiceOwner.Floating;
		}
		
	}

	public static void SetToSide(Pice pice, int index)
	{
		Assert.IsTrue(pice.linkingList.Count == 0, "only non-linked pice can'bt be set to side.");
		PiceMover.SetBlockToFloat(pice);
		if(index != -1)
		{
			Puzzle.instance.side.Insert(pice, index);
			pice.owner = PiceOwner.Side;
		}
		else
		{
			Puzzle.instance.side.Append(pice);
			pice.owner = PiceOwner.Side;
		}
	}

	public static void SetBlockToBoard(Pice pice, int indexX, int indexY)
	{
		PiceMover.SetBlockToFloat(pice);
		pice.ForeachPiceOfBlockWhitShift((onePice, shift)=>{
			var onePiceIndexX = indexX + shift.x;
			var onePiceIndexY = indexY + shift.y;
			Puzzle.instance.board.Put(onePice, onePiceIndexX, onePiceIndexY);
		});
	}

}