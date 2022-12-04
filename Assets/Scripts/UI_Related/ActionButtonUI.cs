using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textMeshPro;
	[SerializeField] private Button button;
	[SerializeField] private GameObject selectedOutlineGameObject;
	
	private BaseAction baseAction;
	
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public void SetBaseAction(BaseAction baseAction)
	{
		this.baseAction = baseAction;
		textMeshPro.SetText(baseAction.GetActionName().ToUpper()); 

		// This is an anonymous function (lambda expression)
		button.onClick.AddListener(() => 
		{
			UnitActionSystem.Instance.SetSelectedAction(baseAction);
		});
	}
	
	public void UpdateSelectedVisual()
	{
		BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
		selectedOutlineGameObject.SetActive(selectedBaseAction == baseAction);
	}
}
