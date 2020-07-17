
/// 
///
///     Conceptualised, created and developed by Cade Sayner 
///     Developer Contact: 031 916 7898
///     For CS Studios pty limited ALL RIGHTS RESERVED      ///
///     16 San Raphael ave Kingsburgh 4126 Amanzimtoti, Durban, South Africa
///
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// The tile clsss is the simplest, base on top of which the entire program is built on
// All other classes in some way interact or use the tile data class

public class Tile 
{
    public enum TileType { Empty, Floor,}  
   
    TileType type;
    
    public TileType Type 
    {
        get
        {
            return type;
        }

        set 
        {
            type = value;
           
            world.OnTileChanged(this);
            
        }

            
    }
    LooseObject looseObject;
    InstalledObject installedObject;

    public InstalledObject Installedobject
    {
        get
        {
            return installedObject;

        }
        protected set
        {

        }
        
        
    }
    World world;
    
    int x;
    public int X
    {
        get 
        {
            return x;
        }
    }
    int y;
    public int Y
    {
        get 
        {
            return y;
        }
    }

    public float MovementCost 
    {
        get
        {
            if (Type == TileType.Empty)
            {
                return 0f; // if tile is empty then movement cost is 0 and the tile is unwalkable
            }

            if (installedObject == null)
            {
                return 1f; // if there is no installed objects on a tile then return the default value of 1
            }

            return 1 * installedObject.movementCost; // if the tile does have an installed object situated on it then return 1 multiplied by the movement cost multiplier on the installedObject 
        }
    }

    public Job JobPendingInstalledObject;
   
    public Tile( World world, int x, int y )
    {
        this.world = world;
        this.x = x;
        this.y = y;
    }

    public void SetInstalledObject(InstalledObject obj)
    {
        if (installedObject == null)
        {
            installedObject = obj;
        }

        else
        {
            return;
        }
    }

   

    // Tells us if two tiles are adjacent to one another
    // Includes those which are at diagonals to one another (diagOkay boolean)
    public bool IsNeighbour(Tile tile, bool diagOkay = false)   // Really cool advanced function to be remembered and used in future projects 
    {
        // Checks to see if we a difference of exacle one between the tile co - ordinates: 
        return Mathf.Abs( this.X - tile.X) + Mathf.Abs( this.Y - tile.Y) == 1 // Check horixontal / vertical adjacency
        || (diagOkay && (Mathf.Abs( this.X - tile.X) == 1 && Mathf.Abs(this.Y - tile.Y) == 1));
        // If we allow it to be diagonally okay we return true if the difference on the X axis and the difference on the y values is equal to exactly one ( Abs function forces it to be positive )
    }

    public Tile[] getNeighbours(bool diagOkay = false) // Returns an array of tiles that contains the neighbours for each intsnace of a tile
    {
        Tile[] ns;      // Note: Probably a more advanced function tht can be implemented down the road

        if (diagOkay == false)
        {
            ns = new Tile[4]; // Tile order = N E S W (Clockwise)

        }

        else
        {
            ns = new Tile[8]; // Tile order = N, E, S, W, NE, SE, SW, NW (Clockwise)
        }

        Tile n; // Gets overwritten as it is not a global variable
        n = world.GetTileAt(X, Y + 1); // Above
        ns[0] = n; // Could be null but thats okay
        n = world.GetTileAt(X + 1 , Y); // Above
        ns[1] = n; // Could be null but thats okay
        n = world.GetTileAt(X, Y - 1); // Above
        ns[2] = n; // Could be null but thats okay
        n = world.GetTileAt(X - 1, Y); // Above
        ns[3] = n; // Could be null but thats okay

        if (diagOkay == true)
        {
        n = world.GetTileAt(X + 1, Y + 1); // Above
        ns[4] = n; // Could be null but thats okay
        n = world.GetTileAt(X + 1 , Y - 1); // Above
        ns[5] = n; // Could be null but thats okay
        n = world.GetTileAt(X - 1, Y - 1); // Above
        ns[6] = n; // Could be null but thats okay
        n = world.GetTileAt(X - 1, Y + 1); // Above
        ns[7] = n; // Could be null but thats okay
        }
        
        return ns;
    }


}
