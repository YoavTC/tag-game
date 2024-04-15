using System;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class TitleSystem : Singleton<TitleSystem>
{
    [SerializeField] float fadeDuration;
    private bool isLocalGame;

    private TMP_Text titleText;

    private void Start()
    {
        isLocalGame = GameManager.Instance.isLocalGame;
        titleText = GetComponent<TMP_Text>();
    }

    [Button]
    public void TestTitle()
    {
        DisplayText("testing!!!", true, "#B4E14C");
    }
    
    public void DisplayText(string text, bool fade, string color = "#FFFFFF", bool local = false)
    {
        //Reset
        titleText.DOKill();
        
        titleText.enabled = false;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.verticalAlignment = VerticalAlignmentOptions.Middle;
        titleText.enableAutoSizing = true;
        titleText.fontStyle = FontStyles.Bold;
        // GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 

        titleText.color = HelperFunctions.HexToColor(color);
        titleText.text = text;
        

        if (fade)
        {
            titleText.DOFade(0, 0f);
            titleText.enabled = true;
            titleText.DOFade(1, fadeDuration).SetEase(Ease.InOutSine).OnComplete(() => 
                    titleText.DOFade(0, fadeDuration * 1.5f).SetDelay(1.5f));
        }
        else
        {
            titleText.enabled = true;
            StartCoroutine(RemoveText(titleText));
        }
    }

    private IEnumerator RemoveText(TMP_Text text)
    {
        yield return HelperFunctions.GetWait(2f);
        text.enabled = false;
    }
}
