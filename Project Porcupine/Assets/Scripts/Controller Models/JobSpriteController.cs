using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JobSpriteController : MonoBehaviour
{
    // This bare bones controller will piggy back on the back of the InstalledObjectSpriteController, as I have no idea how the job sprite
    // system is going to work in the future

    
    public Sprite TemporaryWallSprite;
    public Sprite CompletedWallSprite;

    Dictionary <Job, GameObject> JobGameObjectMap;
    

    InstalledObjectSpriteController iosc;
    void Start()
        {
            JobGameObjectMap = new Dictionary<Job, GameObject>();
            iosc = GameObject.FindObjectOfType<InstalledObjectSpriteController>(); // Initialise the reference to the Sprite Controller for the installed objects
            
            // This sprite controller on start tells the JobQueue that the OnJobCreation is what needs to be called whenever there is a new objetc added to the queue
            // This will happen every time the BUildModeController() Class tells it to
            // They are kept seperate to segregate data for organisation
            WorldController.Instance.world.jobQueue.RegisterJobCreationCallback(OnJobCreated); // Registers the callback in the JobQueue to thw OnJobCreate() function in this class
            
        }

    
    public void OnJobCreated(Job jb)
        {
            jb.RegisterJobCancelledCallBack(OnJobEnded);
            Debug.Log("Job Created succesfully");
            // Create a temporary sprite 
            GameObject job_go = new GameObject();
            JobGameObjectMap.Add(jb, job_go);
            job_go.transform.position = new Vector3 (jb.tile.X, jb.tile.Y, 0);
            SpriteRenderer job_sr = job_go.AddComponent<SpriteRenderer>();
            job_sr.sortingLayerName = "Installed Object Layer";

            // Check what type of job is being called, same varable sent to the prototype dictionary but im too lazy to 
            // Do that now 
            // TODO Implement the Dictionary system to map from a jobtype to a specific sprite
            if (jb.JobType == "Wall")
            {
                job_sr.sprite = TemporaryWallSprite;
            }
            else
            {
                Debug.LogError ("No Sprite being passed for " + jb.JobType);
            }

            

            // CallBack triggers whenever a job is created and added to the queue in the BuildModeController() class in the DoBuild() function
            // FIXME: We can only do furniture building rn!!
            // jb.DoWork(3f, jb); jb. DoWork() triggers the job complete function whichg can be used to render animations
            // for the job, while it is being run it can be used to display player animations and make things happen 
        }

    public void OnJobEnded(Job jb) // Called when the Job is finished // Should be added to the OnJobCompletedCallback list instead of being called directly
    {
        // Callback triggers whenever a job is completed or cancelled
        // Deletes temporary sprite and install permanent sprite
        Debug.Log("Job Complete callback triggered succesfully");
        if (JobGameObjectMap.ContainsKey(jb) == false)
        {
            Debug.Log("Cant find definition for Job Game Object Map");
            GameObject Job_go = JobGameObjectMap[jb];
            WorldController.Instance.world.PlaceInstalledObject("Wall",jb.tile); //Place he installed object at the position of the job
            Destroy(Job_go);

        }


        
        
    }


}
