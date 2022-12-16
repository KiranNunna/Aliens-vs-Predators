using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
	private object gridObject;
	[SerializeField] private TextMeshPro debugTMP;
	
	protected virtual void Update()
	{
		debugTMP.SetText(gridObject.ToString());
	}
	
	public virtual void SetGridObject(object gridObject)
	{
		this.gridObject = gridObject;
	}
}
