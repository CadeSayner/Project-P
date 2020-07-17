using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildModeController : MonoBehaviour
{

    // Controls the different building modes and executed functions from across the program accordingly
    
    Tile.TileType buildModeTile = Tile.TileType.Floor;
    public string buildModeforObjectType;
    public bool BuildModeIsObjects = false;
    
   

    void Start()
    {
       
    }
    
    public void SetMode_BuildFloor()
        {
            BuildModeIsObjects = false;
            buildModeTile = Tile.TileType.Floor;
        }

    public void SetMode_DestroyFloor()
        {
            BuildModeIsObjects = false;
            buildModeTile = Tile.TileType.Empty;
        }

    public void SetMode_BuildInstalledObject(string ObjectType)
        {
            BuildModeIsObjects = true;
            buildModeforObjectType = ObjectType; // Thois is the string that is passed as a key to the prototyoe dictionary in the world class
        }

    public void DoBuild (Tile t) // Responsible for the building of both the Installed Objects and the Floor Tile Objects // Called from the loop in the mouse controller
    {
        string jobtype = buildModeforObjectType;
        if(BuildModeIsObjects == true)
            {
                string InstalledObjectType = buildModeforObjectType;
                if((WorldController.Instance.world.IsInstalledObjectValid(InstalledObjectType, t) == true && t.JobPendingInstalledObject == null))
                    {
                        Job j = new Job(t, jobtype, (thejob) => { WorldController.Instance.world.PlaceInstalledObject(InstalledObjectType, t); });
                        WorldController.Instance.world.jobQueue.Enqueue(j);

                        t.JobPendingInstalledObject = null;
                        j.RegisterJobCancelledCallBack( (thejob) => {thejob.tile.JobPendingInstalledObject = null;});
                       
                        t.JobPendingInstalledObject = j;
                    }
            }
        else
            {   
                    // Debug.Log("Changing Tile Type to " + buildModeTile);
                    t.Type = buildModeTile;
            }

    }

    public void BuildPathFindingTest()
    {
        WorldController.Instance.world.GeneratePathfindingTestMap();
    }
        
}

