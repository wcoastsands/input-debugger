using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Nikkolai
{
    public class InputDebugger : MonoBehaviour
    {
        [System.Serializable]
        public class OnAxisInput : UnityEvent<string, float> { }
        [System.Serializable]
        public class OnJoystickButtonDown : UnityEvent<int> { }
        [System.Serializable]
        public class OnJoystickButtonUp : UnityEvent<int> { }

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
        public bool logAxisInputs;
        [Tooltip("Logs input on joystick button down.")]
        public bool logJoystickButtonDown;
        [Tooltip("Logs input on joystick button up.")]
        public bool logJoystickButtonUp;

        [Space]
        public OnAxisInput onAxisInput;
        public OnJoystickButtonDown onJoystickButtonDown;
        public OnJoystickButtonUp onJoystickButtonUp;

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
                    m_AxisNames.Insert(0, axisNames[i]);
                }
            }
        }

        void Update ()
        {
            DebugAxisInputs();
            DebugJoystickButtons();
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

        void DebugAxisInputs ()
        {
            // Loop in reverse to avoid errors when removing invalid names.
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
                    if (logAxisInputs)
                    {
                        Debug.Log(string.Format("\"{0}\" is active ({1}).", m_AxisNames[i], m_AxisValue));
                    }

                    if (onAxisInput != null)
                    {
                        onAxisInput.Invoke(m_AxisNames[i], m_AxisValue);
                    }
                }
            }
        }

        void DebugJoystickButtons ()
        {
            m_ButtonIndex = 0;

            for (int i = (int)KeyCode.JoystickButton0; i <= (int)KeyCode.JoystickButton19; i++)
            {
                if ((logJoystickButtonDown || onJoystickButtonDown != null) && Input.GetKeyDown((KeyCode)i))
                {
                    if (logJoystickButtonDown)
                    {
                        Debug.Log(string.Format("Gamepad button {0} pressed.", m_ButtonIndex));
                    }

                    if (onJoystickButtonDown != null)
                    {
                        onJoystickButtonDown.Invoke(m_ButtonIndex);
                    }
                }

                if ((logJoystickButtonUp || onJoystickButtonUp != null) && Input.GetKeyUp((KeyCode)i))
                {
                    if (logJoystickButtonUp)
                    {
                        Debug.Log(string.Format("Gamepad button {0} released.", m_ButtonIndex));
                    }

                    if (onJoystickButtonUp != null)
                    {
                        onJoystickButtonUp.Invoke(m_ButtonIndex);
                    }
                }

                m_ButtonIndex++;
            }
        }
    }
}
