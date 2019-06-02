
using UnityEngine;
using UnityEngine.Events;

public class SignalUnityEvents : MonoBehaviour
{
    #region public ISignal signal { get; set; }

    [SerializeField, EnsureInterfaceImplementation(typeof(ISignal), "signal")]
    private GameObject m_signalGameObject;

    private ISignal m_signal;
    public ISignal signal
    {
        get { if (m_signal == null && m_signalGameObject != null) m_signal = m_signalGameObject.GetComponent<ISignal>(); return m_signal; }
        set { m_signal = value; m_signalGameObject = null; }
    }

    #endregion

    public UnityEvent onActivated;
    public UnityEvent onDeactivated;
    public BoolUnityEvent onActivationChanged;

    private void Awake()
    {
        signal.onStateChanged += OnSignalChanged;
    }

    private void Reset()
    {
        m_signalGameObject = (GetComponentInChildren<ISignal>(true) as Component)?.gameObject;
    }

    private void OnSignalChanged(bool value)
    {
        if (value) onActivated.Invoke(); else onDeactivated.Invoke();
        onActivationChanged.Invoke(value);
    }
}