using UnityEngine;
using System.Collections;
using System;

public class Character : MonoBehaviour
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
        if (next == null || !next.isWalkAble()) { 
            GameData.audioManager.PlayPlayer("PlayerBlocked");
            return false;
        }

        pos += dist;
        // Lerp player position while playing audio.
        // Player movement audio is 0.5s length.
        GameData.audioManager.PlayPlayer("PlayerWalk");
        updatePosition();
        Debug.Log(pos);
        return true;
    }

    public void updatePosition()
    {
        transform.position = new Vector3(pos.x + TileMetrics.tileHalfLength, transform.position.y, pos.y + TileMetrics.tileHalfLength);
    }
}
