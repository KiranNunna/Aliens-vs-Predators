using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	private const int ACTION_POINTS_MAX = 2;
	
	public static event EventHandler OnAnyActionPointsChanged;
	
	private GridPosition gridPosition;
	
	private MoveAction moveAction;
	private SpinAction spinAction;
	
	private BaseAction[] baseActionArray;
	
	private int actionPoints = ACTION_POINTS_MAX;
	
	void Awake()
	{
		moveAction = GetComponent<MoveAction>();
		spinAction = GetComponent<SpinAction>();
		
		baseActionArray = GetComponents<BaseAction>();
		
		// Debug.Log(this + ", " + LevelGrid.Instance + ", " + gridPosition);
		gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
		LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
	}
	
	// Start is called before the first frame update
	void Start()
	{
		TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
	}

	// Update is called once per frame
	void Update()
	{
		GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
		if(newGridPosition != gridPosition)
		{
			// Debug.Log(this + "position changed!");
			LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
			gridPosition = newGridPosition;
		}
	}
	
	public MoveAction GetMoveAction()
	{
		return moveAction;
	}
	
	public SpinAction GetSpinAction()
	{
		return spinAction;
	}
	
	public GridPosition GetGridPosition()
	{
		return gridPosition;
	}
	
	public BaseAction[] GetBaseActionArray()
	{
		return baseActionArray;
	}
	
	public bool TrySpendingActionPointsToTakeAction(BaseAction baseAction)
	{
		if(CanSpendActionPointsToTakeAction(baseAction))
		{
			SpendActionPoints(baseAction.GetActionPointsCost());
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
	{
		if(actionPoints >= baseAction.GetActionPointsCost())
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	private void SpendActionPoints(int amount)
	{
		actionPoints -= amount;
		
		if(OnAnyActionPointsChanged != null)
		{
			OnAnyActionPointsChanged(this, EventArgs.Empty);
		}
	}
	
	public int GetActionPoints()
	{
		return this.actionPoints;
	}
	
	private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
	{
		actionPoints = ACTION_POINTS_MAX;
		
		if(OnAnyActionPointsChanged != null)
		{
			OnAnyActionPointsChanged(this, EventArgs.Empty);
		}
	}
}