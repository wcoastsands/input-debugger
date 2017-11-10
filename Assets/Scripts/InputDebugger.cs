using UnityEngine;
using System.Collections.Generic;

public class InputDebugger : MonoBehaviour
{
	private string[] _joystickNames = new string[0];
	private JoystickDebugger[] _joystickDebuggers = new JoystickDebugger[0];

	void Start ()
	{
		UpdateJoysticks();
	}

	void Update ()
	{
		if (_joystickNames.Length != Input.GetJoystickNames().Length)
		{
			UpdateJoysticks();
		}
	}

	private void UpdateJoysticks ()
	{
		List<JoystickDebugger> debuggers = new List<JoystickDebugger>();

		_joystickNames = Input.GetJoystickNames();

		Debug.Log("Number of joysticks attached: " + _joystickNames.Length);

		foreach (JoystickDebugger debugger in _joystickDebuggers)
		{
			Destroy(debugger.gameObject);
		}

		for (int i = 0; i < _joystickNames.Length; i++)
		{
			if (string.IsNullOrEmpty(_joystickNames[i]))
				continue;

			JoystickDebugger debugger;
			GameObject gO = new GameObject();
			gO.transform.parent = transform;
			gO.name = string.Format("{0} {1}", i, _joystickNames[i]);

			switch (_joystickNames[i])
			{
				case "Motion Controller":
					debugger = (JoystickDebugger)gO.AddComponent<PSMoveDebugger>();
					break;
				default:
					debugger = gO.AddComponent<JoystickDebugger>();
					break;
			}

			debugger.index = i;
			debuggers.Add(debugger);
		}

		_joystickDebuggers = debuggers.ToArray();
	}
}
