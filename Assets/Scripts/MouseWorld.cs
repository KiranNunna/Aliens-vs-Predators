using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
	private static MouseWorld Instance;
	[SerializeField] private LayerMask mousePlaneLayerMask;  
	
	void Awake()
	{
		Instance = this;
	}
	
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		// transform.position = MouseWorld.GetPosition();
	}
	
	public static Vector3 GetPosition()
	{
		Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
		Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, Instance.mousePlaneLayerMask);
		return raycastHit.point;
	}
}
