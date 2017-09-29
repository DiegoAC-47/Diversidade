using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TextSizeFixer : MonoBehaviour
{
    public void Fixer(Image dialogUI, string textComplete)
    {
        if (dialogUI.transform == null)
            print("parent");

        Text copy = Instantiate(dialogUI.GetComponentInChildren<Text>(), dialogUI.transform);

        copy.color = new Color(dialogUI.color.r, dialogUI.color.g, dialogUI.color.b, 0);
        copy.resizeTextMaxSize = 999;
        copy.text = textComplete;
        copy.resizeTextForBestFit = true;
        StartCoroutine(WaitFrame(dialogUI.GetComponentInChildren<Text>(), copy));
    }

    private IEnumerator WaitFrame(Text textOriginal, Text copy)
    {
        yield return 0;

        int size = 0;

        size = copy.cachedTextGenerator.fontSizeUsedForBestFit;

        Destroy(copy.gameObject);
        textOriginal.resizeTextForBestFit = false;
        textOriginal.fontSize = size;
    }
}
