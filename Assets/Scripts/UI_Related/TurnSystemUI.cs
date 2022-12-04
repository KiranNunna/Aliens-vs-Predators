using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
	[SerializeField] private Button endTurnButton;
	[SerializeField] private TextMeshProUGUI turnNumberText;
	// Start is called before the first frame update
	void Start()
	{
		endTurnButton.onClick.AddListener(() =>
		{
			TurnSystem.Instance.NextTurn();
		});
		
		TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
		
		UpdateTurnNumberText();
	}
	
	private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
	{
		UpdateTurnNumberText();
	}
	
	public void UpdateTurnNumberText()
	{
		turnNumberText.SetText("TURN: " + TurnSystem.Instance.GetTurnNumber());
	}
}
