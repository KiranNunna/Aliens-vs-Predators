using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	[SerializeField] private bool invert;
	private Transform cameraTransform;
	
	void Awake()
	{
		cameraTransform = Camera.main.transform;
	}
	
	void LateUpdate()
	{
		if(invert)
		{
			Vector3 dirToCamera = (cameraTransform.position - transform.position).normalized;
			this.transform.LookAt(transform.position + dirToCamera*-1);
		}
		else
		{
			this.transform.LookAt(cameraTransform);
		}
	}
}
