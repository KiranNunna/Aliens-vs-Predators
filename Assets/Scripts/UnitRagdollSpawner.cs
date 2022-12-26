using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
	[SerializeField] private Transform ragdollPrefab;
	[SerializeField] private Transform originalRootBone;

	private HealthSystem healthSystem;
	
	private Vector3 damagedFromPosition;
	
	void Awake()
	{
		healthSystem = GetComponent<HealthSystem>();
		
		healthSystem.OnDead += HealthSystem_OnDead;
		ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
		GrenadeAction.OnAnyGrenade += GrenadeAction_OnAnyGrenade;
		SwordAction.OnAnySword += SwordAction_OnAnySword;
	}
	
	private void HealthSystem_OnDead(object sender, EventArgs e)
	{
		// Spawns a ragdoll in place of the unit with the exact pose and sets it up
		// for an explosion.
		Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
		UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
		// Debug.Log("HealthSystem_OnDead: " + damagedFromPosition);
		unitRagdoll.Setup(originalRootBone, damagedFromPosition);
	}
	
	private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
	{
		damagedFromPosition = e.shootingUnit.GetWorldPosition();
		// Debug.Log("ShootAction_OnShoot: " + damagedFromPosition);
	}
	
	private void GrenadeAction_OnAnyGrenade(object sender, GrenadeAction.OnGrenadeEventArgs e)
	{
		damagedFromPosition = LevelGrid.Instance.GetWorldPosition(e.grenadeGridPosition);
		// Debug.Log("GrenadeAction_OnAnyGrenade: " + damagedFromPosition);
	}
	
	private void SwordAction_OnAnySword(object sender, SwordAction.OnSwordEventArgs e)
	{
		damagedFromPosition = e.swordUnit.GetWorldPosition();
		// Debug.Log("SwordAction_OnAnySword: " + damagedFromPosition);
	}
}
