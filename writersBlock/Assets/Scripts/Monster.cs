using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour {

    Vec2i pos;
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

    State state;
    enum State { idle, walk, attack}

	// Use this for initialization
	void Start ()
    {

        state = State.idle;
        pos = new Vec2i(5, 5);
        setObjectPosition(pos);
        updatePath(pos);

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
            case State.idle:
                idleBehavior();
                break;
            case State.walk:
                moveBehavior();
                break;
            case State.attack:
                attackBehavior();
                break;
        }
        
    }

    void idleBehavior()
    {
        state = State.walk;
    }

    void moveBehavior()
    {

        if (path == null)
            return;

        timer += Time.deltaTime;
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
        if (nextToTarget())
        {
            state = State.walk;
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
        Debug.Log("Attack is not implemented yet");
    }

    void updateNextPathPos()
    {
        pathIndex++;

        //If monster has reached the end of its path
        if(path.Count <= pathIndex)
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

        pos = nextPos;
        nextPos = pos + path[pathIndex];
        timer2 = 0;
    }

    void delayPathUpdateCheck()
    {
        if (timer > pathUpdateTimer)
        {
            updatePath(nextPos);
            timer = 0;
        }
    }

    void updatePath(Vec2i from)
    {

        if (targetPos.Equals(GameData.playerCharacter.pos))
            return;

        targetPos = GameData.playerCharacter.pos;
        path = GameData.aStar.FindShortestPath(pos, targetPos);

        nextPos = pos + path[0];
        pathIndex = 0;
        timer2 = 0;
    }

    bool nextToTarget()
    {
        return Mathf.Abs(targetPos.x - pos.x) == 1 || Mathf.Abs(targetPos.y - pos.y) == 1;
    }

    void setObjectPosition(Vec2i newPos)
    {
        transform.position = new Vector3(newPos.x + TileMetrics.tileHalfLength, transform.position.y, newPos.y + TileMetrics.tileHalfLength);
    }

    void setObjectPosition(Vector3 newPos)
    {
        transform.position = new Vector3(newPos.x + TileMetrics.tileHalfLength, transform.position.y, newPos.z + TileMetrics.tileHalfLength);
    }
}
