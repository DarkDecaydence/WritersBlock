using UnityEngine;
using System.Collections;

public class Tile {

    public Vec2i pos;

    bool isWalkable = true;
    bool isExit = false;

    GamePiece occupant = null;

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

    public void setGamePiece(GamePiece p)
    {
        occupant = p;
    }

    public bool isTileOccupied()
    {
        if (occupant == null)
            return false;
        else
            return true;
    }

    public GamePiece getTileOccupant()
    {
        return occupant;
    }

}
