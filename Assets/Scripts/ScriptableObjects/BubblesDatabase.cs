using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BubblesDatabase", menuName = "Scriptable Objects/BubblesDatabase")]
public class BubblesDatabase : ScriptableObject
{
    public List<BubbleTypeData> bubbleTypes;

    public BubbleTypeData GetRandomBubbleType()
    {
        return bubbleTypes[Random.Range(0, bubbleTypes.Count)];
    }

}
