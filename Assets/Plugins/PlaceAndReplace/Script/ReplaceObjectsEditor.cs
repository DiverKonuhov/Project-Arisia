using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ReplaceObjects)), CanEditMultipleObjects]
public class ReplaceObjectsEditor : Editor
{
    ReplaceObjects replaceObjs;
    RectOffset rctOff;

    public string replacedStatus;

    public int replacedSoFar;

    public int ToReplace;

    public string ObjectsFound;

    public string TagChanging;

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        replaceObjs = (ReplaceObjects)target;

        DrawHorizontalGUILine();

        //Button in Inpector to find all objects of name and replace them
        EditorGUILayout.LabelField("Replace Objects:", EditorStyles.boldLabel);

        GUILayout.Space(16);
        if (GUILayout.Button("Replace Objects"))
        {
            if(replaceObjs.Changing == false)
            {
                bool ObjectToReplace = false;
                ObjectToReplace = replaceObjs.ReplaceAllCheck(ObjectToReplace);

                if (ObjectToReplace == true)
                {
                    replaceObjs.ReplaceAll();
                    replacedStatus = "Replacing";
                    replacedSoFar = replaceObjs.replacedSoFarGUI(replacedSoFar);
                    ToReplace = replaceObjs.amountToChange(ToReplace);
                    ObjectsFound = replaceObjs.objectFound(ObjectsFound);
                }
                else
                {
                    replacedStatus = "Nothing to Replace.";
                    replacedSoFar = 0;
                    ToReplace = 0;
                }
            }
            

            

        }
        string status = "Status: " + replacedStatus;
        GUILayout.TextField(status, 45);
        GUILayout.Space(4);
        //Button in Inpector to change between the new and old objects
        if (GUILayout.Button("View Old/New"))
        {
            replaceObjs.ViewOldOrNew();
        }
        GUILayout.Space(4);
        //Button in inspector to undo replace all objects
        if (GUILayout.Button("Undo Replace"))
        {
            replacedStatus = "Replacement Undone.";
            replaceObjs.UndoReplaceObjects();
        }
        GUILayout.Space(4);
        //Button in Inpector to complete the replacement
        if (GUILayout.Button("Finished Replacment"))
        {
            replaceObjs.SetObjectReplace();
            replacedStatus = "Finished Replacing";
            replacedSoFar = replaceObjs.replacedSoFarGUI(replacedSoFar);
            ToReplace = replaceObjs.amountToChange(ToReplace);
        }
        string found = replaceObjs.objectFound(ObjectsFound);
        GUILayout.TextField(found, 45);
        string amount = "Replaced " + replaceObjs.replacedSoFarGUI(replacedSoFar) + " of " + replaceObjs.amountToChange(ToReplace);
        GUILayout.TextField(amount, 45);


        DrawHorizontalGUILine();

        EditorGUILayout.LabelField("Change Tags:", EditorStyles.boldLabel);
        if (GUILayout.Button("Change Tags Now"))
        {
            replaceObjs.ChooseToReplaceTag();
        }
        string tagChange = replaceObjs.ReplacingTags(TagChanging);
        GUILayout.TextField(tagChange, 25);
    }



   /* public void OnSceneGUI()
    {
        replaceObjs = (ReplaceObjects)target;

        string status = "Status: " + replacedStatus;
        string amount = "Replaced " + (replaceObjs.replacedSoFarGUI(replacedSoFar) - 1) + " of " + replaceObjs.amountToChange(ToReplace);
        string found = replaceObjs.objectFound(ObjectsFound);
        string tagChange = replaceObjs.ReplacingTags(TagChanging);


        Handles.BeginGUI();

        // Starts an area to draw elements
        GUILayout.BeginArea(new Rect(10, 10, 150, 120));
        
        GUILayout.TextField(status, 25);
        GUILayout.TextField(found, 25);
        GUILayout.TextField(amount, 25);
        GUILayout.TextField(tagChange, 25);



        GUILayout.EndArea();
        Handles.EndGUI();
    }*/


    private static void DrawHorizontalGUILine(int height = 1)
    {
        GUILayout.Space(20);

        Rect rect = GUILayoutUtility.GetRect(10, height, GUILayout.ExpandWidth(true));
        rect.height = height;
        rect.xMin = 0;
        rect.xMax = EditorGUIUtility.currentViewWidth;

        Color lineColor = new Color(0.10196f, 0.10196f, 0.10196f, 1);
        EditorGUI.DrawRect(rect, lineColor);
        GUILayout.Space(20);
    }


}
