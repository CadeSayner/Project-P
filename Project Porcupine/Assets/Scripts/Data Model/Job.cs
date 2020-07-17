using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Job 
{
    public Tile tile{
        get; protected set;
    }

    public string JobType 
    {
        get; protected set;
    }
    float job_time = 1f;

    Action <Job> cbJobComplete;
    Action <Job> cbJobCancel;

    public Job(Tile tile, string JobObjType, Action<Job> cbJobComplete, float Jobtime = 1f)
    {
        this.tile = tile;
        this.cbJobComplete = cbJobComplete;
        this.JobType = JobObjType;
    }

    public void RegisterJobCompletedCallback(Action<Job> cb)
    {
        this.cbJobComplete += cb;
    }
    public void RegisterJobCancelledCallBack(Action<Job> cb)
    {
        this.cbJobCancel += cb;
    }


    public void DoWork(float jobtime)
    // This will work now since it is called by an update function 
    {
        // This function is called every frame that we have a job and have reached the tile at which the job is situated
        job_time -= jobtime;
        if (job_time <= 0)
        {
            if (cbJobComplete != null)
            {
                cbJobComplete(this);
            }
        }
    }        
    
    
    public void CancelJob()
    {
        if(cbJobCancel != null)
        {
            cbJobCancel(this);
        }
    }
}

