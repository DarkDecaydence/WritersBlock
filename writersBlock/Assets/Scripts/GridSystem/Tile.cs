using UnityEngine;
using System.Collections;

public class Tile {

    public Vec2i pos;

    bool isWalkable = true;
    bool isExit = false;

    public Tile(Vec2i pos, bool walkAble)
    {
        this.pos = pos;
        this.isWalkable = walkAble;
    }

    public bool isWalkAble()
    {
        return isWalkable;
    }

    public bool isTileExit()
    {
        return isExit;
    }

    public void setExit(bool b)
    {
        isExit = b;
    }

    public void setWalkability(bool b)
    {
        isWalkable = b;
    }

}
