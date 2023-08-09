using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DialogImport : EditorWindow
{
   [MenuItem("Custom/Import Dialogs from dialog Sheet(.tsv)")]
   static void Init()
   {
      EditorWindow window = GetWindow(typeof(DialogImport), false, "Import DialogSheet");
      window.Show();
   }

   private void OnGUI()
   {
      _dialogSheet = (TextAsset)EditorGUILayout.ObjectField ("dialogSheet", _dialogSheet, typeof(TextAsset), false);

      if (GUILayout.Button("Import Dialogs"))
      {
         if (_dialogSheet == null) return;
         RefreshDialogDataBase();
      }
   }
   
   private void RefreshDialogDataBase()
   {
      index = 0;
      string dir = $"Assets/_/Database/Dialogs/{_dialogSheet.name}";

      CreateDir(dir);
      
      foreach (var file in Directory.GetFiles(dir))
      {
         File.Delete(file);
      }

      _stringList = _dialogSheet.text.Split(new string[] { "\t", "\n" }, StringSplitOptions.None).ToList();

      for (int i = 3; i < _stringList.Count; i++)
      {
         if (i % 3 == 0)
         {
            index++;
            dir = $"Assets/_/Database/Dialogs/{_dialogSheet.name}/{_stringList[i]}/{_stringList[i + 1]}";
            CreateDir(dir);
            dir = $"Assets/_/Database/Dialogs/{_dialogSheet.name}/{_stringList[i]}/{_stringList[i + 1]}/{_stringList[i+1]}{index}.asset";
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

   private TextAsset _dialogSheet;
   
   private int index;
   private List<string> _stringList = new ();
}
