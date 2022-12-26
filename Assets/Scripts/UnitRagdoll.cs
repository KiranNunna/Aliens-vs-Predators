using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
	[SerializeField] private Transform ragdollRootBone;
	[SerializeField] private Transform rifleModel;
	
	[SerializeField] private float explosionForce = 300f;
	[SerializeField] private float explosionRange = 10f;
	[SerializeField] private float explosionHeight = 2f;
	
	[SerializeField] private float slowdownFactor = 0.05f;
	[SerializeField] private float slowdownLength = 2f;
	
	private float timer;
	private float defaultFixedDeltaTime;
	private bool isInSlowMotion = false;
	
	void Start()
	{
		timer = slowdownLength;
		defaultFixedDeltaTime = Time.fixedDeltaTime;
	}
	
	void Update()
	{	
		// Using unscaledDeltaTime here because, since we are tampering with
		// timescale, it also changes the value of deltaTime.
		// Time.timeScale += (1f/slowdownLength) * Time.unscaledDeltaTime;
		// Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
		
		if(isInSlowMotion)
		{
			timer -= Time.unscaledDeltaTime;
		}
		if(timer <= 0)
		{
			StopSlowMotion();
			this.enabled = false;
		}
	}
	
	public void Setup(Transform originalRootBone, Vector3 shooterPosition)
	{
		MatchAllChildTransforms(originalRootBone, ragdollRootBone);
		
		DoSlowMotion();	
		
		// Applying explosion
		float offSet = 0.5f;
		Vector3 explosionPosition = (((shooterPosition - this.transform.position).normalized) * offSet) + this.transform.position + new Vector3(0, explosionHeight, 0); // 1.7 is the neck height of the unit
		// Debug.Log("ShooterPosition: " + shooterPosition + "; " + "TargetPosition: " + this.transform.position + "; " + explosionPosition);
		ApplyExplosionToRagdoll(ragdollRootBone, explosionForce, explosionPosition, explosionRange);
	}
	
	private void MatchAllChildTransforms(Transform root, Transform clone)
	{
		/*
		Calling recursive function to copy the transform of the 
		unit bone transforms onto ragdoll bones so that the death
		animation seems much more fluid and realistic.
		*/
		foreach(Transform child in root)
		{
			Transform cloneChild = clone.Find(child.name);
			if(cloneChild != null)
			{
				cloneChild.position = child.position;
				cloneChild.rotation = child.rotation;
			
				// Making a recursive call because
				// there are multiple levels in the 
				// bone structure.
				
				// *DFS (Depth First Search)*
				MatchAllChildTransforms(child, cloneChild);
			}
		}
		
		// Detaching rifleModel from the ragdoll
		rifleModel.transform.parent = null;
	}
	
	private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
	{
		foreach(Transform child in root)
		{
			if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
			{
				childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
			}
			
			ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
		}
	}
	
	private void DoSlowMotion()
	{
		isInSlowMotion = true;
		Time.timeScale = slowdownFactor;
		Time.fixedDeltaTime = Time.timeScale * 0.02f;
	}
	
	private void StopSlowMotion()
	{
		isInSlowMotion = false;
		Time.timeScale = 1;
		Time.fixedDeltaTime = defaultFixedDeltaTime;
	}
}
