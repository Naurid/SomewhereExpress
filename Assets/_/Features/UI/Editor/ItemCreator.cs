using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ItemCreator : EditorWindow
{
    #region Unity API

    [MenuItem("Custom/Create New Item")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(ItemCreator), false, "Custom Item Creator");
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(20f);
        _itemName = EditorGUILayout.TextField("item name", _itemName);
        _itemDescription = EditorGUILayout.TextField("item description", _itemDescription);
        
        EditorGUILayout.Space(20f);
        _isItemStackable = EditorGUILayout.ToggleLeft("Is this Item stackable?", _isItemStackable);

        if (_isItemStackable)
        {
            _stackSize = EditorGUILayout.IntField(_stackSize);
        }
        else
        {
            _stackSize = 1;
        }
        
        EditorGUILayout.Space(20f);
        _itemSprite = (Sprite)EditorGUILayout.ObjectField(_itemSprite, typeof(Sprite), false);
        _itemPrefab = (GameObject)EditorGUILayout.ObjectField(_itemPrefab, typeof(GameObject), false);
        
        EditorGUILayout.Space(20f);
        if (GUILayout.Button("Create Item"))
        {
            if (_itemName == String.Empty) return;
            if (_itemDescription == String.Empty) return;
            if (_itemSprite == null) return;
            if (_itemPrefab == null) return;

            CreateItem();
        }
    }

    #endregion


    #region Main Methods

    private void CreateItem()
    {
        string dir = "Assets/_/Database/Items";
        CreateDir(dir);
        
        ItemData newItemData = ScriptableObject.CreateInstance<ItemData>();
        
        newItemData.m_itemName = _itemName;
        newItemData.m_itemDescription = _itemDescription;
        newItemData.m_isItemStackable = _isItemStackable;
        newItemData.m_stackSize = _stackSize;
        newItemData.m_itemSprite = _itemSprite;
        newItemData.m_itemPrefab = _itemPrefab;

        dir = $"Assets/_/Database/Items/{_itemName}.asset";
        AssetDatabase.CreateAsset(newItemData, dir);

    }
    
    private static void CreateDir(string dir)
    {
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }


    #endregion

    
    #region Private and Protected

    private Sprite _itemSprite;
    private GameObject _itemPrefab;
    
    private string _itemName;
    private string _itemDescription;
    
    private bool _isItemStackable;
    private int _stackSize;


    #endregion
 
}
