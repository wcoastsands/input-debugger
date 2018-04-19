using System.Collections.Generic;
using UnityEngine;

namespace Nikkolai
{
    public class InputDebugger : MonoBehaviour
    {
        [Tooltip("Axis names found in the Input Manager.")]
        public string[] axisNames = {
            "Horizontal",
            "Vertical",
            "Look X",
            "Look Y",
            "Joystick Axis 5",
            "Joystick Axis 6",
            "Joystick Axis 7",
            "Joystick Axis 8",
            "Joystick Axis 9",
            "Joystick Axis 10",
        };
        [Tooltip("Logs input values of axis names listed above.")]
        public bool debugAxes;
        [Tooltip("Logs input of joystick buttons 0 through 19.")]
        public bool debugButtons;

        string[] m_JoystickNames;
        float m_AxisValue = 0f;
        int m_ButtonIndex = 0;

        // Validated list of axis names.
        readonly List<string> m_AxisNames = new List<string>();

        void Start ()
        {
            // Only log joystick names from editor and development builds.
            if (Debug.isDebugBuild)
            {
                LogJoystickNames();
            }
        }

        void OnEnable ()
        {
            // Only debug inputs from editor and development builds.
            enabled = Debug.isDebugBuild;

            if (enabled)
            {
                m_AxisNames.Clear();

                for (int i = 0; i < axisNames.Length; i++)
                {
                    m_AxisNames.Add(axisNames[i]);
                }
            }
        }

        void Update ()
        {
            if (debugAxes)
            {
                DebugAxes();
            }

            if (debugButtons && Input.anyKeyDown)
            {
                DebugButtons();
            }
        }

        void LogJoystickNames ()
        {
            m_JoystickNames = Input.GetJoystickNames();

            var message = m_JoystickNames.Length > 0 ?
                string.Format("Found {0} joysticks:", m_JoystickNames.Length) :
                "No joysticks found.";

            foreach (string joystickName in m_JoystickNames)
            {
                message += string.Format(" {0},", joystickName);
            }

            Debug.Log(message.TrimEnd(','));
        }

        void DebugAxes ()
        {
            // Loop in reverse to avoid errors after the removal of invalid names.
            for (int i = m_AxisNames.Count - 1; i >= 0; i--)
            {
                try
                {
                    m_AxisValue = Input.GetAxis(m_AxisNames[i]);
                }
                catch
                {
                    Debug.LogWarningFormat("Input Manager does not contain an axis named \"{0}\".", m_AxisNames[i]);
                    m_AxisNames.RemoveAt(i);
                    continue;
                }

                if (m_AxisValue != 0f)
                {
                    Debug.Log(string.Format("\"{0}\" is active ({1}).", m_AxisNames[i], m_AxisValue));
                }
            }
        }

        void DebugButtons ()
        {
            m_ButtonIndex = 0;

            // For KeyCode values JoystickButton0 through JoystickButton19...
            for (int i = 330; i <= 349; i++)
            {
                if (Input.GetKeyDown((KeyCode)i))
                {
                    Debug.Log(string.Format("Gamepad button {0} pressed.", m_ButtonIndex));
                }

                m_ButtonIndex++;
            }
        }
    }
}
