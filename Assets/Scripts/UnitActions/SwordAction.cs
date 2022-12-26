using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
	public static event EventHandler<OnSwordEventArgs> OnAnySword;
	public event EventHandler OnSwordActionStarted;
	public event EventHandler OnSwordActionCompleted;
	
	public class OnSwordEventArgs : EventArgs
	{
		public Unit targetUnit;
		public Unit swordUnit;
	}
	
	private enum State
	{
		SwingingSwordBeforeHit,
		SwingingSwordAfterHit,
	}
	
	private int maxSwordDistance = 1;
	private State state;
	private float stateTimer;
	private Unit targetUnit;
	
	void Update()
	{
		if(!isActive)
		{
			return;
		}
		
		stateTimer -= Time.deltaTime;
		switch(state)
		{
			case State.SwingingSwordBeforeHit:
				Aim();
				break;
			case State.SwingingSwordAfterHit:
				break;
		}
		
		if(stateTimer <= 0f)
		{
			NextState();
		}
	}
	
	private void NextState()
	{
		switch (state)
		{
			case State.SwingingSwordBeforeHit:
				state = State.SwingingSwordAfterHit;
				float afterHitStateTime = 0.5f;
				stateTimer = afterHitStateTime;
				
				if(OnAnySword != null)
				{
					OnAnySword(this, new OnSwordEventArgs
					{
						targetUnit = targetUnit,
						swordUnit = unit,
					});
				}
				
				targetUnit.Damage(100);
				break;
			case State.SwingingSwordAfterHit:
				if(OnSwordActionCompleted != null)
				{
					OnSwordActionCompleted(this, EventArgs.Empty);
				}
			
				ActionComplete();
				break;
		}
		// Debug.Log(state);
	}
	
	private void Aim()
	{
		// Rotate towards the target
		Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
		float rotateSpeed = 10f;
		transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotateSpeed * Time.deltaTime);
	}
	
	public override string GetActionName()
	{
		return "Sword";
	}

	public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
	{
		return new EnemyAIAction
		{
			gridPosition = gridPosition,
			actionValue = 200,
		};
	}

	public override List<GridPosition> GetValidActionGridPositionList()
	{
		List<GridPosition> validGridPositionList = new List<GridPosition>();
		GridPosition unitGridPosition = unit.GetGridPosition();
		
		for(int x = -maxSwordDistance; x <= maxSwordDistance; x++)
		{
			for(int z = -maxSwordDistance; z <= maxSwordDistance; z++)
			{
				GridPosition offsetGridPosition = new GridPosition(x, z);
				GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
				
				if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
				{
					// ignore
					continue;
				}
				
				if(!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
				{
					// Grid Position is empty, no Unit.
					// ignore
					continue;
				}
				
				Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
				
				if(targetUnit.IsEnemy() == unit.IsEnemy())
				{
					// Both are on the same 'team'
					continue;
				}
				
				// Debug.Log(testGridPosition);
				validGridPositionList.Add(testGridPosition);
			}
		}
		
		return validGridPositionList;
	}

	public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
	{
		targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition); 
		
		state = State.SwingingSwordBeforeHit;
		float beforeHitStateTime = 0.7f;
		stateTimer = beforeHitStateTime;
		
		if(OnSwordActionStarted != null)
		{
			OnSwordActionStarted(this, EventArgs.Empty);
		}
		
		ActionStart(onActionComplete);
	}
	
	public int GetMaxSwordDistance()
	{
		return maxSwordDistance;
	}
}
