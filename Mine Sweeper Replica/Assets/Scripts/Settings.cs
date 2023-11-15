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
        InitializeSliders();

        xSlider.onValueChanged.AddListener((value) =>
        {
            mineSlider.maxValue = xSlider.value * ySlider.value - 1;
            xTxt.text = value.ToString();
        });
        ySlider.onValueChanged.AddListener((value) =>
        {
            mineSlider.maxValue = xSlider.value * ySlider.value - 1;
            yTxt.text = value.ToString();
        });
        mineSlider.onValueChanged.AddListener((value) =>
        {
            mineTxt.text = value.ToString();
        });
    }

    private void InitializeSliders()
    {
        xSlider.value = PlayerPrefs.GetInt("sizeX", 10);
        xTxt.text = xSlider.value.ToString();
        ySlider.value = PlayerPrefs.GetInt("sizeY", 10);
        yTxt.text = ySlider.value.ToString();
        mineSlider.maxValue = xSlider.value * ySlider.value - 1;
        mineSlider.value = PlayerPrefs.GetInt("mineCount", 10);
        mineTxt.text = mineSlider.value.ToString();
    }

    public void ChangeSettings()
    {
        PlayerPrefs.SetInt("sizeX", (int)xSlider.value);
        PlayerPrefs.SetInt("sizeY", (int)ySlider.value);
        PlayerPrefs.SetInt("mineCount", (int)mineSlider.value);
    }
}
