using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mouse_Controller : MonoBehaviour
{

    public GameObject Circle_Cursor_Prefab;
 
    public Vector3 LastFramePosition;
    public Vector3 Offset;
    public Vector3 DragStartPosition;
    public Vector3 curr_frame_position; 

    
    List<GameObject> Highlight_Prefabs;

    void Start()
    {
        Highlight_Prefabs = new List<GameObject>();
    }
    void Update()
    {
      
        curr_frame_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        curr_frame_position = curr_frame_position +  Offset; 
        DraggingFunction();
        HandleCameraMovement();
    }
    void DraggingFunction()
    {
       if (Input.GetMouseButtonDown(1))
            {
                DragStartPosition = curr_frame_position;
            }

        int start_x = Mathf.FloorToInt(DragStartPosition.x);
        int end_x = Mathf.FloorToInt(curr_frame_position.x);
        int start_y = Mathf.FloorToInt(DragStartPosition.y);
        int end_y = Mathf.FloorToInt(curr_frame_position.y);

            
            if (start_x > end_x)
            {
                int tmp = end_x;
                end_x = start_x;
                start_x = tmp;
            }


            if (start_y > end_y)
            {
                int tmp = end_y;
                end_y = start_y;
                start_y = tmp;
            }

       
        while(Highlight_Prefabs.Count > 0)
            {
                GameObject go = Highlight_Prefabs[0];
                Highlight_Prefabs.RemoveAt(0);
                SimplePool.Despawn(go);
            }


        if (Input.GetMouseButton(1))
            {  
                for (int x = start_x; x <= end_x; x++)
                    {
                        for (int y = start_y; y <= end_y; y++)
                            {
                                Tile t = WorldController.Instance.world.GetTileAt(x, y);
                               
                                if (t != null)
                                    {
                                        GameObject go = SimplePool.Spawn(Circle_Cursor_Prefab, new Vector3(x, y, 0), Quaternion.identity);
                                        Highlight_Prefabs.Add(go); 
                                        go.transform.SetParent(this.transform, true);
                                    }
                            }
                    }
            
        }

        if (Input.GetMouseButtonUp(1))
            {
                BuildModeController bmc = GameObject.FindObjectOfType<BuildModeController>();
                for (int x = start_x; x <= end_x; x++)
                    {
                        for (int y = start_y; y <= end_y; y++)
                            {
                                Tile t = WorldController.Instance.world.GetTileAt(x, y);
                                if (t != null)
                                    {
                                        // Call BuildModeModeController. DoWork()
                                        // Tiles are passed to the DoBuild Function which starts the building process of both floor tiles and installed objects
                                        bmc.DoBuild(t);
                                    }           
                            }
                    }
            }
    }

    void HandleCameraMovement()
    {
       if(Input.GetMouseButton(2) ) 
            {
                Vector3 Diff = LastFramePosition - curr_frame_position;
                Camera.main.transform.Translate(Diff);
            }

        LastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        curr_frame_position = curr_frame_position + Offset;

        Camera.main.orthographicSize -= Camera.main.orthographicSize *  Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 10f);
    }
}

