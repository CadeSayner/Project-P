using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class World 

/// This class consists of an array of Tile objects, and contains definitions for a variety of methods to connect the controller scripts to the data
/// oriented Tile class
/// The job queue is present in the world class
/// The installed object prototype dictionary is defined and acessed through the world
/// The callback to the OnInstalledObjectCreated() is found here as well as the definition sof the functions that are used to register and unregister this callback
/// The call back is defined here so that it can be called when the InstalledObject is placed and checks all of the logic gates before instantiation


{
   
    Tile[,] tiles;

    List <Character> characters; 


    // The pathfinding graph used to navigate the world map
    Tile_Graph tile_Graph;
    public JobQueue jobQueue; // INstead of a direct link to a generic Queue, the world has a reference to nthe JobQueue Class which returns the queue is needed



    Dictionary<string, InstalledObject> InstalledObjectPrototypes;
    int width;
    
    public int Width
    {
        get 
        {
            return width;
        }
    }

    int height;

    public int Height
    {
        get
        {
            return height;
        }
    }
    Action<Tile> cbTileTypeChanged;
    Action<InstalledObject> cbInstalledObjectCreated;
    Action<Character> cbCharacterCreated;

    public World(int width, int height) 
    {
        this.width = width;
        this.height = height;

        jobQueue = new JobQueue();

        tiles = new Tile[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles [x,y] = new Tile(this, x, y);
                
            }
        
        }

        Debug.Log("World Created with " + width * height + " Tiles");
        CreateInstalledObjectPrototypesDictionary();

        characters = new List <Character>(); // Instantiate a new list of type Character

    }

    // Definition for function that calls update on all characters 
    public void Update (float deltaTime)
    {
        foreach(Character character in characters)
        {
            character.Update(deltaTime);
        }
    }
    public Character CreateCharacter(Tile t) 
    {   
        // Characters belong to the world and are intantiated as soon as the world controller creates a new instance of this class
        Character c = new Character(tiles[width / 2, height / 2]); // Send the character class information about the Tile it should be spawned on
        if (cbCharacterCreated != null)
        {   
                characters.Add(c);
                cbCharacterCreated(c);
                
        }
        return c;
        
    }

    
    void CreateInstalledObjectPrototypesDictionary()
    {
        InstalledObjectPrototypes = new Dictionary<string, InstalledObject>();

        InstalledObject WallPrototype = InstalledObject.CreatePrototype(
        "Wall", 
         0, 
         1,
         1);
        InstalledObjectPrototypes.Add("Wall", WallPrototype);
    }
    public Tile GetTileAt(int x, int y)
    {
        if (x > width || x < 0 || y > height || y < 0) 
        {
            Debug.LogError ("Error!! Tile ("+x+", "+y+") is out of range");
            return null;
        }
        return tiles[x, y];
    }
    public void PlaceInstalledObject(string ObjectType, Tile t)
    {
        // Debug.Log("Place Installed Object running succesfully");
 
        if (InstalledObjectPrototypes.ContainsKey(ObjectType) == false)
            {
                Debug.LogError("The prototype dictionary does not contain a definition for the passed string");
                return;
            }

        InstalledObject obj = InstalledObject.PlaceInstance( InstalledObjectPrototypes [ObjectType] , t ); 
        if (obj == null)                                 
            {
                return; 
            }
       
        if(cbInstalledObjectCreated != null)
            {
                cbInstalledObjectCreated(obj);
            }
    }

    public void OnTileChanged(Tile t) // Intermediatory function that is the "bridge" between the TileSpriteController and the Tile data itself
    {   // Gets called whenever any tile gets changed
        if(cbTileTypeChanged != null)
            {
                cbTileTypeChanged(t); // Calls the OnTileTypeChanged() function in the TileSpriteController class
            }
    }

    // Should be called whenever the world is changed
    // because if the world changes then the graph we have of the world is invalid and needs to be destroyed
    public void InvalidateTileGraph()
    {
        tile_Graph  = null;
    }


    public void RegisterTileTypeChangedCallBack(Action<Tile> callback)
    { 
        cbTileTypeChanged += callback;
    }

    public void UnRegisterTileTypeChangedCallBack(Action<Tile> callback)
    { 
        cbTileTypeChanged -= callback;
    }

   
   
    public void RegisterInstalledObjectCreated(Action<InstalledObject> callb)
    {
        cbInstalledObjectCreated += callb;
    }

    public void UnregisterInstalledObjectCreated(Action<InstalledObject> callb)
    {
        cbInstalledObjectCreated -= callb;
    }

    public void RegisterCharacterCreated(Action<Character> callb)
    {
        cbCharacterCreated += callb;
    }

    public void UnregisterCharacterCreated(Action<Character> callb)
    {
        cbCharacterCreated -= callb;
    }


    public bool IsInstalledObjectValid (string InstalledObjectType, Tile t) 
    {
       return InstalledObjectPrototypes[InstalledObjectType].IsValidPosition(t);
    }
    public void GeneratePathfindingTestMap()
    {
        int l = width / 2 - 5; // 5 left of the centre x - value 
        int b = height / 2 - 5; // 5 down from the centre y - value 

        for (int x = l - 5; x < l + 15; x++) // loop runs from 10 left of the centre x - value to 10 right of the centre x - value
        {
           for (int y = b - 5; y < b + 15; y++) // loop runs from 10 down from the centre y - value to 10 up from the centre y - value
           {
               Tile tile = GetTileAt(x, y); // Returns the tile that are in this range
               tile.Type = Tile.TileType.Floor; // Changes their types to floor


                if (x == l || x == (l + 9) || y == b || y ==(b + 9)) // if x is equal to 1 or x is equal to 4 right of the centre value or y is equal to 45 or y is equal to 4 abaove the centre y - value 
                {
                    if(x!= (l + 9) && y != (b + 4) && x != (l - 4)) // if x is not equal to 4 right of the centre tile and y is not equal to 1 unit left of the centre tile 
                    {                               // Check that one of these excluded tiles ignored by the loop
                        PlaceInstalledObject("Wall", tile); // Create an object at that specifi co - ordinate
                    }
                }
            } 
        }
    }
}


