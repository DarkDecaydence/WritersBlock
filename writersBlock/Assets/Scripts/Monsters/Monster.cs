using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum MonsterElement {  None, Arcane, Earth, Water, Fire, Air, Void }

public class Monster : GamePiece {

    Vec2i nextPos;
    Vec2i targetPos;

    List<Vec2i> path;
    int pathIndex;

    //Timer for updating the path finding
    float pathUpdateTimer = 3f;
    float timer = 0f;
    float timer2 = 0f;

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
        this.pos = pos;
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

        timer2 += Time.deltaTime;
        float t = timer2 / movementTime;
        setObjectPosition(Vec2i.Lerp(pos, nextPos, t, transform.position.y));

        if (t >= 1)
        {
            updateNextPathPos();
            delayPathUpdateCheck();
            timer2 = 0;
        }
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

        timer2 += Time.deltaTime;
        if(timer2 > attackTime)
        {
            attack();
            timer2 = 0;
        }
    }

    void attack()
    {
        //DO NOTHING
        //Debug.Log("Attacking");
        GameData.playerCharacter.gameObject.GetComponent<HealthScript>().addHealth(-attackValue);
    }

    void updateNextPathPos()
    {
        pathIndex++;
        pos = nextPos;

        //If monster has reached the end of its path
        if (path.Count - 1 <= pathIndex)
        {
            //If monster is next to its target switch to attack state
            if (nextToTarget())
            {
                state = State.attack;
            }
            //Else update the path to reach the player
            else
            {
                updatePath(nextPos);
            }
            return;
        }

        
        nextPos = pos + path[pathIndex];
        timer2 = 0;
    }

    void delayPathUpdateCheck()
    {
        timer += Time.deltaTime;
        if (timer > pathUpdateTimer && !nextToTarget())
        {
            updatePath(nextPos);
            timer = 0;
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

        nextPos = pos + path[0];
        pathIndex = 0;
        timer2 = 0;
    }

    public void aggro()
    {
        Debug.Log("I just got aggored warrrrhh");
        state = State.move;
        updatePath(pos);
    }

    bool nextToTarget()
    {
        //Debug.Log((Mathf.Abs(targetPos.x - pos.x) + Mathf.Abs(targetPos.y - pos.y)) == 1 || targetPos.Equals(pos));
        return (Mathf.Abs(GameData.playerCharacter.Pos.x - pos.x) + Mathf.Abs(GameData.playerCharacter.Pos.y - pos.y)) == 1 || GameData.playerCharacter.Pos.Equals(pos);
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
