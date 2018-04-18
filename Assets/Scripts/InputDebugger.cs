using UnityEngine;

namespace Nikkolai
{
    public class InputDebugger : MonoBehaviour
    {
        [Tooltip("Logs input of joystick axes 1 through 10.")]
        public bool debugAxes;
        [Tooltip("Logs input of joystick buttons 0 through 19.")]
        public bool debugButtons;

        static readonly int k_FirstAxis = 1;
        static readonly int k_LastAxis = 10;
        static readonly string k_AxisErrorMessage = string.Format(
            "Input Manager must contain axis inputs for \"Joystick Axis {0}\" through \"Joystick Axis {1}\" before debugging axes.",
            k_FirstAxis,
            k_LastAxis
        );

        string[] m_JoystickNames;
        float m_AxisValue = 0f;
        int m_ButtonIndex = 0;

        void Start ()
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

        void Update ()
        {
            if (debugAxes)
            {
                for (int i = 1; i <= 10; i++)
                {
                    try
                    {
                        m_AxisValue = Input.GetAxis(string.Format("Joystick Axis {0}", i));
                    }
                    catch
                    {
                        Debug.LogErrorFormat("Axis name \"Joystick Axis {0}\" is invalid. {1}", i, k_AxisErrorMessage);
                        debugAxes = false;
                        break;
                    }

                    if (m_AxisValue != 0f)
                    {
                        Debug.Log(string.Format("Gamepad axis {0} is active ({1}).", i, m_AxisValue));
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
    }
}
