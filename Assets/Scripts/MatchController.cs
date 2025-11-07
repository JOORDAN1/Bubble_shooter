using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : MonoBehaviour
{
    private Board board;
    private List<Bubble> bubblesToClear = new List<Bubble>();
    
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
    
    // public void LookForMatches(Bubble bubbleToCheck)
    // {
    //     List<Bubble> matchedBubbles = new List<Bubble>();
    //     String currentBubbleName = bubbleToCheck.data.bubbleName;
    //     int currentBubbleColumn = bubbleToCheck.column;
    //     int currentBubbleRow = bubbleToCheck.row;
    //     
    //     List<Bubble> bubblesToCheck = new List<Bubble>();
    //     bubblesToCheck.Add(GetBubbleAt(currentBubbleColumn - 2, currentBubbleRow));
    //     bubblesToCheck.Add(GetBubbleAt(currentBubbleColumn + 2, currentBubbleRow));
    //     bubblesToCheck.Add(GetBubbleAt(currentBubbleColumn - 1, currentBubbleRow + 1));
    //     bubblesToCheck.Add(GetBubbleAt(currentBubbleColumn + 1, currentBubbleRow + 1));
    //     bubblesToCheck.Add(GetBubbleAt(currentBubbleColumn + 1, currentBubbleRow - 1));
    //     bubblesToCheck.Add(GetBubbleAt(currentBubbleColumn - 1, currentBubbleRow - 1));
    //     
    //
    //     for (int i = 0; i < bubblesToCheck.Count; i++)
    //     {
    //         if (bubblesToCheck[i] == null || bubblesToCheck[i].isMatched)
    //         {
    //             continue;
    //         }
    //         if (bubblesToCheck[i].data.bubbleName == currentBubbleName)
    //         {
    //             bubblesToCheck[i].MatchBubble();
    //             matchedBubbles.Add(bubblesToCheck[i]);
    //             bubblesToClear.Add(bubblesToCheck[i]);
    //         }
    //     }
    //
    //     for (int i = 0; i < matchedBubbles.Count; i++)
    //     {
    //         LookForMatches(matchedBubbles[i]);
    //     }
    // }

    public void FloodFillMatch(Bubble bubble, string targetBubbleName, List<Bubble> matches)
    {
        if (bubble == null || bubble.isMatched || matches.Contains(bubble)) return;
        
        if(bubble.data.bubbleName != targetBubbleName) return;

        matches.Add(bubble);
        
        int col =  bubble.column;
        int row =  bubble.row;
        
        FloodFillMatch(GetBubbleAt(col - 2, row), targetBubbleName, matches);
        FloodFillMatch(GetBubbleAt(col + 2, row), targetBubbleName, matches);
        FloodFillMatch(GetBubbleAt(col - 1, row + 1), targetBubbleName, matches);
        FloodFillMatch(GetBubbleAt(col + 1, row + 1), targetBubbleName, matches);
        FloodFillMatch(GetBubbleAt(col + 1, row - 1), targetBubbleName, matches);
        FloodFillMatch(GetBubbleAt(col - 1, row - 1), targetBubbleName, matches);
        
    }

    public void LookForMatches(Bubble startBubble)
    {
        List<Bubble> matchedGroup = new List<Bubble>();
        string targetBubbleName = startBubble.data.bubbleName;
        
        FloodFillMatch(startBubble, targetBubbleName, matchedGroup);

        if (matchedGroup.Count >= 3)
        {
            for (int i = 0; i < matchedGroup.Count; i++)
            {
                matchedGroup[i].MatchBubble();
                bubblesToClear.Add(matchedGroup[i]);
            }
        }
        else
        {
            return;
        }
    }

    public void ClearMatches()
    {
        if (bubblesToClear.Count >= 3)
        {
            foreach (Bubble b in bubblesToClear)
            {
                board.gridSlots[b.column, b.row].currentBubble = null;
                board.RecycleBubble(b);
            }
        }
        else
        {
            foreach (Bubble b in bubblesToClear)
            {
                b.isMatched = false;
            }
        }

        bubblesToClear.Clear();
    }
    

}
