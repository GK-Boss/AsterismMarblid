using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTile")]
public class Tile : ScriptableObject
{
    public TileBase tileBase;
    public float cost = 1;
    public bool canWalk = true;
    public Resource resource;
}
