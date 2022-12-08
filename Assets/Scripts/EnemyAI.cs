using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	private enum State
	{
		WaitingForEnemyTurn,
		TakingTurn,
		Busy
	}
	
	private const float timerDuration = 2f;
	private float timer;
	
	private State state;
	
	void Awake()
	{
		state = State.WaitingForEnemyTurn;
	}
	
	void Start()
	{
		timer = timerDuration;
		
		TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
	}
	
	// Update is called once per frame
	void Update()
	{
		if(TurnSystem.Instance.IsPlayerTurn())
		{
			return;
		}
		
		switch(state)
		{
			case State.WaitingForEnemyTurn:
				break;
			case State.TakingTurn:
				timer -= Time.deltaTime;
				if(timer <= 0f)
				{
					state = State.Busy;
					if(TryTakeEnemyAIAction(SetStateTakingTurn))
					{
						state = State.Busy;
					}
					else
					{
						// No more enemies have action they can take, end the turn.
						TurnSystem.Instance.NextTurn();
					}
				}
				break;
			case State.Busy:
				break;
		}
		
		
	}
	
	private void SetStateTakingTurn()
	{
		timer = 0.5f;
		state = State.TakingTurn;
	}
	
	private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
	{
		if(!TurnSystem.Instance.IsPlayerTurn())
		{
			state = State.TakingTurn;
			timer = timerDuration;
		}
		
	}
	
	private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
	{
		foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
		{
			if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
			{
				return true;	
			}
		}
		return false;
	}
	
	private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
	{
		EnemyAIAction bestEnemyAIAction = null;
		BaseAction bestBaseAction = null;
		
		foreach(BaseAction baseAction in enemyUnit.GetBaseActionArray())
		{
			if(!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
			{
				// Enemy cannot afford to take action
				continue;
			}
			
			if(bestEnemyAIAction == null)
			{
				bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
				bestBaseAction = baseAction;
			}
			else
			{
				EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
				if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
				{
					bestEnemyAIAction = testEnemyAIAction;
					bestBaseAction = baseAction;
				}
			}
		}
		
		if(bestEnemyAIAction != null && enemyUnit.TrySpendingActionPointsToTakeAction(bestBaseAction))
		{
			bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
			return true;
		}
		else
		{
			return false;
		}
	}
}
