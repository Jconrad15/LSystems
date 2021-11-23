using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliderHandleText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI iterationText;
    [SerializeField]
    private TextMeshProUGUI angleRotationText;
    [SerializeField]
    private TextMeshProUGUI lineLengthText;
    [SerializeField]
    private TextMeshProUGUI lineWidthText;

    public void SetIterationText(System.Single newIterationText)
    {
        iterationText.SetText(newIterationText.ToString());
    }

    public void SetAngleRotationText(System.Single newAngleRotationText)
    {
        angleRotationText.SetText(newAngleRotationText.ToString());
    }

    public void SetLineLengthText(System.Single newLineLengthText)
    {
        lineLengthText.SetText(newLineLengthText.ToString("F1"));
    }

    public void SetLineWidthText(System.Single newLineWidthText)
    {
        lineWidthText.SetText(newLineWidthText.ToString("F1"));
    }

}
