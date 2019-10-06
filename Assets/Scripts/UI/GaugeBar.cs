using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class GaugeBar : MonoBehaviour
{
    [SerializeField] private Image barImage;

    [SerializeField] private FloatReference currentAmt;
    [SerializeField] private FloatReference maxAmt;

    [SerializeField] private Gradient colorGradient;


    // Update is called once per frame
    void Update()
    {
        barImage.fillAmount = currentAmt.Value / maxAmt.Value;
        barImage.color = colorGradient.Evaluate(barImage.fillAmount);
    }
}
