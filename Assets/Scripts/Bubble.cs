using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{

    public BubbleTypeData data;
    public SpriteRenderer spriteRenderer;
    public int column;
    public int row;
    public bool isFlying = false;
    public Board board;
    public bool hasSettled = false;
    public bool isMatched = false;
    public MatchController matchController;

    private void Awake()
    {
        
       board = GameObject.FindWithTag("Board").GetComponent<Board>();
       matchController = GameObject.FindWithTag("MatchController").GetComponent<MatchController>();
       if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
    }
    
    public void Init(BubbleTypeData newdata)
    {
        data = newdata;
        spriteRenderer.sprite = data.sprite;
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
    }

    private void Update()
    {
        CheckLeftAndRight();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFlying || hasSettled) return;

        isFlying = false;
        hasSettled = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        
        GridSlot nearestSlot = board.FindNearestAvailableSlot(transform.position);
        if (nearestSlot != null)
        {
            transform.position = nearestSlot.transform.position;
            nearestSlot.currentBubble = this;
            column = nearestSlot.column;
            row = nearestSlot.row;
            
            
            matchController.CheckMatches(this);
            board.OnBubbleSettled();
            
        }
        
        GetComponent<Collider2D>().isTrigger = false;
        
    }

    private void CheckLeftAndRight()
    {
        if (!hasSettled)
        {
            if (transform.position.x < board.left.transform.position.x)
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Static;
                GridSlot leftSlot = board.FindAvalivableLeftSlot();
                transform.position = leftSlot.transform.position;
                leftSlot.currentBubble = this;
                column = leftSlot.column;
                row = leftSlot.row;
                hasSettled = true;
                board.OnBubbleSettled();
                GetComponent<Collider2D>().isTrigger = false;
            }

            if (transform.position.x > board.right.transform.position.x)
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Static;
                GridSlot rightSlot = board.FindAvalivableRightSlot();
                transform.position = rightSlot.transform.position;
                rightSlot.currentBubble = this;
                column = rightSlot.column;
                row = rightSlot.row;
                hasSettled = true;
                board.OnBubbleSettled();
                GetComponent<Collider2D>().isTrigger = false;
            }
        }
    }
    
    public void MatchBubble()
    {
        isMatched = true;
        spriteRenderer.color = Color.black;
    }
    
    
}
