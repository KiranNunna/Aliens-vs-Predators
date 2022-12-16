using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
	// [SerializeField] private Transform gridDebugObjectPrefab;
	// private GridSystem gridSystem;
	
	// // Start is called before the first frame update
	// void Start()
	// {
	// 	gridSystem = new GridSystem(10, 10, 2);
	// 	gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
		
	// 	Debug.Log(new GridPosition(5, 7));
	// }
	
	// void Update()
	// {
	// 	// Debug.Log(gridSystem.GetGridPosition(MouseWorld.GetPosition()));

	// 	if (Input.GetKeyDown(KeyCode.T))
	// 	{
	// 		GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
	// 		GridPosition startGridPosition = new GridPosition(0, 0);
			
	// 		List<GridPosition> gridPositionList = PathFinding.Instance.FindPath(startGridPosition, mouseGridPosition);
		
	// 		for(int i = 0; i < gridPositionList.Count; i++)
	// 		{
	// 			Debug.DrawLine(LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
	// 						   LevelGrid.Instance.GetWorldPosition(gridPositionList[Mathf.Min(i+1, gridPositionList.Count-1)]),
	// 						   Color.white, 10f);
	// 		}
			
	// 	}
	// }
	
	// [SerializeField] private Unit unit;
	
	// void Update()
	// {
	// 	if(Input.GetKeyDown(KeyCode.T))
	// 	{
	// 		GridSystemVisual.Instance.HideAllGridPosition();
	// 		GridSystemVisual.Instance.ShowGridPositionList(
	// 			unit.GetMoveAction().GetValidActionGridPositionList()
	// 		);
	// 	}
	// }
}
