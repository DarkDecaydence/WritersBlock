using UnityEngine;
using System.Collections;

public class Tile {

    public Vec2i pos;
    bool walkAble = true;

    public Tile(Vec2i pos, bool walkAble)
    {
        this.pos = pos;
        this.walkAble = walkAble;
    }

    public bool isWalkAble()
    {
        return walkAble;
    }

}
