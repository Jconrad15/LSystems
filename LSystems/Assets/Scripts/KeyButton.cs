using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyButton : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup keyCG;

    private bool isShown = false;

    // Start is called before the first frame update
    void Start()
    {
        HideKey();
    }

    public void HideKey()
    {
        keyCG.alpha = 0;
        keyCG.blocksRaycasts = false;
        keyCG.interactable = false;
        isShown = false;
    }

    public void ShowKey()
    {
        keyCG.alpha = 1;
        keyCG.blocksRaycasts = true;
        keyCG.interactable = true;
        isShown = true;
    }

    public void SwitchState()
    {
        if (isShown == true)
        {
            HideKey();
        }
        else
        {
            ShowKey();
        }
    }

}
