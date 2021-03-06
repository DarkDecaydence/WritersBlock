/// UnityUtils https://github.com/mortennobel/UnityUtils
/// By Morten Nobel-Jørgensen
/// License lgpl 3.0 (http://www.gnu.org/licenses/lgpl-3.0.txt)


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Interface for a shortest path problem
/// </summary>
public interface IShortestPath<State,Action> {
	/// <summary>
	/// Should return a estimate of shortest distance. The estimate must me admissible (never overestimate)
	/// </summary>
	float Heuristic(State fromState, State toState);
	
	/// <summary>
	/// Return the legal moves from a state
	/// </summary>
	List<Action> Expand(State position);
	
	/// <summary>
	/// Return the actual cost between two adjecent states, given state+action
	/// </summary>
	float ActualCost(State fromState, Action action);
	
	/// <summary>
	/// Returns the new state after an action has been applied
	/// </summary>
	State ApplyAction(State state, Action action);
}
