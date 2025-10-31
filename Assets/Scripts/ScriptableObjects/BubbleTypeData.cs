using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BubbleTypeData", menuName = "Scriptable Objects/BubbleTypeData")]
public class BubbleTypeData : ScriptableObject
{
    public string bubbleName;
    public Sprite bubbleSprite;
}
