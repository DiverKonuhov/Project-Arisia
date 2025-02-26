using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ReplaceObjects : MonoBehaviour
{
    //Bool for if creator wants to see debug.log counting the changes
    [Header("Want To See Debug Counts?")]
    public bool SeeInDebug;

    //Tag name of the object we want to change
    [Header("Tag Name of Object To Change")]
    public string objectTag;

    //List of objects we want to change - found through tag
    private List<GameObject> ObjectsToChange = new List<GameObject>();

    //Object to change position
    private Vector3 objectToChangePos;

    //Object we want to replace with (will use list incase you want a random selection)
    [Header("Object to Replace With")]
    public List<GameObject> ObjectsToReplaceWith = new List<GameObject>();

    //private int for counter in debug.log
    private int changeCounter;
    private int ObjectToChangeCountTemp;

    //Private List of old objects to reset Changes
    private List<GameObject> OldObject = new List<GameObject>();

    //Private list of new objects for reseting
    private List<GameObject> NewObject = new List<GameObject>();

    //Private bool to make sure buttons are not overclicked
    [HideInInspector]public bool Changing;

    //Private count child count for tags
    private int childCountTags;

    //Parent for children we want to chnage tag of
    [Header("Parent for Quick Tag Change")]
    public GameObject parentForTagChange;




    // Start is called before the first frame update
    public void ReplaceAll()
    {
        if (Changing == false)
        {
            //Reset lists and start counter at 1 (since we are using a list, why not)
            changeCounter = 1;
            ObjectsToChange.Clear();
            OldObject.Clear();
            NewObject.Clear();
            Changing = false;

            //Find our objects to change in the scene
            if (objectTag != "")
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag(objectTag))
                {
                    ObjectsToChange.Add(obj);
                    ObjectToChangeCountTemp = ObjectsToChange.Count;
                    //DebugChoiceChecker("Objects found to change = " + ObjectsToChange.Count);
                }
            }
            else
            {
                DebugChoiceChecker("No name selected.");
            }
            
            //Make sure we have found objects before starting the CoroutineS
            if (ObjectsToChange.Count > 0)
            {
                //Make sure we have object to replace with
                if (ObjectsToReplaceWith.Count > 0)
                {
                    Changing = true;
                    StartCoroutine("ChangeObjects");
                }
                else
                {
                    DebugChoiceChecker("No Object To Replace With.");
                }

            }
            else
            {
                DebugChoiceChecker("No Objects Found!");
            }
        }
        else
        {
            DebugChoiceChecker("Not Finished Changing");
        }
        

    }

    public bool ReplaceAllCheck(bool hasObjectsToChange)
    {


        //Find our objects to change in the scene
        if (objectTag != "")
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag(objectTag))
            {
                ObjectsToChange.Add(obj);
                ObjectToChangeCountTemp = ObjectsToChange.Count;
                //DebugChoiceChecker("Objects found to change = " + ObjectsToChange.Count);
            }
        }


        if (ObjectsToChange.Count > 0)
        {
            hasObjectsToChange = true;
            
        }
        else
        {
            hasObjectsToChange = false;
        }

        return hasObjectsToChange;
    }

    //IEnumerator for changing objects
    IEnumerator ChangeObjects()
    {

        for (int i = ObjectsToChange.Count; i >= 1; i--)
        {
            //---
            //Variables for the old object

            //Int for getting the index of Objects Count for it's list (this is only necessary because I want the count to start from 1 - good for when we debug results)
            int index = i - 1;


            //Position of object
            Vector3 pos = ObjectsToChange[index].transform.position;

            //Parent Transform of object
            Transform parentObj = null;

            if (ObjectsToChange[index].transform.parent != null)
            {
                parentObj = ObjectsToChange[index].transform.parent.transform;
            }

            //GameObject To Replace With
            GameObject newObject = null;
            newObject = ChosenObj(newObject);

            //Add the Object to the reset list before destroying, and grab it's postiion
            OldObject.Add(ObjectsToChange[index]);

            //Destroy old object
            //DestroyImmediate(ObjectsToChange[index]);
            ObjectsToChange[index].SetActive(false);

            //Remove it from list of objects to change
            ObjectsToChange.RemoveAt(index);




            //---
            //Variable for new object

            //Instantiate new Object
            newObject = Instantiate(newObject, pos, transform.rotation);

            //Parent to old object parent
            if (parentObj != null)
            {
                newObject.transform.parent = parentObj;
            }

            //Add new object to list of new Objects added
            NewObject.Add(newObject);

            //Debug Change Count and Add to Counter
            DebugChoiceChecker(changeCounter + " of " + ObjectToChangeCountTemp + " replaced.");
            changeCounter += 1;

        }

        DebugChoiceChecker("Replace Initiated.");

        yield return new WaitForSeconds(0.1f);
        

        StopCoroutine("ChangeObjects");
    }

    //Function to return the game object we want to change with
    public GameObject ChosenObj(GameObject Chosen)
    {
        if (ObjectsToReplaceWith.Count > 1)
        {
            int rando = Random.Range(0, ObjectsToReplaceWith.Count);
            Chosen = ObjectsToReplaceWith[rando];
        }
        else
        {
            Chosen = ObjectsToReplaceWith[0];
        }

        return Chosen;
    }

    //Function to check if we want to debug, and then debug
    public void DebugChoiceChecker(string str)
    {
        if (SeeInDebug == true)
        {
            Debug.Log(str);
        }
    }

    //Function for swapping between the change
    public void ViewOldOrNew()
    {
        if(OldObject.Count > 1 && OldObject[0].activeSelf == false)
        {
            foreach(GameObject obj in NewObject)
            {
                obj.SetActive(false);
            }

            foreach(GameObject obj in OldObject)
            {
                obj.SetActive(true);
            }
        }
        else
        {

            foreach (GameObject obj in OldObject)
            {
                obj.SetActive(false);
            }

            foreach (GameObject obj in NewObject)
            {
                obj.SetActive(true);
            }

        }
    }

    //Function for undoing the replace - permenant
    public void UndoReplaceObjects()
    {
        if(NewObject.Count > 0)
        {
            for (int i = NewObject.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(NewObject[i]);
            }
            NewObject.Clear();

            foreach(GameObject obj in OldObject)
            {
                obj.SetActive(true);

            }
            OldObject.Clear();

            DebugChoiceChecker("Replacement Undone");
            Changing = false;
        }
        

    }

    //Function for completing the replace - permenant
    public void SetObjectReplace()
    {
        if (NewObject.Count > 0)
        {
            for (int i = OldObject.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(OldObject[i]);
            }
            OldObject.Clear();

            foreach (GameObject obj in NewObject)
            {
                obj.SetActive(true);

            }
            NewObject.Clear();

            DebugChoiceChecker("Replace Completed.");
            Changing = false;
        }

    }
    
    //Function for Changing Tag
    public void ChooseToReplaceTag()
    {
        List<GameObject> tempList = new List<GameObject>();
        if(parentForTagChange.transform.childCount > 0)
        {
            tempList = ChildObjectsToList(tempList, parentForTagChange);
            childCountTags = tempList.Count;

            foreach (GameObject i in tempList)
            {
                i.tag = objectTag;
            }

        }
        else
        {
            DebugChoiceChecker("No Children Found");
        }

        
    }

    public List<GameObject> ChildObjectsToList(List<GameObject> cCollection, GameObject parentObject)
    {
        foreach (Transform child in parentObject.transform)
        {
            cCollection.Add(child.gameObject);
        }

        return cCollection;
    }


    //Functions to change GUI in Editor Window
    public int replacedSoFarGUI(int replacedSoFar)
    {
        replacedSoFar = changeCounter - 1;
        return replacedSoFar;
    }

    public int amountToChange(int toChange)
    {
        toChange = ObjectToChangeCountTemp;
        return toChange;
    }

    public string objectFound(string objFound)
    {
        int found = ObjectToChangeCountTemp;

        if(ObjectToChangeCountTemp > 1)
        {
            objFound = "Found: " + found + " objects";
            return objFound;
        }
        else
        {
            objFound = "No Objects Found";
            return objFound;

        }
    }

    public string ReplacingTags(string replacing)
    {
        replacing = "Child Tags Changed = " + childCountTags;
        return replacing;
       
    }

}
