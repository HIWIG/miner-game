using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileAtlas", menuName = "Tile Atlas")]
public class TileAtlas : ScriptableObject
{
    public TileClass dirt;
    public TileClass grass;
    public TileClass stone;
    public TileClass grain;
    public TileClass log;
    public TileClass leaf;

    public TileClass coal;
    public TileClass iron;
    public TileClass diamond;

    public TileClass cabin;
    public TileClass cabinWalls;
    public TileClass cabinInside;
    public TileClass cabinWallsInside;
    public TileClass cabinWindow;

    public TileClass backgroundStone;
    public TileClass Ladder;
}
