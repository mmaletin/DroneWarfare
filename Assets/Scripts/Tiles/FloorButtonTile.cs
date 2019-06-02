
using System;
using UnityEngine;

public class FloorButtonTile : MonoBehaviour, ITile, ISignal
{
    public Transform button;

    private Vector3 buttonPos;

    #region ISignal implementation

    private bool m_state;
    public bool state
    {
        get { return m_state; }
        set
        {
            bool changed = m_state != value;
            m_state = value;
            if (changed && onStateChanged != null) onStateChanged(m_state);
        }
    }

    public event Action<bool> onStateChanged;

    #endregion

    private void Start()
    {
        buttonPos = button.localPosition;
    }

    public void Activate(PlayerRobot playerRobot)
    {
        button.localPosition = Vector3.zero;

        state = true;
    }

    public void Deactivate(PlayerRobot playerRobot)
    {
        button.localPosition = buttonPos;

        state = false;
    }
}