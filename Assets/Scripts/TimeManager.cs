using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	[SerializeField] private float slowdownFactor = 0.05f;
	[SerializeField] private float slowdownLength = 2f;
	
	private float timer;
	private float defaultFixedDeltaTime;
	
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
		
		timer -= Time.unscaledDeltaTime;
		Debug.Log(timer);
		
		if(timer <= 0)
		{
			Time.timeScale = 1;
			Time.fixedDeltaTime = defaultFixedDeltaTime;
			this.enabled = false;
		}
	}
	
	public void DoSlowMotion()
	{
		Time.timeScale = slowdownFactor;
		Time.fixedDeltaTime = Time.timeScale * 0.02f;
	}
}
