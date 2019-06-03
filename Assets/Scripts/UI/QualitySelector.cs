
using UnityEngine;
using UnityEngine.UI;

public class QualitySelector : MonoBehaviour {

    private RadioGroup m_radioGroup;
    private RadioGroup radioGroup { get { if (m_radioGroup == null) m_radioGroup = GetComponent<RadioGroup>(); return m_radioGroup; } }

    private Dropdown m_dropdown;
    private Dropdown dropdown { get { if (m_dropdown == null) m_dropdown = GetComponent<Dropdown>(); return m_dropdown; } }

    private void Start()
    {
        if (radioGroup != null)
        {
            radioGroup.Select(QualitySettings.GetQualityLevel());
            radioGroup.onSelected.AddListener(QualitySettings.SetQualityLevel);
        }

        if (dropdown != null)
        {
            dropdown.value = QualitySettings.GetQualityLevel();
            dropdown.onValueChanged.AddListener(QualitySettings.SetQualityLevel);
        }
    }
}