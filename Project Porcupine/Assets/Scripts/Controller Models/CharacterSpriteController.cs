using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterSpriteController : MonoBehaviour
{
    Dictionary <Character, GameObject> characterGameObjectmap;

    Dictionary<string, Sprite> charactersprites;

    World world 
    {
        get {return WorldController.Instance.world;}
    }
    void Start()
    {
        LoadSprites();
        characterGameObjectmap = new Dictionary<Character, GameObject>();
      
        world.RegisterCharacterCreated(OnCharacterCreated);
        
         // passes the centre tile to the function in world which then uses it to tell the character what its current tile is
        Character c = world.CreateCharacter(world.GetTileAt(world.Width/ 2, world.Height/2));                                                       // Will be more important when it comes to character movement
        // c.SetDestination(world.GetTileAt( world.Width / 2 + 5, world.Height / 2)); 
        
    }

    void LoadSprites() 
    {
        charactersprites = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites"); 
        foreach(Sprite s in sprites)
        {
            Debug.Log(s);
            Debug.Log("Sprites Succesfully loaded");
            charactersprites[s.name] = s;  // therefore we only have to pass the string for the correct sprite to be assignes and rendered
        }

    }

    void OnCharacterCreated(Character character)
    {
        
        GameObject char_go = new GameObject();
        char_go.name = "Character";
        characterGameObjectmap.Add(character, char_go); // Adds a specific Character to a specific definition or accompanying game object in the Resources folder

        char_go.transform.position = new Vector3 (character.currentTile.X, character.currentTile.Y, 0);
    
        SpriteRenderer char_sr = char_go.AddComponent<SpriteRenderer>();
        char_sr.sortingLayerName = "Character Sorting Layer";
        char_sr.sprite = charactersprites["AlienSprite"];
        character.RegisterCharacterOnChangedCallback(OnCharacterChanged);
        
    }

    void OnCharacterChanged(Character c)
    {

        // Debug.Log("OnCharacterChanged");
        if (characterGameObjectmap.ContainsKey(c) == false)
        {
            Debug.Log("Map does not contain correct key");
            return;
            
        }

        GameObject char_go = characterGameObjectmap[c];
        char_go.transform.position = new Vector3 (c.X, c.Y, char_go.transform.position.z);

        // char_go.GetComponent<SpriteRenderer>().sprite 


    }
}

 


