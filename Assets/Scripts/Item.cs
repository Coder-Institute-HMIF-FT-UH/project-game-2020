using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string objectName;
    public bool canBePicked;
    public bool canBeCollected;
}
