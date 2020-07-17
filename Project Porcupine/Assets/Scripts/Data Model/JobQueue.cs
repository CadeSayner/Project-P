using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JobQueue
// This class defines the entire JobQueue, and instance of which is created in the BuildModeController which in turn is called on 
// By the Mouse Controller
// When a Job is added to the queue a callback function is triggered 
{
    Action<Job> cbJobCreated;

    // Wrapper for the jobqueue allows segregation of data models
     Queue<Job> jobqueue; 
     
     // Constructor for JobQueue
     public JobQueue()
     {
         // The constructor by default returns a Queue
         this.jobqueue = new Queue<Job>();
     }

     public void Enqueue(Job j)
     {
         jobqueue.Enqueue(j); // It firstly actually enqueues the Job befire it calls any callbacks 
         // TODO: Call Backs here !!
         // Queue up the jobs and if there is any call backs then they will be run

         if (cbJobCreated != null)
            {
                cbJobCreated(j);
            }
     }

     public Job DeQueue()
     {
        if(jobqueue.Count == 0)
        {
            return null;
        }
        return jobqueue.Dequeue(); // Fetches the job at the front of the queue
     }
     public void RegisterJobCreationCallback(Action<Job> cb)
     {
         cbJobCreated += cb;
     }

     public void UnRegisterJobCreationCallback(Action<Job> cb)
     {
         cbJobCreated -= cb;
     }

    
}
