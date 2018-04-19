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
        public class OnInputButtonDown : UnityEvent<string> { }
        [System.Serializable]
        public class OnInputButtonUp : UnityEvent<string> { }
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
            "Joystick Axis 10"
        };
        [Tooltip("Button names found in the Input Manager.")]
        public string[] buttonNames = {
            "Jump",
            "Crouch",
            "Sprint"
        };
        [Tooltip("Logs input values for axis names listed above.")]
        public bool logInputAxisValues;
        [Tooltip("Logs button presses for button names listed above.")]
        public bool logInputButtonDown;
        [Tooltip("Logs button releases for button names listed above.")]
        public bool logInputButtonUp;
        [Tooltip("Logs joystick button presses.")]
        public bool logJoystickButtonDown;
        [Tooltip("Logs joystick button releases.")]
        public bool logJoystickButtonUp;

        [Space]
        public OnAxisInput onAxisInput;
        public OnInputButtonDown onInputButtonDown;
        public OnInputButtonUp onInputButtonUp;
        public OnJoystickButtonDown onJoystickButtonDown;
        public OnJoystickButtonUp onJoystickButtonUp;

        string[] m_JoystickNames;
        float m_AxisValue = 0f;
        int m_ButtonIndex = 0;

        // Validated list of axis names.
        readonly List<string> m_AxisNames = new List<string>();
        // Validated list of button names.
        readonly List<string> m_ButtonNames = new List<string>();

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

                for (int i = 0; i < buttonNames.Length; i++)
                {
                    m_ButtonNames.Insert(0, buttonNames[i]);
                }
            }
        }

        void Update ()
        {
            DebugInputAxes();
            DebugInputButtons();
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

        void DebugInputAxes ()
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
                    if (logInputAxisValues)
                    {
                        Debug.Log(string.Format("Input axis \"{0}\" is active ({1}).", m_AxisNames[i], m_AxisValue));
                    }

                    if (onAxisInput != null)
                    {
                        onAxisInput.Invoke(m_AxisNames[i], m_AxisValue);
                    }
                }
            }
        }

        void DebugInputButtons ()
        {
            // Loop in reverse to avoid errors when removing invalid names.
            for (int i = m_ButtonNames.Count - 1; i >= 0; i--)
            {
                try
                {
                    if ((logInputButtonDown || onInputButtonDown != null) && Input.GetButtonDown(m_ButtonNames[i]))
                    {
                        if (logInputButtonDown)
                        {
                            Debug.Log(string.Format("Input button \"{0}\" pressed.", m_ButtonNames[i]));
                        }

                        if (onInputButtonDown != null)
                        {
                            onInputButtonDown.Invoke(m_ButtonNames[i]);
                        }
                    }

                    if ((logInputButtonUp || onInputButtonUp != null) && Input.GetButtonUp(m_ButtonNames[i]))
                    {
                        if (logInputButtonUp)
                        {
                            Debug.Log(string.Format("Input button \"{0}\" released.", m_ButtonNames[i]));
                        }

                        if (onInputButtonUp != null)
                        {
                            onInputButtonUp.Invoke(m_ButtonNames[i]);
                        }
                    }
                }
                catch
                {
                    Debug.LogWarningFormat("Input Manager does not contain a button named \"{0}\".", m_ButtonNames[i]);
                    m_ButtonNames.RemoveAt(i);
                    continue;
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
