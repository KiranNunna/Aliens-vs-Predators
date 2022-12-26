using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
	public static event EventHandler OnAnyDestroyed; 
	
	[SerializeField] private Transform crateDestroyedPrefab;
	private GridPosition gridPosition;
	
	void Start()
	{
		gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
	}
	
	public void Damage()
	{
		Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);
		ApplyExplosionToChildren(crateDestroyedTransform, 150f, transform.position, 10f);
		
		Destroy(gameObject);
		
		if(OnAnyDestroyed != null)
		{
			OnAnyDestroyed(this, EventArgs.Empty);
		}
	}
	
	public GridPosition GetGridPosition()
	{
		return this.gridPosition;
	}
	
	private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
	{
		foreach(Transform child in root)
		{
			if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
			{
				childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
			}
			
			ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
		}
	}
}
