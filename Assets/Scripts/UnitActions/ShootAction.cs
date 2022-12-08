using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
	public event EventHandler<OnShootEventArgs> OnShoot;
	
	public class OnShootEventArgs : EventArgs
	{
		public Unit targetUnit;
		public Unit shootingUnit;
	}
	
	private enum State
	{
		Aiming,
		Shooting,
		Cooloff,
	}
	
	private State state;
	
	[SerializeField] private int maxShootDistance = 7;
	private float stateTimer;
	private Unit targetUnit;
	private bool canShootBullet;

	// Update is called once per frame
	void Update()
	{
		if(!isActive)
		{
			return;
		}
		
		stateTimer -= Time.deltaTime;
		switch(state)
		{
			case State.Aiming:
				Aim();
				break;
			case State.Shooting:
				if(canShootBullet)
				{
					Shoot();
					canShootBullet = false;
				}
				break;
			case State.Cooloff:
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
			case State.Aiming:
				state = State.Shooting;
				float shootingStateTime = 0.1f;
				stateTimer = shootingStateTime;
				break;
			case State.Shooting:
				state = State.Cooloff;
				float coolOffStateTime = 0.5f;
				stateTimer = coolOffStateTime;
				break;
			case State.Cooloff:
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
	
	private void Shoot()
	{
		Debug.Log(this + "Shooter position is: " + unit.transform.position);
		if(OnShoot != null)
		{
			Debug.Log(this + "Shooter position is2: " + unit.transform.position);
			OnShoot(this, new OnShootEventArgs
			{
				targetUnit = targetUnit,
				shootingUnit = unit
			});
			targetUnit.Damage(40);
		}
		
		
	}
	
	public override string GetActionName()
	{
		return "Shoot";
	}

	public override List<GridPosition> GetValidActionGridPositionList()
	{
		GridPosition gridPosition = unit.GetGridPosition();
		return GetValidActionGridPositionList(gridPosition);
	}

	public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
	{
		List<GridPosition> validGridPositionList = new List<GridPosition>();
		
		for(int x = -maxShootDistance; x <= maxShootDistance; x++)
		{
			for(int z = -maxShootDistance; z <= maxShootDistance; z++)
			{
				GridPosition offsetGridPosition = new GridPosition(x, z);
				GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
				
				if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
				{
					// ignore
					continue;
				}
				
				int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
				if(testDistance > maxShootDistance)
				{
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
		
		// Debug.Log("Aiming");
		state = State.Aiming;
		float aimingStateTime = 1f;
		stateTimer = aimingStateTime;
		
		canShootBullet = true;
		
		ActionStart(onActionComplete);
	}
	
	public Unit GetTargetUnit()
	{
		return targetUnit;
	}
	
	public int GeMaxShootDistance()
	{
		return maxShootDistance;
	}
	
	public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
	{
		Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
		return new EnemyAIAction
		{
			gridPosition = gridPosition,
			actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalised()) * 100f)
		};
	}
	
	public int GetTargetCountAtPosition(GridPosition gridPosition)
	{
		return GetValidActionGridPositionList(gridPosition).Count;
	}
}
