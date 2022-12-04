using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
	private GridObject gridObject;
	[SerializeField] private TextMeshPro debugTMP;
	
	void Update()
	{
		SetGridObject(gridObject);
	}
	
	public void SetGridObject(GridObject gridObject)
	{
		this.gridObject = gridObject;
		
		debugTMP.SetText(gridObject.ToString());
	}
}
