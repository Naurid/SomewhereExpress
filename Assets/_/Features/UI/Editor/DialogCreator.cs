using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogManager))]
public class DialogCreator : Editor
{

    
    #region Unity API
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogManager manager = (DialogManager)target;

        if (GUILayout.Button("Refresh Library"))
        {
            RefreshDialogDataBase(manager);
        }
    }

    #endregion


    #region Main Methods

    private void RefreshDialogDataBase(DialogManager manager)
    {
        index = 0;
        string dir = "Assets/_/Database/Dialogs";

        CreateDir(dir);
        foreach (var file in Directory.GetFiles(dir))
        {
            File.Delete(file);
        }

        _stringList = manager._dialogSheet.text.Split(new string[] { "\t", "\n" }, StringSplitOptions.None).ToList();

        for (int i = 3; i < _stringList.Count; i++)
        {
            if (i % 3 == 0)
            {
                index++;
                dir = $"Assets/_/Database/Dialogs/{_stringList[i]}/{_stringList[i + 1]}";
                CreateDir(dir);
                dir = $"Assets/_/Database/Dialogs/{_stringList[i]}/{_stringList[i + 1]}/{_stringList[i+1]}{index}.asset";
                Dialog newDialog = ScriptableObject.CreateInstance<Dialog>();
                newDialog.m_name = _stringList[i];
                newDialog.m_message = _stringList[i + 2];
                AssetDatabase.CreateAsset(newDialog, dir);
            }
        }
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

    private int index;
    private List<string> _stringList = new ();

    #endregion
}