using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : MonoBehaviour
{
    private Board board;
    
    private void Awake()
    {
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
    }

    private Bubble GetBubbleAt(int column, int row )
    {
        if (column < 0 || column >= board.width || row < 0 || row >= board.height)
        {
            return null;
        }
        
        return board.gridSlots[column, row]?.currentBubble;
    }
    
    public void CheckMatches(Bubble bubbleToCheck)
    {
        List<Bubble> matchedBubbles = new List<Bubble>();
        String currentBubbleName = bubbleToCheck.data.bubbleName;
        int currentBubbleColumn = bubbleToCheck.column;
        int currentBubbleRow = bubbleToCheck.row;
        
        List<Bubble> bubblesToCheck = new List<Bubble>();
        // bubblesToCheck.Add(board.gridSlots[currentBubbleColumn - 2, currentBubbleRow].currentBubble);
        // bubblesToCheck.Add(board.gridSlots[currentBubbleColumn + 2, currentBubbleRow].currentBubble);
        // bubblesToCheck.Add(board.gridSlots[currentBubbleColumn - 1, currentBubbleRow + 1].currentBubble);
        // bubblesToCheck.Add(board.gridSlots[currentBubbleColumn + 1, currentBubbleRow + 1].currentBubble);
        // bubblesToCheck.Add(board.gridSlots[currentBubbleColumn + 1, currentBubbleRow - 1].currentBubble);
        // bubblesToCheck.Add(board.gridSlots[currentBubbleColumn - 1, currentBubbleRow - 1].currentBubble);
        bubblesToCheck.Add(GetBubbleAt(currentBubbleColumn - 2, currentBubbleRow));
        bubblesToCheck.Add(GetBubbleAt(currentBubbleColumn + 2, currentBubbleRow));
        bubblesToCheck.Add(GetBubbleAt(currentBubbleColumn - 1, currentBubbleRow + 1));
        bubblesToCheck.Add(GetBubbleAt(currentBubbleColumn + 1, currentBubbleRow + 1));
        bubblesToCheck.Add(GetBubbleAt(currentBubbleColumn + 1, currentBubbleRow - 1));
        bubblesToCheck.Add(GetBubbleAt(currentBubbleColumn - 1, currentBubbleRow - 1));
        

        for (int i = 0; i < bubblesToCheck.Count; i++)
        {
            if (bubblesToCheck[i] == null || bubblesToCheck[i].isMatched)
            {
                continue;
            }
            if (bubblesToCheck[i].data.bubbleName == currentBubbleName)
            {
                bubblesToCheck[i].MatchBubble();
                matchedBubbles.Add(bubblesToCheck[i]);
            }
        }

        for (int i = 0; i < matchedBubbles.Count; i++)
        {
            CheckMatches(matchedBubbles[i]);
        }
    }

   
    
    

}
