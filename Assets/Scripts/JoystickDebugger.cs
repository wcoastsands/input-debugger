using UnityEngine;
using System.Collections;

public class JoystickDebugger : MonoBehaviour
{
	public int index { get; set; }

	void Update ()
	{
		if (Input.anyKeyDown)
		{
			LogAnyKeyDown();
		}
	}

	private void LogAnyKeyDown ()
	{
		for (int i = 0; i < 20; i++)
		{
			string joystickButton = string.Format("joystick {0} button {0}", index, i);

			if (Input.GetKeyDown(joystickButton))
			{
				Debug.Log("Pressed " + joystickButton);
			}
		}
	}
}
