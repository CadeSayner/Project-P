using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// TODO::: Fix ehat happens in the OnInstalledObjecCreated() function to accomodate other types of InstalledObjects
public class InstalledObjectSpriteController : MonoBehaviour

// Main purpose is to define the OnInstalledObjectCreated() functions and assign sprtes according to type and position in the world 
{

    public Sprite WallSprite; 


    Dictionary<InstalledObject, GameObject> installedObjectGameObjectMap;

    World world 
    {
        get {return WorldController.Instance.world;}
    }

    void Start()
    {
        installedObjectGameObjectMap = new Dictionary<InstalledObject, GameObject>();
        WorldController.Instance.world.RegisterInstalledObjectCreated(OnInstalledObjectCreated);
    }

    public void OnInstalledObjectCreated(InstalledObject obj)

    {
        // Debug.Log("OnInstalledObjectCreated()");
                // Conditions that require ObjectType definitions can be run here to differentiate the different furniture visually 
            
                GameObject obj_go = new GameObject();
                installedObjectGameObjectMap.Add(obj, obj_go);
                obj_go.transform.position = new Vector3(obj.tile.X, obj.tile.Y, -1);
                SpriteRenderer obj_sr = obj_go.AddComponent<SpriteRenderer>();
                obj_sr.sortingLayerName = "Installed Object Layer";
                obj_sr.sprite = WallSprite;
                obj.RegisterOnChangedCallback(OnInstalledObjectChanged);
    }

    void OnInstalledObjectChanged(InstalledObject obj)
    {
      Debug.LogError("OnInstalledObjectChanged not yet implemeneted");
    }
}
