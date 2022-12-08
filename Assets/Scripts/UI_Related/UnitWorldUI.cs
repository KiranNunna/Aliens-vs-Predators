using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitWorldUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI actionPointsText;
	[SerializeField] private Unit unit;
	[SerializeField] private Image healthBarImage;
	[SerializeField] private HealthSystem healthSystem;
	
	void Start()
	{
		Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
		healthSystem.OnDamaged += HealthSystem_OnDamaged;
		
		UpdateActionPointsText();
		UpdateHealthBar();
	}
	
	private void UpdateActionPointsText()
	{
		actionPointsText.SetText(unit.GetActionPoints().ToString());
	}
	
	private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
	{
		UpdateActionPointsText();
	}
	
	private void UpdateHealthBar()
	{
		healthBarImage.fillAmount = healthSystem.GetHealthNormalised();
	}
	
	private void HealthSystem_OnDamaged(object sender, EventArgs e)
	{
		UpdateHealthBar();
	}
}
