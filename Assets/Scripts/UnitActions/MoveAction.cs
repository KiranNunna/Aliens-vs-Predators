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
	[SerializeField] private Animator unitAnimator;
	
	private float stoppingDistance = 0.1f;
	[SerializeField] private float moveSpeed = 4f;
	[SerializeField] private float rotateSpeed = 10f;
	
	[SerializeField] private int maxMoveDistance = 4;
	
	private Vector3 targetPosition;
	
	protected override void Awake()
	{
		base.Awake();
		this.targetPosition = transform.position;
	}
	
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{	
		if(!isActive)
		{
			return;
		}
		
		Vector3 moveDirection = (targetPosition - transform.position).normalized;
		if(Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
		{
			transform.position += moveDirection * moveSpeed * Time.deltaTime;
			
			unitAnimator.SetBool("IsWalking", true);
		}
		else
		{
			unitAnimator.SetBool("IsWalking", false);
			isActive = false;
			onActionComplete();
		}
		
		transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
	}
	
	public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
	{
		this.onActionComplete = onActionComplete;
		this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
		isActive = true;
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
}
