using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class TitleSystem : Singleton<TitleSystem>
{
    [SerializeField] float fadeDuration;

    [Button]
    public void TestTitle()
    {
        DisplayText("testing!!!", true, "#B4E14C");
    }
    
    public void DisplayText(string text, bool fade, string color = "#FFFFFF")
    {
        TMP_Text newText = gameObject.GetComponent<TMP_Text>();

        newText.enabled = false;
        newText.alignment = TextAlignmentOptions.Center;
        newText.verticalAlignment = VerticalAlignmentOptions.Middle;
        newText.enableAutoSizing = true;
        newText.fontStyle = FontStyles.Bold;
        // GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 

        newText.color = HelperFunctions.HexToColor(color);
        newText.text = text;
        

        if (fade)
        {
            newText.DOFade(0, 0f);
            newText.enabled = true;
            newText.DOFade(1, fadeDuration).SetEase(Ease.InOutSine).OnComplete(() => 
                    newText.DOFade(0, fadeDuration * 1.5f).SetDelay(1.5f));
        }
        else
        {
            newText.enabled = true;
            StartCoroutine(RemoveText(newText));
        }
    }

    private IEnumerator RemoveText(TMP_Text text)
    {
        yield return HelperFunctions.GetWait(2f);
        text.enabled = false;
    }
}
