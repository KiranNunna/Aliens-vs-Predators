using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
NOTE: The thought process while designing the action scripts
	  is that we assume the action scripts are always attached
	  to the unit.
*/

public class MoveAction : BaseAction
{
	public event EventHandler OnStartMoving;
	public event EventHandler OnStopMoving;
	
	private float stoppingDistance = 0.1f;
	[SerializeField] private float moveSpeed = 4f;
	[SerializeField] private float rotateSpeed = 10f;
	
	[SerializeField] private int maxMoveDistance = 4;
	
	private List<Vector3> positionList;
	private int currentPositionIndex;

	// Update is called once per frame
	void Update()
	{	
		if(!isActive)
		{
			return;
		}
		
		Vector3 targetPosition = positionList[currentPositionIndex];
		Vector3 moveDirection = (targetPosition - transform.position).normalized;
		
		transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
		
		if(Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
		{
			transform.position += moveDirection * moveSpeed * Time.deltaTime;
		}
		else
		{
			currentPositionIndex++;
			if(currentPositionIndex >= positionList.Count)
			{
				if(OnStopMoving != null)
				{
					OnStopMoving(this, EventArgs.Empty);
				}
				ActionComplete();
			}
		}
	}
	
	public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
	{
		List<GridPosition> pathGridPositionList = PathFinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);
		currentPositionIndex = 0;
		positionList = new List<Vector3>();
		
		foreach(GridPosition pathGridPosition in pathGridPositionList)
		{
			positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
		}
		
		if(OnStartMoving != null)
		{
			OnStartMoving(this, EventArgs.Empty);
		}
		
		ActionStart(onActionComplete);
	}
	
	public override List<GridPosition> GetValidActionGridPositionList()
	{
		List<GridPosition> validGridPositionList = new List<GridPosition>();
		
		GridPosition unitGridPosition = unit.GetGridPosition();
		for(int x = -maxMoveDistance; x <= maxMoveDistance; x++)
		{
			for(int z = -maxMoveDistance; z <= maxMoveDistance; z++)
			{
				GridPosition offsetGridPosition = new GridPosition(x, z);
				GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
				
				if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
				{
					// ignore
					continue;
				}
				if(unitGridPosition == testGridPosition)
				{
					// Same grid position where we are standing at
					// ignore
					continue;
				}
				if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
				{
					// Grid position is already occupied with another unit
					// ignore
					continue;
				}
				if(!PathFinding.Instance.IsWalkableGridPosition(testGridPosition))
				{
					continue;
				}
				if(!PathFinding.Instance.HasPath(unitGridPosition, testGridPosition))
				{
					continue;
				}
				
				int pathfindingDistanceMultiplier = 10;
				if(PathFinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier)
				{
					// Path length is too long
					continue;
				}
				
				// Debug.Log(testGridPosition);
				validGridPositionList.Add(testGridPosition);
			}
		}
		
		return validGridPositionList;
	}
	
	public override string GetActionName()
	{
		return "Move";
	}
	
	public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
	{
		int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
		return new EnemyAIAction
		{
			gridPosition = gridPosition,
			actionValue = targetCountAtGridPosition * 10
		};
	}
}
