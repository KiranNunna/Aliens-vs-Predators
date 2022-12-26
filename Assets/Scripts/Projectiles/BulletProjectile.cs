using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
	[SerializeField] private TrailRenderer trailRenderer;
	[SerializeField] private Transform bulletHitVFXPrefab;
	
	private Vector3 targetPosition;
	[SerializeField] private float moveSpeed = 200f;
	
	public void Setup(Vector3 targetPosition)
	{
		this.targetPosition = targetPosition;
	}
	
	void Update()
	{
		Vector3 moveDir = (targetPosition - transform.position).normalized;
		
		float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);
		
		transform.position += moveDir * moveSpeed * Time.deltaTime;
		
		float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);
		
		if(distanceBeforeMoving < distanceAfterMoving)
		{
			// To avoid overshooting
			transform.position = targetPosition;			
			
			trailRenderer.transform.parent = null;
			Destroy(gameObject);
			
			// Instantiate bullet hit VFX at hit location
			Instantiate(bulletHitVFXPrefab, targetPosition, Quaternion.identity);
		}
	}
}
