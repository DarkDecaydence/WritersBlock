﻿using UnityEngine;
using System.Collections;

public class Character : GamePiece
{

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
        if (next == null || !next.isWalkAble()) { 
            GameData.audioManager.PlayPlayer("PlayerBlocked");
            return false;
        }

        Debug.Log(next.isWalkAble() + " "+  next.ToString());
        if (next.isTileExit())
        {
            GameData.audioManager.PlayVictory();
            winLevel();
            return true;
        }

        GameData.monsterGenerator.MonsterAggroCheck();

        pos += dist;
        // Lerp player position while playing audio.
        // Player movement audio is 0.5s length.
        GameData.audioManager.PlayPlayer("PlayerWalkCycle");
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
