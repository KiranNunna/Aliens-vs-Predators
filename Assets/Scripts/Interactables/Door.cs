using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
	[SerializeField] private bool isOpen;
	
	private GridPosition gridPosition;
	private Animator animator;
	
	private bool isActive;
	private Action onInteractComplete;
	private float timer;
	
	void Awake()
	{
		animator = GetComponent<Animator>();
	}
	
	void Start()
	{
		gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
		LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

		// Handling default
		if(isOpen)
		{
			OpenDoor();
		}
		else
		{
			CloseDoor();
		}
	}
	
	void Update()
	{
		if(!isActive)
		{
			return;
		}
		
		timer -= Time.deltaTime;
		if(timer <= 0f)
		{
			isActive = false;
			onInteractComplete();
		}
	}
	
	public void Interact(Action onInteractComplete)
	{
		this.onInteractComplete = onInteractComplete;
		isActive = true;
		timer = 0.5f;
		
		if(isOpen)
		{
			CloseDoor();
		}
		else
		{
			OpenDoor();
		}
	}
	
	private void OpenDoor()
	{
		isOpen = true;
		animator.SetBool("IsOpen", isOpen);
		PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
	}
	
	private void CloseDoor()
	{
		isOpen = false;
		animator.SetBool("IsOpen", isOpen);
		PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
	}
}
