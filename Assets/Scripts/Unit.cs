using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	private const int ACTION_POINTS_MAX = 2;
	
	public static event EventHandler OnAnyActionPointsChanged;
	public static event EventHandler OnAnyUnitSpawned;
	public static event EventHandler OnAnyUnitDead;
	
	private GridPosition gridPosition;
	
	private HealthSystem healthSystem;
	
	private MoveAction moveAction;
	private SpinAction spinAction;
	private ShootAction shootAction;
	
	private BaseAction[] baseActionArray;
	
	private int actionPoints = ACTION_POINTS_MAX;
	
	[SerializeField] private bool isEnemy;
	
	void Awake()
	{
		healthSystem = GetComponent<HealthSystem>();
		
		moveAction = GetComponent<MoveAction>();
		spinAction = GetComponent<SpinAction>();
		shootAction = GetComponent<ShootAction>();
		
		baseActionArray = GetComponents<BaseAction>();
		
		// Debug.Log(this + ", " + LevelGrid.Instance + ", " + gridPosition);
		gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
		LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
	}
	
	// Start is called before the first frame update
	void Start()
	{
		TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

		healthSystem.OnDead += HealthSystem_OnDead;
		
		if(OnAnyUnitSpawned != null)
		{
			OnAnyUnitSpawned(this, EventArgs.Empty);
		}
	}

	// Update is called once per frame
	void Update()
	{
		GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
		if(newGridPosition != gridPosition)
		{
			// Debug.Log(this + "position changed!");
			GridPosition oldGridPosition = gridPosition;
			gridPosition = newGridPosition;
			LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
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
	
	public ShootAction GetShootAction()
	{
		return shootAction;
	}
	
	public GridPosition GetGridPosition()
	{
		return gridPosition;
	}
	
	public Vector3 GetWorldPosition()
	{
		return this.transform.position;
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
		// If is enemy and enemy's turn we reset action points.
		// If is player and player's turn we reset action points.
		if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
			(!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
		{
			actionPoints = ACTION_POINTS_MAX;
		
			if(OnAnyActionPointsChanged != null)
			{
				OnAnyActionPointsChanged(this, EventArgs.Empty);
			}
		}
		
	}
	
	public bool IsEnemy()
	{
		return this.isEnemy;
	}
	
	public void Damage(int damageAmount)
	{
		healthSystem.Damage(damageAmount);
	}
	
	private void HealthSystem_OnDead(object sender, EventArgs e)
	{
		LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
		Destroy(gameObject);
		
		if(OnAnyUnitDead != null)
		{
			OnAnyUnitDead(this, EventArgs.Empty);
		}
	}
	
	public float GetHealthNormalised()
	{
		return healthSystem.GetHealthNormalised();
	}
}
