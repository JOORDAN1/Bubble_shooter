using UnityEngine;

public class Board : MonoBehaviour
{
    
    public static Board Instance;
    public Shooter shooter;

    public GameObject bubblePrefab;
    public BubblesDatabase  bubblesDatabase;

    public float bubbleSpacing = 0.1f;
    
    public int width = 19;
    public int height = 13;
    [SerializeField] private BubblesDatabase gemDatabase;
    public GridSlot[,] gridSlots;

    public GameObject left;
    public GameObject right;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        shooter = GameObject.FindWithTag("Shooter").GetComponent<Shooter>();
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
        GridSlot left = null;

        if (gridSlots[1, 3].currentBubble == null)
        {
            left = gridSlots[1, 3];
        }
        else if (gridSlots[0, 2].currentBubble == null)
        {
            left = gridSlots[0, 2];
        }
        else if (gridSlots[1, 1].currentBubble == null)
        {
            left = gridSlots[1, 1];
        }
        else
        {
            left = gridSlots[0, 0];
        }


        return left;
    }
    
    
    public GridSlot FindAvalivableRightSlot()
    {
        GridSlot right = null;

        if (gridSlots[17, 3].currentBubble == null)
        {
            right = gridSlots[17, 3];
        }
        else if (gridSlots[18, 2].currentBubble == null)
        {
            right = gridSlots[18, 2];
        }
        else if (gridSlots[17, 1].currentBubble == null)
        {
            right = gridSlots[17, 1];
        }
        else
        {
            right = gridSlots[18, 0];
        }


        return right;
    }
    
    public void OnBubbleSettled()
    {
        shooter.SpawnBubble();
    }


    
}
