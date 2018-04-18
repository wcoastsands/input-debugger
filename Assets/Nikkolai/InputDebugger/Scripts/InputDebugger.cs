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

        readonly List<string> m_InvalidAxisNames = new List<string>();

        void Start ()
        {
            // Only log joystick names from editor and development builds.
            if (Debug.isDebugBuild) LogJoystickNames();
        }

        void OnEnable ()
        {
            // Only debug inputs from editor and development builds.
            enabled = Debug.isDebugBuild;
        }

        void OnDisable ()
        {
            m_InvalidAxisNames.Clear();
        }

        void Update ()
        {
            if (debugAxes)
            {
                for (int i = 0; i < axisNames.Length; i++)
                {
                    if (string.IsNullOrEmpty(axisNames[i]) || m_InvalidAxisNames.Contains(axisNames[i]))
                    {
                        continue;
                    }

                    try
                    {
                        m_AxisValue = Input.GetAxis(axisNames[i]);
                    }
                    catch
                    {
                        Debug.LogWarningFormat("Input Manager does not contain an axis named \"{0}\".", axisNames[i]);
                        m_InvalidAxisNames.Add(axisNames[i]);
                        continue;
                    }

                    if (m_AxisValue != 0f)
                    {
                        Debug.Log(string.Format("\"{0}\" is active ({1}).", axisNames[i], m_AxisValue));
                    }
                }
            }

            if (debugButtons && Input.anyKeyDown)
            {
                m_ButtonIndex = 0;

                for (int i = 330; i <= 349; i++) // JoystickButton0 through JoystickButton19
                {
                    if (Input.GetKeyDown((KeyCode)i))
                    {
                        Debug.Log(string.Format("Gamepad button {0} pressed.", m_ButtonIndex));
                    }

                    m_ButtonIndex++;
                }
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
    }
}
