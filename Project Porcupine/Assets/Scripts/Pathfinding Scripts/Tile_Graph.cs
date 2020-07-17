using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Graph
{
    // Not generic!!!

    Dictionary <Tile, Path_Node<Tile>> nodes; // Maps from tiles to nodes
    
    // Public constructor
    // This class creates a tile compatible graph of the world, each tile is a graph, each walkable neighbour from a tile is linked via an edge
    public Tile_Graph(World world)
    {
        Debug.Log ("Tile Graph initialised");
        nodes = new Dictionary<Tile, Path_Node<Tile>>();
        // Loop through all tiles of the world, for each tile that is wallkable create a node, create a node
        for (int x = 0; x < world.Width; x++)
        {
            for (int y = 0; y < world.Height; y++)
            {
                Tile t = world.GetTileAt(x, y); // Fetch the tile at the looped x and y - values 
                if (t.MovementCost > 0) // if the tiles are walkable
                {
                    // Create a node
                    Path_Node<Tile> n = new Path_Node<Tile>();
                    n.data = t; 
                    nodes.Add(t, n); // Dictionary will now have a corresponding node for any tile in the world that is walkable
                }

                
            }
        }

        Debug.Log("Created" +  nodes.Count + "Nodes");


        int EdgeCount = 0;
        foreach (Tile t in nodes.Keys) // For each tile (Key) in the dictionary 
        {
            Path_Node<Tile> n = nodes[t]; // Get the corresponding node

            List<Path_Edge<Tile>> edges = new List<Path_Edge<Tile>>(); // List of edges that come out of a node
            // Get a list of neighbours
            Tile[] neighbours = t.getNeighbours(true); // Note: Some tiles in this array could be null

            // loop through the array of neighbouring tiles for each walkable tile 
            for (int i = 0; i < neighbours.Length; i++)
            {
                if(neighbours[i] != null && neighbours[i].MovementCost > 0)
                {
                    // The nighbouring tile exists and is walkable so create an edge for that neighbour tile 
                    Path_Edge<Tile> e = new Path_Edge<Tile>(); 
                    e.cost = neighbours[i].MovementCost; // feeds the neighbouring tile to the edge constructor
                    e.path_Node = nodes[neighbours[i]]; // the node fed to the path_edge is the neighbouring tile that is being processed
                    // Returns the node for the neighbouring tile ? // 
                    edges.Add(e); // Add the edge to temp and throwable list
                    EdgeCount ++;
                }
            }

            n.edges = edges.ToArray(); // Casts the List of Edges to an array that is stored in the node // so that the node is aware of how many edges it has
        }
        Debug.Log(EdgeCount + " Edges created");
    }


}
