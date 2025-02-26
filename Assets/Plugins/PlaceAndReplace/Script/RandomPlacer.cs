using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RandomPlacer : MonoBehaviour
{
    //Place you want to place objects
    [Header("*The place you want to place your objects")]
    public GameObject PlaceToPlace;
    //Floats for size of Surface
    private float placeFloorX;
    private float placeFloorZ;

    //Objects To Place
    [Header("*Object/s To Place")]
    public List<GameObject> ObjectToPlace = new List<GameObject>();

    [Header("Object to make Parent (if blank, PlaceToPlace will be parent).")]
    public GameObject ObjectsNewParent;

    //How many objects you want to place
    [Header("How many do you want to place")]
    public int AmountToPlace;
    private int AmountPlacedSoFar;

    //List of places to place
    private List<Vector3> positonOfPlacementList = new List<Vector3>();
    private int positionsFound;
    private int placementsPickTries;

    //Minimum distance of placement
    [Header("Minimum Distance BetweenPlacements")]
    public float MinDistanceBetweenPlacements;

    //Is the placing finished? Good for calling if placement breaks somewhere to stop coroutine
    [HideInInspector]public bool unfinished;


    //Function To Start Placing Objects
    public void PlaceObjects()
    {
        if(unfinished == false)
        {
            if(PlaceToPlace != null && ObjectToPlace.Count > 0)
            {
                positonOfPlacementList.Clear();

                positionsFound = 0;
                AmountPlacedSoFar = 0;

                placeFloorX = (PlaceToPlace.GetComponent<MeshFilter>().sharedMesh.bounds.max.x * PlaceToPlace.transform.localScale.x) - 1f;
                placeFloorZ = (PlaceToPlace.GetComponent<MeshFilter>().sharedMesh.bounds.max.z * PlaceToPlace.transform.localScale.z) - 1f;
                Debug.Log("Started Placing.");

                StartCoroutine("StartPlacing");
                unfinished = true;
            }
            else
            {
                Debug.Log("You have not filled out the Required Inspector Variables");
            }
            
        }
        
    }

    IEnumerator StartPlacing()
    {
        
        int placingTempCount = 0;
        while(placingTempCount < 1)
        {
            //int placementSpots = 0;
            while(positionsFound < AmountToPlace)
            {
                placementsPickTries = 0;
                Vector3 spotFound = new Vector3(0,0,0);
                spotFound = FindPlacementSpot(spotFound);
                positonOfPlacementList.Add(spotFound);

                positionsFound += 1;
            }

            foreach(Vector3 pos in positonOfPlacementList)
            {
                GameObject Obj = null;
                Obj = ChooseObject(Obj);

                Obj = Instantiate(Obj, pos, transform.rotation);

                if(ObjectsNewParent != null)
                {
                    Obj.transform.parent = ObjectsNewParent.transform;
                }
                else
                {
                    Obj.transform.parent = PlaceToPlace.transform;
                }
                

            }
                
            

            yield return new WaitForSeconds(0.1f);
            placingTempCount = 1;

            


        }
        unfinished = false;

        Debug.Log("Finished Placing.");
        StopCoroutine("StartPlacing");

    }

    public Vector3 FindPlacementSpot(Vector3 spotFound)
    {
        Vector3 placeFloor = PlaceToPlace.transform.position;
        float randomX = Random.Range(-placeFloorX, placeFloorX);
        float randomZ = Random.Range(-placeFloorZ, placeFloorZ);


        for(int i = 0; i < 10; i++)
        {
            Vector3 posTry = new Vector3(placeFloor.x + randomX, placeFloor.y + 0.5f, placeFloor.z + randomZ);

            if (positonOfPlacementList.Count > 0)
            {   
                //Check Distance
                foreach (Vector3 pos in positonOfPlacementList)
                {
                    float distance = Vector3.Distance(posTry, pos);
                    if(distance < MinDistanceBetweenPlacements)
                    {
                        //Add to attempts counter and do nothing
                        placementsPickTries += 1;
                        //break;
                    }
                    else if(positonOfPlacementList.IndexOf(pos) == positonOfPlacementList.Count - 1 && distance > MinDistanceBetweenPlacements)
                    {
                        spotFound = posTry;
                        //i = 10;
                        return spotFound;
                    }
                    else
                    {
                        //Do Nothing
                    }

                    
                }
                
                
            }
            else
            {
                spotFound = posTry;
                break;
            }
        }

        if(placementsPickTries >= 10)
        {
            spotFound = new Vector3(0, 0, 0);
            positionsFound = AmountPlacedSoFar;
        }

        return spotFound;
    }


    public GameObject ChooseObject(GameObject chosenObject)
    {
        if(ObjectToPlace.Count > 1)
        {
            int rando = Random.Range(0, ObjectToPlace.Count);
            chosenObject = ObjectToPlace[rando];
        }
        else
        {
            chosenObject = ObjectToPlace[0];
        }

        return chosenObject;
    }



    

}
