using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{

    public BubbleTypeData data;

    public int column;
    public int row;
    public Board board;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
       board = GameObject.FindWithTag("Board").GetComponent<Board>();
    }

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
