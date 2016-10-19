using UnityEngine;
using System.Collections;

public class GamePiece : MonoBehaviour {

    protected Vec2i pos;

    public Vec2i Pos { get { return pos; } }

    protected void updateDataPosition(Vec2i newPos)
    {

        GameData.grid.getTile(pos).setGamePiece(null);
        GameData.grid.getTile(newPos).setGamePiece(this);
        pos = newPos;

    }
}
