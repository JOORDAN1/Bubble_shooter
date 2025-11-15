using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    public GameObject bubblePrefab;
    public BubblesDatabase bubblesDatabase;
    
    private Bubble currentBubble;
    public float shootForce = 10f;

    void Start()
    {
        SpawnBubble();
    }
    
    void Update()
    {
        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            ShootToward(Camera.main.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue()));
        }
        
    }

    public void SpawnBubble()
    {
        GameObject go;

        if (Board.Instance.bubblesToUse.Count > 0)
        {
            Bubble recycled = Board.Instance.bubblesToUse[0];
            Board.Instance.bubblesToUse.RemoveAt(0);
            go = recycled.gameObject;
            go.transform.position = transform.position;
        }
        else
        {
            go = Instantiate(bubblePrefab, transform.position, Quaternion.identity);
        }

        currentBubble = go.GetComponent<Bubble>();
        currentBubble.Init(bubblesDatabase.GetRandomBubbleType());
    }
    
    void ShootToward(Vector3 worldTarget)
    {
        if (currentBubble == null) return;

        Vector2 direction = (worldTarget - currentBubble.transform.position).normalized;

        Rigidbody2D rb = currentBubble.GetComponent<Rigidbody2D>();
        currentBubble.isFlying = true;
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0;
            rb.linearVelocity = direction * shootForce;
        }
        
        currentBubble = null;
    }
}
