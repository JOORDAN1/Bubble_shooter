using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    
    public static Board Instance;
    public Shooter shooter;
    private MatchController matchController;

    public GameObject bubblePrefab;
    public BubblesDatabase  bubblesDatabase;

    public float bubbleSpacing = 0.1f;
    
    public int width = 19;
    public int height = 13;
    [SerializeField] private BubblesDatabase gemDatabase;
    public GridSlot[,] gridSlots;
    public List<Bubble> allBubbles = new List<Bubble>();
    public List<Bubble> bubblesToUse = new List<Bubble>();
    
    public GameObject left;
    public GameObject right;
    public GameObject top;
    public GameObject bottom;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        shooter = GameObject.FindWithTag("Shooter").GetComponent<Shooter>();
        matchController = GameObject.FindWithTag("MatchController").GetComponent<MatchController>();
        Instance = this;
        gridSlots = new GridSlot[width, height];
        FillingBoard();
    }


    private void FillingBoard()
    {
        GridSlot[] allGridSlots = GetComponentsInChildren<GridSlot>();

        for (int i = 0; i < allGridSlots.Length; i++)
        {
            GridSlot slot = allGridSlots[i];
            int col = slot.column;
            int row = slot.row;
            
            if (col < 0 || col >= width || row < 0 || row >= height)
            {
                Debug.LogError($"Slot ({col},{row}) is OUTSIDE the board bounds!");
                continue;
            }

            gridSlots[col, row] = slot;
            
            if (row < 4)
                continue;
            
            if ((col % 2 == 0 && row % 2 != 0) || (col % 2 != 0 && row % 2 == 0))
                continue;

            Bubble bubble = slot.GetComponentInChildren<Bubble>(true);
            slot.currentBubble = bubble;

            if (bubble == null)
            {
                Debug.LogWarning($"Missing Bubble on Slot ({col}, {row})");
                continue;
            }

            BubbleTypeData randomType = bubblesDatabase.GetRandomBubbleType();
            bubble.Init(randomType);
            bubble.column = col;
            bubble.row = row;
            bubble.isFlying = false;
            
            Debug.Log(gridSlots);
        }
    }
    
    public GridSlot FindNearestAvailableSlot(Vector2 position)
    {
        GridSlot nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var slot in gridSlots)
        {
            if (slot == null || slot.currentBubble != null)
                continue;

            float dist = Vector2.Distance(position, slot.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = slot;
            }
        }
        
        return nearest;
    }

    public GridSlot FindAvalivableLeftSlot()
    {
        List<(int col, int row)> preferredSlots = new List<(int, int)>
        {
            (0, 12),
            (1, 11),
            (0, 10),
            (1, 9),
            (0, 8),
            (1, 7),
            (0, 6),
            (1, 5),
            (0, 4),
            (1, 3),
            (0, 2),
            (1, 1),
            (0, 0),
        };

        foreach (var (col, row) in preferredSlots)
        {
            if (col >= 0 && col < width && row >= 0 && row < height)
            {
                GridSlot slot = gridSlots[col, row];
                if (slot != null && slot.currentBubble == null)
                {
                    return slot;
                }
            }
        }

        return null;
    }
    
    
    public GridSlot FindAvalivableRightSlot()
    {
        List<(int col, int row)> preferredSlots = new List<(int, int)>
        {
            (18, 12),
            (17, 11),
            (18, 10),
            (17, 9),
            (18, 8),
            (17, 7),
            (18, 6),
            (17, 5),
            (18, 4),
            (17, 3),
            (18, 2),
            (17, 1),
            (18, 0),
        };

        foreach (var (col, row) in preferredSlots)
        {
            if (col >= 0 && col < width && row >= 0 && row < height)
            {
                GridSlot slot = gridSlots[col, row];
                if (slot != null && slot.currentBubble == null)
                {
                    return slot;
                }
            }
        }

        return null;
    }
    
    public void OnBubbleSettled()
    {
        shooter.SpawnBubble();
        matchController.ClearMatches();
        matchController.ClearDisconnectedBubbles();
    }
    
    public void RecycleBubble(Bubble bubble)
    {
        bubble.ClearBubble();
        bubblesToUse.Add(bubble);
    }

    public List<string> CheckBubbleTypes()
    {
        List<string> allTypes = new List<string>();
    
        for (int col = 0; col < width; col++)
        {
            for (int row = 0; row < height; row++)
            {
                Bubble bubble = gridSlots[col, row]?.currentBubble;
                if (bubble != null)
                {
                    if (!allTypes.Contains(bubble.data.bubbleName))
                    {
                        allTypes.Add(bubble.data.bubbleName);
                    }
                }
            }
        }

        return allTypes;
    }
    
    
}
