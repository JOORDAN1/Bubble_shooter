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

    public void ClearDisconnectedBubbles()
    {
        List<Bubble> allBubbles = new List<Bubble>();
    
        for (int col = 0; col < board.width; col++)
        {
            for (int row = 0; row < board.height; row++)
            {
                Bubble bubble = board.gridSlots[col, row]?.currentBubble;
                if (bubble != null)
                {
                    allBubbles.Add(bubble);
                }
            }
        }
    
        for (int i = 0; i < allBubbles.Count; i++)
        {
            HashSet<Bubble> visited = new HashSet<Bubble>();
            if (!IsConnectedToTop(allBubbles[i], visited))
            {
                for (int j = 0; j < visited.Count; j++)
                {
                    foreach (var b in visited)
                    {
                        if (!b.cleared)
                        {
                            b.ClearBubble();
                            board.gridSlots[b.column, b.row].currentBubble = null;
                            board.bubblesToUse.Add(b);
                        }
                    }
                }
            }
        }
    }
    

    private bool IsConnectedToTop(Bubble bubble, HashSet<Bubble> visited)
    {
        if(bubble == null || visited.Contains(bubble)) return false;
        
        visited.Add(bubble);
        
        if (bubble.row == board.height - 1)
            return true;
        
        int col = bubble.column;
        int row = bubble.row;
        return 
            IsConnectedToTop(GetBubbleAt(col - 2, row), visited) ||
            IsConnectedToTop(GetBubbleAt(col + 2, row), visited) ||
            IsConnectedToTop(GetBubbleAt(col - 1, row + 1), visited) ||
            IsConnectedToTop(GetBubbleAt(col + 1, row + 1), visited) ||
            IsConnectedToTop(GetBubbleAt(col + 1, row - 1), visited) ||
            IsConnectedToTop(GetBubbleAt(col - 1, row - 1), visited);
    }
    

    

}
