using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AStarSearch : MonoBehaviour, IShortestPath<Vec2i, Vec2i>
{

    void Start()
    {
        GameData.aStar = this;
    }

    List<Vec2i> actions = new List<Vec2i>
    {
        new Vec2i(0, -1),
        new Vec2i(0, 1),
        new Vec2i(1, 0),
        new Vec2i(-1, 0)
    };
	
	public List<Vec2i> FindShortestPath(Vec2i from, Vec2i to){

        IShortestPath<Vec2i, Vec2i> info = this;
        ShortestPathGraphSearch<Vec2i, Vec2i> pathFinder = new ShortestPathGraphSearch<Vec2i, Vec2i>( info );

        return pathFinder.GetShortestPath(from, to);
    }

	bool Blocked(Vec2i tile){
        if (GameData.grid.getTile(tile) == null || !GameData.grid.getTile(tile).isWalkAble())
            return true;

        return false;
	}

    public float Heuristic(Vec2i fromState, Vec2i toState)
    {
        return Mathf.Abs(toState.x - fromState.x) + Math.Abs(toState.y - fromState.y);
    }

    public List<Vec2i> Expand(Vec2i position)
    {
        return actions;
    }

    public float ActualCost(Vec2i fromState, Vec2i action)
    {
        return 1f;
    }

    public Vec2i ApplyAction(Vec2i state, Vec2i action)
    {
        Vec2i next = state + action;
        if (!Blocked(next))
            return next;

        return state;
    }
}
