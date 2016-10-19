using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum MonsterElement {  None, Arcane, Earth, Water, Fire, Air, Void }

public class Monster : GamePiece {

    Vec2i prevPos;
    Vec2i actualPos;
    Vec2i targetPos;

    List<Vec2i> path;
    int pathIndex;

    //Timer for updating the path finding
    float pathUpdateTimer = 3f;
    float timer = 0f;
    float walkTimer = 0f;

    //Movement speed and timer
    float movementTime = 1.5f;

    //Attack speed
    float attackTime = 2f;
    float attackValue = 10f;

    public State state;
    public enum State { Patroling, idle, move, attack}

    public MonsterElement Element;

    public float aggroRange = 7f;

	// Use this for initialization
	public void Init (Vec2i pos)
    {
        updateDataPosition(pos);
        setObjectPosition(pos);

        state = State.Patroling;

    }

    // Update is called once per frame
    void Update ()
    {

        FSM();

	}

    public void FSM()
    {
        switch (state)
        {
            case State.Patroling:
                break;
            case State.idle:
                idleBehavior();
                break;
            case State.move:
                moveBehavior();
                break;
            case State.attack:
                attackBehavior();
                break;
        }
        
    }

    void idleBehavior()
    {
        delayPathUpdateCheck();
        if (path != null)
        {
            state = State.move;      
        }
    }

    void moveBehavior()
    {

        if (path == null)
            return;

        walkTimer += Time.deltaTime;
        float t = walkTimer / movementTime;
        setObjectPosition(Vec2i.Lerp(prevPos, pos, t, transform.position.y));

        if (t >= 1)
        {
            // Change location of audio play to counter repetition and desync.
            GameData.audioManager.PlayMonster("MonsterWalkCycle");
            actualPos = pos;
            GetNextPosInPath();
            delayPathUpdateCheck();
            walkTimer = 0;
        }
    }

    void updatePath(Vec2i from)
    {

        if (targetPos.Equals(GameData.playerCharacter.Pos))
            return;

        targetPos = GameData.playerCharacter.Pos;
        path = GameData.aStar.FindShortestPath(pos, targetPos);

        if (path == null)
            return;

        pathIndex = 0;
        GetNextPosInPath();
    }

    void GetNextPosInPath()
    {

        prevPos = pos;
        Vec2i nextPosTemp = pos + path[pathIndex];

        if (checkForPathEnd())
            return;

        if (checkIfTileIsOccupied(nextPosTemp))
            return;

        updateDataPosition(nextPosTemp);
        walkTimer = 0;
        pathIndex++;
    }

    bool checkIfTileIsOccupied(Vec2i v)
    {
        if (GameData.grid.getTile(v).isTileOccupied())
        {
            state = State.idle;
            path = null;
            return true;
        }
        return false;
    }
    
    bool checkForPathEnd()
    {
        //If monster has reached the end of its path
        if (path.Count - 1 <= pathIndex)
        {
            //If monster is next to its target switch to attack state
            Debug.Log("NExt to trget? : " + nextToTarget());
            if (nextToTarget())
            {
                Debug.Log("ok");
                state = State.attack;
                path = null;
            }
            //Else update the path to reach the player
            else
            {
                updatePath(pos);
            }
            return true;
        }
        return false;
    }

    void delayPathUpdateCheck()
    {
        timer += Time.deltaTime;
        if (timer > pathUpdateTimer && !nextToTarget())
        {
            updatePath(pos);
            timer = 0;
        }
    }

    public void aggro()
    {
        Debug.Log("I just got aggored warrrrhh");
        GameData.audioManager.PlayMonster("MonsterLaugh");
        state = State.move;
        updatePath(pos);
    }

    void attackBehavior()
    {
        if (!nextToTarget())
        {
            Debug.Log("IM no longer next to character");
            state = State.move;
            updatePath(pos);
            return;
        }

        walkTimer += Time.deltaTime;
        if (walkTimer > attackTime)
        {
            attack();
            walkTimer = 0;
        }
    }

    void attack()
    {
        GameData.playerCharacter.gameObject.GetComponent<HealthScript>().addHealth(-attackValue);
    }

    bool nextToTarget()
    {
        return (Mathf.Abs(GameData.playerCharacter.Pos.x - actualPos.x) + Mathf.Abs(GameData.playerCharacter.Pos.y - actualPos.y)) == 1 
            || GameData.playerCharacter.Pos.Equals(actualPos);
    }

    void setObjectPosition(Vec2i newPos)
    {
        transform.position = new Vector3(newPos.x + TileMetrics.tileHalfLength, transform.position.y, newPos.y + TileMetrics.tileHalfLength);
    }

    void setObjectPosition(Vector3 newPos)
    {
        transform.position = new Vector3(newPos.x + TileMetrics.tileHalfLength, transform.position.y, newPos.z + TileMetrics.tileHalfLength);
    }

    public Vec2i GetPosition()
    {
        return pos;
    }
}
