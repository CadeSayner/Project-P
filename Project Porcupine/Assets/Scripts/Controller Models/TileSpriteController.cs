using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileSpriteController : MonoBehaviour

/// SUMMARY ///

// This class handles the rendering of sprites when it is applicable to tiles (ie. Floor/ Empty)
// On the Start() method, game objects are created for every tile in the world that has been created by the WorldController() 
// For each tile a GameObject is created and assigned a SpriteRenderer component
// The OnTileType change callback function is also defined in this class and is registered to every Tile which is given a SpriteRenderer component

{
    public Sprite floor_sprite;
    public Sprite Empty_Sprite;
    Dictionary<Tile, GameObject> tileGameObjectMap;
    World world 
    {
        get {return WorldController.Instance.world;}
    }

    void Start()
    {
        for (int x = 0; x < world.Width; x++)
        {
            for (int y = 0; y < world.Height; y++)
            {
                Tile tiledata = world.GetTileAt(x, y);
                GameObject tile_go = new GameObject();

                
                tile_go.name = "Tile_" + x + "_" + y;

                tile_go.transform.position = new Vector3(tiledata.X, tiledata.Y, 0);
                tile_go.transform.SetParent(this.transform, true);
                SpriteRenderer tile_sr = tile_go.AddComponent<SpriteRenderer>();

                
                
                tile_sr.sprite = Empty_Sprite;
                tile_sr.sortingLayerName = "Tile UI";

                world.RegisterTileTypeChangedCallBack((tile) => {OnTileTypeChanged(tiledata, tile_go);});
            }
        }
    }
        private void OnTileTypeChanged(Tile tile_data, GameObject tile_go)
        {

          
            if (tile_data.Type == Tile.TileType.Floor)
            {
               
                tile_go.GetComponent<SpriteRenderer>().sprite = floor_sprite;
            }
            
            else if (tile_data.Type == Tile.TileType.Empty)
            {
                
                tile_go.GetComponent<SpriteRenderer>().sprite = Empty_Sprite;
            }

            else
            {
                Debug.LogError("Unrecognised Tile Type");
            }
        }
}


    

    