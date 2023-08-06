using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Templates/Item")]
public class ItemData : ScriptableObject
{
    public Sprite m_itemSprite;
    public GameObject m_itemPrefab;
    
    public string m_itemName;
    public string m_itemDescription;
    
    public bool m_isItemStackable;
    
    [Tooltip("Only fill this field if the item is stackable")]
    public int m_stackSize = 1;
}
