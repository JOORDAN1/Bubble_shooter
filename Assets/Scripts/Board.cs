using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject bubblePrefab;
    public BubblesDatabase  bubblesDatabase;

    public int rows = 6;
    public int columns = 8;

    public float bubbleSpacing = 0.1f;

    private float bubbleHeight;
    private float bubbleWidth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupBubbleSize();
        GenerateBoard();
    }

    void SetupBubbleSize()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        bubbleHeight  = spriteRenderer.bounds.size.y + bubbleSpacing;
        bubbleWidth = spriteRenderer.bounds.size.x + bubbleSpacing;
    }

    void GenerateBoard()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                float xOffset;
                if (row % 2 == 1)
                {
                    xOffset = bubbleWidth / 2f;
                }
                else
                {
                    xOffset = 0f;
                }
                
                Vector2 spawnPos = new Vector2(
                    transform.position.x + column * bubbleWidth +  xOffset,
                    transform.position.y + row * bubbleHeight
                    );
            }
        }
    }
}
