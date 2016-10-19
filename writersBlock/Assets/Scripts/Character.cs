using UnityEngine;
using System.Collections;

public class Character : GamePiece
{

    void Awake()
    {
        GameData.playerCharacter = this;
    }

    public void setPostion(Vec2i newPos)
    {
        updateDataPosition(newPos);
        updatePosition();
    }

    public bool move(Vec2i dist)
    {
        Tile next = GameData.grid.getTile(pos + dist);
        if (next == null || !next.isWalkAble() || next.isTileOccupied()) { 
            GameData.audioManager.PlayPlayer("PlayerBlocked");
            return false;
        }

        if (next.isTileExit())
        {
            GameData.audioManager.PlayVictory();
            winLevel();
            return true;
        }

        // Lerp player position while playing audio.
        // Player movement audio is 0.5s length.
        GameData.audioManager.PlayPlayer("PlayerWalkCycle");
        updateDataPosition(pos + dist);
        updatePosition();
        
        //Check for monster aggro
        GameData.monsterGenerator.MonsterAggroCheck();

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
