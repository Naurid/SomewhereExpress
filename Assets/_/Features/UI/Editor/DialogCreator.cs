using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DialogCreator : EditorWindow
{
  [MenuItem("Custom/Create New Dialog")]
  static void Init()
  {
    EditorWindow window = GetWindow(typeof(DialogCreator), false, "Custom Dialog Creator");
    window.Show();
  }

  private void OnGUI()
  {
    EditorGUILayout.Space(20f);
    _dialogAuthor = EditorGUILayout.TextField("person speaking", _dialogAuthor);
    _dialogType = EditorGUILayout.TextField("type of dialog", _dialogType);
    _dialogText = EditorGUILayout.TextField("dialog text", _dialogText);

    if (GUILayout.Button("Create Dialog"))
    {
        if (_dialogAuthor == String.Empty) return;
        if (_dialogType == String.Empty) return;
        if (_dialogText == String.Empty) return;
        
        CreateDialog();
    }
    

  }
  
  private static void CreateDir(string dir)
  {
      if (!Directory.Exists(dir))
      {
          Directory.CreateDirectory(dir);
      }
  }
  

  private void CreateDialog()
  {
      string path = $"Assets/_/Database/Dialogs/Custom/{_dialogAuthor}/{_dialogType}";
      CreateDir(path);

      Dialog newDialog = ScriptableObject.CreateInstance<Dialog>();
      
      newDialog.m_name = _dialogAuthor;
      newDialog.m_message = _dialogText;
      path = $"Assets/_/Database/Dialogs/Custom/{_dialogAuthor}/{_dialogType}/{_dialogText}.asset";
      AssetDatabase.CreateAsset(newDialog, path);
  }

  private string _dialogAuthor;
  private string _dialogType;
  private string _dialogText;
}
