using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



// Installed objects are permanent, fixed objects that are unchanging (eg. Walls and Shelves)


public class InstalledObject
{
    
    public Tile tile{   // Remember to always have infomation on hand at all times in all classes to make functions easier and more intuitive
        get;
        protected set;
    }   // Represents base tile of object, in practice however, objects may take up additional adjacent tiles
   public string objectType {
        get; 
        protected set;
    }


    // This is a multiplier,  a value of 2 here, means the players speed will be half the default
    // Tile types and other external environmental factors may further act as multipliers
    // for example a rough tile (cost of 2), a tile which is on fire (cost of 3)
    // The player will therefore have a default speed value of (2 + 2 + 3 = 7) 
    // SPECIAL // If movement cost = 0, the obstacle is impassable, ie. A wall
    public float movementCost {get; protected set;} 

    // Sofa may be 3 x 3 tiles (even though the graphics might show it as 3 x 2, as extra space could be for leg room)
    int width;
    int height;


    Action <InstalledObject> cbOnChanged;
    Func<Tile, bool> FuncPositionValidation;
    
    protected InstalledObject ()
        {
            // This replaces the automatically generated dummy constructor that is automatically usually generated
        }   

    static public InstalledObject CreatePrototype(string objectType, float movementCost,int width ,int height )
        {
            // Purely data driven function
            // Basically a contructor but in the form of a static function, A prototype is made and then parsed to the PlaceInstance function
            InstalledObject obj = new InstalledObject();
            obj.objectType = objectType;
            obj.movementCost = movementCost;
            obj.width = width;
            obj.height = height;

            obj.FuncPositionValidation =  obj.__IsValidPosition;


            return obj;


        }

    static public InstalledObject PlaceInstance ( InstalledObject proto, Tile tile)
        {                                   
            if(proto.FuncPositionValidation(tile) == false)
                {
                    Debug.LogError("Place Instance position validity function returned false");
                    return null; 
                }

            InstalledObject obj = new InstalledObject(); 
            obj.objectType = proto.objectType;
            obj.movementCost = proto.movementCost;
            // Debug.Log(obj.movementCost);
            obj.width = proto.width;
            obj.height = proto.height;

            obj.tile = tile;
            tile.SetInstalledObject(obj);
            return obj;
        }

    public void RegisterOnChangedCallback(Action<InstalledObject> callbackfunc)
        {
            cbOnChanged += callbackfunc;
        }

     public void UnregisterOnChangedCallback(Action<InstalledObject> callbackfunc)
        {   
            cbOnChanged -= callbackfunc;
        }

    public bool IsValidPosition(Tile t)
        {
            return FuncPositionValidation(t);
        }

    bool __IsValidPosition(Tile tile)
        {
        
            if (tile.Type != Tile.TileType.Floor)
            {
                return false;
            }

            
            if (tile.Installedobject != null)
            {
                return false;
            }

            return true;
        }

    bool __IsValidPosition_Door(Tile tile)
        {
            return true;
        }
}
