using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject bubblePrefab;
    public BubblesDatabase  bubblesDatabase;

    public float bubbleSpacing = 0.1f;
    
    public int width = 19;
    public int height = 12;
    [SerializeField] private BubblesDatabase gemDatabase;
    public GridSlot[,] gridSlots;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        
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

            // Pomijamy sloty z pierwszych 3 rzędów
            if (row < 3)
                continue;

            // Pomijamy sloty niepasujące do heksagonalnego układu
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
        }
    }
    
    
}
