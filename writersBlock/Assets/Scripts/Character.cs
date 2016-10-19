using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    public Vec2i pos;

    void Awake()
    {
        GameData.playerCharacter = this;
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

        Debug.Log(next.isWalkAble() + " "+  next.ToString());
        if (next.isTileExit())
        {
            winLevel();
            return true;
        }

        GameData.monsterGenerator.MonsterAggroCheck();

        pos += dist;
        updatePosition();
        Debug.Log(pos);
        return true;
    }

    public void updatePosition()
    {
        transform.position = new Vector3(pos.x + TileMetrics.tileHalfLength, transform.position.y, pos.y + TileMetrics.tileHalfLength);
    }

    void winLevel()
    {
        GameData.levelManager.advanceLevel();
    }
}
