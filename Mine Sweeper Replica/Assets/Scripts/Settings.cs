using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public Slider xSlider;
    public TMP_Text xTxt;
    public Slider ySlider;
    public TMP_Text yTxt;
    public Slider mineSlider;
    public TMP_Text mineTxt;

    private void Start()
    {
        ChangeSettings();

        xSlider.onValueChanged.AddListener((value) =>
        {
            mineSlider.maxValue = PlayerPrefs.sizeX * PlayerPrefs.sizeY;
            xTxt.text = value.ToString();
        });
        ySlider.onValueChanged.AddListener((value) =>
        {
            mineSlider.maxValue = PlayerPrefs.sizeX * PlayerPrefs.sizeY;
            yTxt.text = value.ToString();
        });
        mineSlider.onValueChanged.AddListener((value) =>
        {
            mineTxt.text = value.ToString();
        });
    }

    public void ChangeSettings()
    {
        PlayerPrefs.sizeX = (int)xSlider.value;
        PlayerPrefs.sizeY = (int)ySlider.value;
        PlayerPrefs.mineCount = (int)mineSlider.value;
    }
}
