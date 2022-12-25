using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Button btn;
    public bool revealed;

    public void Reveal()
    {

    }

    public void OnClick()
    {
        Reveal();
        btn.interactable = false;
    }
}
