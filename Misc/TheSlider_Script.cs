using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TheSlider_Script : MonoBehaviour {

	public Slider TheSlider;

	[Tooltip("How fast the Slider bar should replenish. (Higher number replenishes faster")]
	public float m_RecoverRate;

	[Tooltip("How fast the Slider bar should deplete. (Higher number depletes faster")]
	public float m_DamageRate;

	private bool m_IsRecovering;

	// Use this for initialization
	void Start () {

		// Check to make sure that a Slider object has been assigned
		if (TheSlider == null)
			Debug.Log("No mana TheSlider found");
		else
			Debug.Log("Mana TheSlider found, name is: " + TheSlider.name);

		// Have the slider start out at the max value that you've assigned for the Slider Component
		TheSlider.value = TheSlider.maxValue;

		m_IsRecovering = false;
		
	}
	
	// Update is called once per frame
	void Update () {

		ControlSlider();
		
	}


	public void ControlSlider()
	{
		// Conditonal that will trigger the decrease of the slider image and value
		if (Input.GetKey(KeyCode.L))
		{
			m_IsRecovering = false;
			Debug.Log("Is Recovering equals: " + m_IsRecovering);
			TheSlider.value -= m_DamageRate * Time.deltaTime;
		}

		// Conditional to let the Slider start recovering
		if (Input.GetKeyUp(KeyCode.L))
		{
			m_IsRecovering = true;
		}


        if (m_IsRecovering)
            TheSlider.value += m_RecoverRate * Time.deltaTime; ;
	}
}
