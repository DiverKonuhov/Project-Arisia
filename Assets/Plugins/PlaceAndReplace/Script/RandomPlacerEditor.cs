using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(RandomPlacer)), CanEditMultipleObjects]
public class RandomPlacerEditor : Editor 
{
    public RandomPlacer randomPlacer;

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        randomPlacer = (RandomPlacer)target;

        //Button in Inpector to find all objects of name and replace them
        if (GUILayout.Button("Place Objects"))
        {
            randomPlacer.PlaceObjects();
        }

        

    }

    /*protected virtual void OnSceneGUI()
    {
        RandomPlacer randomPlacer = (RandomPlacer)target;

        Vector3 position = randomPlacer.transform.position + Vector3.up * 2f;
        float size = 2f;
        float pickSize = size * 2f;

        if (Handles.Button(position, Quaternion.identity, size, pickSize, Handles.RectangleHandleCap))
            Debug.Log("The button was pressed!");
    }*/

    /* public void OnSceneGUI()
     {
         randomPlacer = (RandomPlacer)target;

         Handles.BeginGUI();

         // Starts an area to draw elements
         GUILayout.BeginArea(new Rect(10, 10, 100, 100));
         if (GUILayout.Button("Press Me"))
         {
             randomPlacer.PlaceObjects();
         }




         GUILayout.EndArea();
         Handles.EndGUI();
     }*/

   /* public void OnSceneGUI()
    {
        randomPlacer = (RandomPlacer)target;

        Handles.BeginGUI();

        // Starts an area to draw elements
        GUILayout.BeginArea(new Rect(10, 10, 100, 100));
        if (GUILayout.Button("Press Me"))
        {
            randomPlacer.PlaceObjects();
        }
        GUILayout.TextField("Hello", 25);



        GUILayout.EndArea();
        Handles.EndGUI();
    }*/

}
