using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
	public event EventHandler OnDead;
	public event EventHandler OnDamaged;
	
	[SerializeField] private int health = 100;
	private int healthMax;
	
	private ShootAction shootAction;
	
	void Awake()
	{
		healthMax = health;
		
		shootAction = GetComponent<ShootAction>();
	}	
	
	public void Damage(int damageAmount)
	{
		health -= damageAmount;
		
		if(health < 0)
		{
			health = 0;
		}
		
		if(OnDamaged != null)
		{
			OnDamaged(this, EventArgs.Empty);
		}
		
		if(health == 0)
		{
			Die();
		}
	}
	
	private void Die()
	{
		if(OnDead != null)
		{
			OnDead(this, EventArgs.Empty);
		}
	}
	
	public float GetHealthNormalised()
	{
		return (float)health / (float)healthMax;	
	}
}
