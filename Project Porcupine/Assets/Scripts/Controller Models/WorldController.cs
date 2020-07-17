using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldController : MonoBehaviour
{
    /// This is the first and lowest lying classes of the Controllers
    /// It is used to instantiate a copy of the world
    /// To position the main scene camera correctly 
    /// And to define a very important function whih is used in other controllers namely the GetTileAtWorldCoord()
    public static WorldController Instance {get; protected set;}
    public int width = 100;
    public int height = 100;
    public World world {get; protected set;}
    void OnEnable()
    {
        // Create a new instance of the world, which in turn creates the array of tiles
        world = new World(width, height );
        Instance = this;

        Camera.main.transform.position = new Vector3 (world.Width / 2 , world.Height / 2 , Camera.main.transform.position.z);
    }

    void Update() 
    {
        // TODO: Add pause, unpause and simulaton speed conditions to this update
        // This is the standard unity update function which runs per rendered frame (Derived from monobehaviour)
        world.Update(Time.deltaTime); // Calls the update on the world which in turn calls the update functions that is defined in the characters class
        
    }
    public Tile GetTileAtWorldCo_ord(Vector3 coord)                                                  
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);
        return world.GetTileAt(x, y);
    }

    

    
}
