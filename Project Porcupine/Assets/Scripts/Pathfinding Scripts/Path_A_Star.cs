using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_A_Star
{
    Queue<Tile> path;


	// public constructor
	public Path_A_star (World world, Tile start, Tile end) // Takes in the world, a starting tile and an ending tile
	{
		// Generates the path
	}

	public Tile GetNextTile()
	{
		// Called by whoever needs the path finding system
		// What is the next tile I should move to? 
		// The path finding system will fetch that tile from the queue

		return path.DeQueue();

	}

}
