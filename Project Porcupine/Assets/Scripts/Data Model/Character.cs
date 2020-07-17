
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character
// The class that governs and controls all game logic which is handled ny the characters
{

    /// These two public getters will be accesed by the visual system in order to accurately display the players movement across the screen
    public float X
    {
        get { return Mathf.Lerp(currentTile.X, destinationTile.X, Movementpercentge ); }
        // Mathf.Lerp() Interpolates between two floats by a given float
    }

     public float Y
    {
        get { return Mathf.Lerp(currentTile.Y, destinationTile.Y, Movementpercentge ); }
        
    }
    public Tile currentTile
    { 
        get; protected set;
    }
    Tile destinationTile;

     // A value that goes from zero to one as we move from current tile to destination tile
    float speed = 2f; // Tiles per second 

    float Movementpercentge;

    Action<Character> cbCharacterChanged;

    Job MyJob; // The job fetched from the front of the queue

    // Public contructor:
    public Character(Tile tile) 
    {
        this.currentTile = this.destinationTile = tile;
    }

    public void Update (float deltaTime) // Not to be confused with UnityEngine.Monobehaviour.Update() // Assists with handling multi threading by having seperate game logic updates, physics updates and render updates
    {   
        //Debug.Log( " Character Updated Succesfully " );
        // TODO: There needs to be simulation speed control UI
        
        // Do I have a job??
        if(MyJob == null)
        { 
            // If not then I grab a new one from the front of the queue
            MyJob = WorldController.Instance.world.jobQueue.DeQueue();

            // This is where the power of the actions being able t register and call multiple functions at he same time comes in very handy 
            MyJob.RegisterJobCancelledCallBack(OnJobComplete);
            MyJob.RegisterJobCompletedCallback(OnJobComplete);
            
        }

        if (MyJob != null)
        {
            // If I have a job then is set my destinaion tile to the tile on which that job is situated
            destinationTile = MyJob.tile;
        }

        if (currentTile == destinationTile) // Are we there yet?
        {
            if (MyJob != null)
            {   
                // If I have reached my job and the I still have a job then call the DoWork() function on the Job which will then callback to the JobSpriteCOntroller and the Temporary sprite will be removed 
                MyJob.DoWork(deltaTime);
            }
            // Debug.Log("destinationTile Tile is equal to the current tile");
            return; // Exit the function
        }

        // Whats the total distance from starting point to destination
        float dist = Mathf.Sqrt(Mathf.Pow((currentTile.X - destinationTile.X), 2) + Mathf.Pow((currentTile.Y -  destinationTile.Y), 2)); // Total Distance that needs to be travelled

        // How much can we move this frame 
        float DistanceThisFrame = speed * deltaTime;;

        // How much is that in terms of a percentage to our destination
        float percThisFrame = DistanceThisFrame / dist;       

        // Add that to overall percentage traveled percentage 
        Movementpercentge += percThisFrame;     

        if (Movementpercentge >= 1)
        {
            // We have reached our destination 
            /// FIX ME? Do we want to keep track of any overshot distances
            currentTile = destinationTile;
            Movementpercentge = 0f;
        }

        if (cbCharacterChanged != null)
        {   
            // Debug.Log("KOKKOOO");
            cbCharacterChanged(this);
        }
           

    
    }

    //  public void SetDestination(Tile tile) //  Comes when Pathfinding comes into effect later // Remove !!!
    //  {
    //  if(currentTile.IsNeighbour(tile, true) == false) // Diagonal movement is acceptable for the character
    //  {
    //  Debug.Log("Our destination tile isnt a neighbour to the passed tile");
    //  }

    //  else
    //  {
    //  Debug.Log("Destination succesfully set");
    //  destinationTile = tile;
    //
    //  }

    public void RegisterCharacterOnChangedCallback(Action<Character> cb)  
    {
        cbCharacterChanged += cb;
    }

     public void UnregisterCharacterOnChangedCallback(Action<Character> cb)  
    {
        cbCharacterChanged -= cb;
    }

    void OnJobComplete(Job j)
    {
        // Refers to the Character only!!!!!
        // Job completed or was cancelled 
        if(j != MyJob)
        {
            Debug.LogError ("Chatracter being told about a job that isnt his, You forgot to unregister something");
            return;
        }

        MyJob = null; // Get rid of the Job for garbage collection by C# compiler
    }
}
