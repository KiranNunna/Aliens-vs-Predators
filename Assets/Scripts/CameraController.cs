using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 10f;
	[SerializeField] private float rotationSpeed = 100f;
	
	[SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
	[SerializeField] private float zoomAmount = 1f;
	[SerializeField] private float zoomSpeed = 5f;
	
	private const float MIN_FOLLOW_Y_OFFSET = 2f;
	private const float MAX_FOLLOW_Y_OFFSET = 12f;
	
	private CinemachineTransposer cinemachineTransposer;
	private Vector3 targetFollowOffset;
	
	// Start is called before the first frame update
	void Start()
	{
		cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
		targetFollowOffset = cinemachineTransposer.m_FollowOffset;
	}

	// Update is called once per frame
	void Update()
	{
		HandleMovement();
		HandleRotation();
		HandleZoom();
	}
	
	private void HandleMovement()
	{
		Vector3 inputMoveDir = new Vector3(0, 0, 0);
		if(Input.GetKey(KeyCode.W))
		{
			inputMoveDir.z = +1f;
		}
		if(Input.GetKey(KeyCode.S))
		{
			inputMoveDir.z = -1f;
		}
		if(Input.GetKey(KeyCode.A))
		{
			inputMoveDir.x = -1f;
		}
		if(Input.GetKey(KeyCode.D))
		{
			inputMoveDir.x = +1f;
		}
		
		Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
		transform.position += moveVector * moveSpeed * Time.deltaTime;
	}
	
	private void HandleRotation()
	{
		Vector3 rotationVector = new Vector3(0, 0, 0);
		if(Input.GetKey(KeyCode.Q))
		{
			rotationVector.y = +1f;
		}
		if(Input.GetKey(KeyCode.E))
		{
			rotationVector.y = -1f;
		}
		
		transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
		
	}
	
	private void HandleZoom()
	{
		// By looking at the source code of Cinemachine package, we have found out a way to manipulate camera offset.
		if(Input.mouseScrollDelta.y > 0)
		{
			targetFollowOffset.y -= zoomAmount;
		}
		if(Input.mouseScrollDelta.y < 0)
		{
			targetFollowOffset.y += zoomAmount;
		}
		targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
		cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, zoomSpeed * Time.deltaTime);
	}
}
