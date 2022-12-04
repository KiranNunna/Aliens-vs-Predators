using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
	public static UnitActionSystem Instance {get; private set;}
	
	public event EventHandler OnSelectedUnitChanged;
	public event EventHandler OnSelectedActionChanged;
	public event EventHandler OnActionStarted;
	
	[SerializeField] private Unit selectedUnit;
	[SerializeField] private LayerMask unitsLayerMask;
	
	private BaseAction selectedAction;
	
	public event EventHandler<bool> OnBusyChanged;
	private bool isBusy;
	
	void Awake()
	{
		if(Instance != null)
		{
			Debug.LogError("There's more than one UnitActionSystem!" + transform + " - " + Instance);
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}
	
	// Start is called before the first frame update
	void Start()
	{
		// selectedUnit = null;
		SetSelectedUnit(selectedUnit);
	}

	// Update is called once per frame
	void Update()
	{
		if(isBusy)
		{
			return;
		}
		
		if(EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}
		
		if(TryHandleUnitSelection())
		{
			return;
		}
		
		HandleSelectedAction();
	}
	
	private void SetBusy()
	{
		isBusy = true;
		if(OnBusyChanged != null)
		{
			OnBusyChanged(this, isBusy);
		}
	}
	
	private void ClearBusy()
	{
		isBusy = false;
		if(OnBusyChanged != null)
		{
			OnBusyChanged(this, isBusy);
		}
	}
	
	private bool TryHandleUnitSelection()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitsLayerMask))
			{
				if(raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
				{
					// Debug.Log(unit);
					if(unit == selectedUnit)
					{
						// This unit is already selected
						 return false;
					}
					SetSelectedUnit(unit);
					return true;
				}
			}
		}
		return false;
	}
	
	private void HandleSelectedAction()
	{
		if(Input.GetMouseButtonDown(0))
		{
			GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
			
			if(selectedAction.IsValidActionGridPosition(mouseGridPosition))
			{
				if(selectedUnit.CanSpendActionPointsToTakeAction(selectedAction))
				{
					if(selectedUnit.TrySpendingActionPointsToTakeAction(selectedAction))
					{
						SetBusy();
						selectedAction.TakeAction(mouseGridPosition, ClearBusy);
					
						if(OnActionStarted != null)
							{
								OnActionStarted(this, EventArgs.Empty);
							}
					}
				}
			}
		}
	}
	
	private void SetSelectedUnit(Unit unit)
	{
		selectedUnit = unit;
		SetSelectedAction(unit.GetMoveAction());
		// Triggering an event
		if(OnSelectedUnitChanged != null)
		{
			OnSelectedUnitChanged(this, EventArgs.Empty);
		}
	}
	
	public void SetSelectedAction(BaseAction baseAction)
	{
		selectedAction = baseAction;
		
		if(OnSelectedActionChanged != null)
		{
			OnSelectedActionChanged(this, EventArgs.Empty);
		}
	}
	
	public Unit GetSelectedUnit()
	{
		return selectedUnit;
	}
	
	public BaseAction GetSelectedAction()
	{
		return selectedAction;
	}
	
	public bool GetBusyStatus()
	{
		return this.isBusy;
	}
}
