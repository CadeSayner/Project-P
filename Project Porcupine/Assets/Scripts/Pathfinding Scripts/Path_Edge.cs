using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_Edge<T>
{
    public float cost; // Cost to traverse this edge (ie. Cost to enter this a tile)
    public Path_Node<T> path_Node; // points to a path_node

}
