/// UnityUtils https://github.com/mortennobel/UnityUtils
/// By Morten Nobel-Jørgensen
/// License lgpl 3.0 (http://www.gnu.org/licenses/lgpl-3.0.txt)


using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Based on uniform-cost-search/A* from the book
/// Artificial Intelligence: A Modern Approach 3rd Ed by Russell/Norvig 
/// </summary>
public class ShortestPathGraphSearch<State, Action> {
	private IShortestPath<State,Action> info;
	public ShortestPathGraphSearch(IShortestPath<State,Action> info){
		this.info = info;
	}
	
	public List<Action> GetShortestPath(State fromState, State toState){
		HeapPriorityQueue<SearchNode<State,Action>> frontier = new HeapPriorityQueue<SearchNode<State,Action>>(99999);
		HashSet<State> exploredSet = new HashSet<State>();
		Dictionary<State, SearchNode<State,Action>> frontierMap = new Dictionary<State, SearchNode<State,Action>>();
		
		SearchNode<State, Action> startNode = new SearchNode<State,Action>(null,0,0,fromState, default(Action));
        frontier.Enqueue(startNode,0);

		frontierMap.Add(fromState, startNode);

		while (frontier.Count > 0){
			SearchNode<State,Action> node = frontier.Dequeue();
			frontierMap.Remove(node.state);
			
			if (node.state.Equals(toState)) return BuildSolution(node);
			exploredSet.Add(node.state);
			// expand node and add to frontier
			foreach (Action action in info.Expand(node.state)){
				State child = info.ApplyAction(node.state, action);
				
				SearchNode<State,Action> frontierNode = null;
				bool isNodeInFrontier = frontierMap.TryGetValue(child, out frontierNode);
				if (!exploredSet.Contains(child) && !isNodeInFrontier){
					SearchNode<State,Action> searchNode = CreateSearchNode(node, action, child, toState);
					frontier.Enqueue(searchNode,searchNode.f);
					frontierMap.Add(child, searchNode);
				} else if (isNodeInFrontier) {
					SearchNode<State,Action> searchNode = CreateSearchNode(node, action, child, toState);
					if (frontierNode.f > searchNode.f){
						frontier.Remove(frontierNode);
						frontier.Enqueue(searchNode, searchNode.f);
					}
				}
			}
		}

		return null;
	}
	
	private SearchNode<State,Action> CreateSearchNode(SearchNode<State,Action> node, Action action, State child, State toState){
		float cost = info.ActualCost(node.state, action);
		float heuristic = info.Heuristic(child, toState);
		return new SearchNode<State,Action>(node, node.g+cost,node.g+cost+heuristic,child,action);
	}
	
	private List<Action> BuildSolution(SearchNode<State,Action> seachNode){
		List<Action> list = new List<Action>();
		while (seachNode != null){
			if ((seachNode.action != null ) && (!seachNode.action.Equals(default(Action)))){
				list.Insert(0,seachNode.action);
			}
			seachNode = seachNode.parent;
		}
		return list;
	}
}

class SearchNode<State,Action> : PriorityQueueNode, IComparable<SearchNode<State,Action>> {
	public SearchNode<State,Action> parent;
	
	public State state;
	public Action action;
	public float g; // cost
	public float f; // estimate
	
	public SearchNode(SearchNode<State,Action> parent, float g, float f, State state, Action action){
		this.parent = parent;
		this.g = g;
		this.f = f;
		this.state = state;
		this.action = action;
	}
	
	/// <summary>
	/// Reverse sort order (smallest numbers first)
	/// </summary>
    public int CompareTo(SearchNode<State,Action> other)
    {
		return other.f.CompareTo(f);
	}
	
	public override string ToString() {
		return "SN {f:"+f+", state: "+state+" action: "+action+"}";
	}
}
