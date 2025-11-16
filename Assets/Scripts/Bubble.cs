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
    public bool cleared = false;
    public TimeManager timeManager;
 


    private void Awake()
    {
        
       board = GameObject.FindWithTag("Board").GetComponent<Board>();
       matchController = GameObject.FindWithTag("MatchController").GetComponent<MatchController>();
       timeManager = GameObject.FindWithTag("TimeManager").GetComponent<TimeManager>();
       if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
    }
    
    public void Init(BubbleTypeData newdata)
    {
        cleared = false;
        data = newdata;
        spriteRenderer.sprite = data.sprite;
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
        hasSettled = false;
        isFlying = false;
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
        if (nearestSlot == null)
        {
            Debug.LogWarning("Nie znaleziono slotu dla bąbla — przerywam przypisanie.");
            return;
        }
        if (nearestSlot != null)
        {
            transform.position = nearestSlot.transform.position;
            nearestSlot.currentBubble = this;
            column = nearestSlot.column;
            row = nearestSlot.row;

            matchController.LookForMatches(this);

            if (nearestSlot.row == 0 && !isMatched)
            {
                timeManager.LostGame();
            }
            else
            {
                board.OnBubbleSettled();
            }
            
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
                if (leftSlot.row == 0)
                {
                    timeManager.LostGame();
                }
                else
                {
                    matchController.LookForMatches(this);
                }
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
                if (rightSlot.row == 0)
                {
                    timeManager.LostGame();
                }
                else
                {
                    matchController.LookForMatches(this);
                }
                
            }
            
            if (transform.position.y > board.top.transform.position.y)
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Static;
                GridSlot topSlot = board.FindNearestAvailableSlot(transform.position);
                transform.position = topSlot.transform.position;
                topSlot.currentBubble = this;
                column = topSlot.column;
                row = topSlot.row;
                hasSettled = true;
                board.OnBubbleSettled();
                GetComponent<Collider2D>().isTrigger = false;
                matchController.LookForMatches(this);
            }

            if (transform.position.y < board.bottom.transform.position.y && cleared == false) 
            {
                if (transform.position.x > 0)
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
                    if (rightSlot.row == 0)
                    {
                        timeManager.LostGame();
                    }
                    else
                    {
                        matchController.LookForMatches(this);
                    }
                
                }
                else
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
                    if (leftSlot.row == 0)
                    {
                        timeManager.LostGame();
                    }
                    else
                    {
                        matchController.LookForMatches(this);
                    }
                }
            }
        }
    }
    
    public void MatchBubble()
    {
        isMatched = true;
    }

    public void ClearBubble()
    {
        cleared  = true;
        data = null;
        isMatched = false;
        spriteRenderer.sprite = null;
        spriteRenderer.color = Color.white;
        hasSettled = false;
        isFlying = false;

        // ❗ wyłącz kolizje i fizykę
        GetComponent<Collider2D>().isTrigger = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        // ❗ ukryj bąbla poza ekranem
        transform.position = new Vector3(0f, -10f, 0f);
        
    }
    

   
    
}
