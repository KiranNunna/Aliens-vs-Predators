using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
	public static event EventHandler<OnGrenadeEventArgs> OnAnyGrenade;
	
	public class OnGrenadeEventArgs : EventArgs
	{
		public GridPosition grenadeGridPosition;
	}
	
	[SerializeField] private Transform grenadeProjectilePrefab;
	
	private int maxThrowDistance = 7;
	
	void Update()
	{
		if(!isActive)
		{
			return;
		}
	}
	
	public override string GetActionName()
	{
		return "Grenade";
	}

	public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
	{
		return new EnemyAIAction
		{
			gridPosition = gridPosition,
			actionValue = 0,
		};
	}

	public override List<GridPosition> GetValidActionGridPositionList()
	{
		List<GridPosition> validGridPositionList = new List<GridPosition>();
		GridPosition unitGridPosition = unit.GetGridPosition();
		
		for(int x = -maxThrowDistance; x <= maxThrowDistance; x++)
		{
			for(int z = -maxThrowDistance; z <= maxThrowDistance; z++)
			{
				GridPosition offsetGridPosition = new GridPosition(x, z);
				GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
				
				if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
				{
					// ignore
					continue;
				}
				
				int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
				if(testDistance > maxThrowDistance)
				{
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
		Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
		GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
		grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviourComplete);
		
		if(OnAnyGrenade != null)
		{
			OnAnyGrenade(this, new OnGrenadeEventArgs
			{
				grenadeGridPosition = gridPosition,
			});
		}
		
		// Debug.Log("Grenade Action");
		ActionStart(onActionComplete);
	}
	
	private void OnGrenadeBehaviourComplete()
	{
		ActionComplete();
	}
}
