using System;
using System.Collections.Generic;
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
        // List<string> allTypes = Board.Instance.CheckBubbleTypes();
        //
        // BubbleTypeData newdata = bubblesDatabase.GetRandomBubbleType();
        //
        // while (!allTypes.Contains(newdata.bubbleName))
        // {
        //     newdata = bubblesDatabase.GetRandomBubbleType();
        // }
        
        List<string> allTypes = Board.Instance.CheckBubbleTypes();
        List<BubbleTypeData> availableTypes = bubblesDatabase.bubbleTypes.FindAll(type => allTypes.Contains(type.bubbleName));

        if (availableTypes.Count == 0)
        {
            // Gra skończona, albo trzeba wygenerować nowy układ
            Debug.Log("Brak dostępnych bąbelków — wygrana lub nowy level");
            return;
        }
        else
        {
            BubbleTypeData newdata = availableTypes[UnityEngine.Random.Range(0, availableTypes.Count)];

            currentBubble = go.GetComponent<Bubble>();
            currentBubble.Init(newdata);
        }
        
        
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
