
using System;
using UnityEngine;

public class SignalsCombiner : MonoBehaviour, ISignal
{
    #region public ISignal[] signals { get; set; }

    [SerializeField, EnsureInterfaceImplementation(typeof(ISignal))]
    private GameObject[] m_signalsGameObjects;

    private ISignal[] m_signals;
    public ISignal[] signals
    {
        get
        {
            if (m_signals == null && m_signalsGameObjects != null)
            {
                m_signals = new ISignal[m_signalsGameObjects.Length];
                for (int i = 0; i < m_signalsGameObjects.Length; i++)
                {
                    var go = m_signalsGameObjects[i];
                    m_signals[i] = go == null ? null : go.GetComponent<ISignal>();
                }
            }
            return m_signals;
        }
        set { m_signals = value; m_signalsGameObjects = null; }
    }

    #endregion

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
        foreach (var s in signals)
        {
            s.onStateChanged += OnStateChanged;
        }
    }

    private void OnStateChanged(bool value)
    {
        foreach (var s in signals)
        {
            if (!s.state)
            {
                state = false;
                return;
            }
        }

        state = true;
    }
}