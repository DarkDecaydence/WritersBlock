using UnityEngine;
using System.Collections;
using System;

public class Character : MonoBehaviour, IGamePiece
{
    public Vec2i pos;

    void Awake()
    {
        GameData.playerCharacter = this;
        setPostion(new Vec2i(1, 1));
    }

    public void setPostion(Vec2i newPos)
    {
        pos = newPos;
        updatePosition();
    }

    public bool move(Vec2i dist)
    {
        Tile next = GameData.grid.getTile(pos + dist);
        if (next == null)
            return false;

        if (!next.isWalkAble())
            return false;

        pos += dist;
        updatePosition();
        Debug.Log(pos);
        return true;
    }

    public void updatePosition()
    {
        transform.position = new Vector3(pos.x + TileMetrics.tileHalfLength, transform.position.y, pos.y + TileMetrics.tileHalfLength);
    }

    Vec2i IGamePiece.GetPosition()
    {
        return pos;
    }
}
