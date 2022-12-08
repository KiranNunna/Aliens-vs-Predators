using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
	private float totalSpinAmount = 0;
	
	void Update()
	{
		if(!isActive)
		{
			return;
		}

		float spinAddAmount = 360f * Time.deltaTime;
		totalSpinAmount += spinAddAmount;
		transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
		
		if(totalSpinAmount >= 360)
		{
			ActionComplete();
		}
	}
	
	public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
	{
		totalSpinAmount = 0f;
		ActionStart(onActionComplete);
	}
	
	public override string GetActionName()
	{
		return "Spin";
	}
	
	public override List<GridPosition> GetValidActionGridPositionList()
	{
		GridPosition unitGridPosition = unit.GetGridPosition();
		
		return new List<GridPosition>{unitGridPosition};
	}

	public override int GetActionPointsCost()
	{
		return 1;
	}
	
	public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
	{
		return new EnemyAIAction
		{
			gridPosition = gridPosition,
			actionValue = 0
		};
	}
}
