using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
	[SerializeField] private Transform ragdollPrefab;
	[SerializeField] private Transform originalRootBone;

	private HealthSystem healthSystem;
	private ShootAction shootAction;
	
	private static Vector3 shooterPosition;
	
	void Awake()
	{
		healthSystem = GetComponent<HealthSystem>();
		shootAction = GetComponent<ShootAction>();
		
		healthSystem.OnDead += HealthSystem_OnDead;

		shootAction.OnShoot += ShootAction_OnShoot;
	}
	
	private void HealthSystem_OnDead(object sender, EventArgs e)
	{
		// Spawns a ragdoll in place of the unit with the exact pose and sets it up
		// for an explosion.
		Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
		UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
		Debug.Log("HealthSystem_OnDead: " + shooterPosition);
		unitRagdoll.Setup(originalRootBone, shooterPosition);
	}
	
	private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
	{
		shooterPosition = e.shootingUnit.GetWorldPosition();
		Debug.Log("ShootAction_OnShoot: " + shooterPosition);
	}
}
