using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
	public static TurnSystem Instance {get; private set;}
	
	public event EventHandler OnTurnChanged;
	
	private int turnNumber = 1;
	
	void Awake()
	{
		if(Instance != null)
		{
			Debug.LogError("There's more than one TurnSystem!" + transform + " - " + Instance);
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}
	
	public void NextTurn()
	{
		turnNumber += 1;
		
		if(OnTurnChanged != null)
		{
			OnTurnChanged(this, EventArgs.Empty);
		}
	}
	
	public int GetTurnNumber()
	{
		return this.turnNumber;
	}
}