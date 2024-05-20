using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class IntroCanvas : Singleton<IntroCanvas>
{
    [Button]
    public void StartIntro()
    {
        Debug.Log("Starting intro");
        transform.GetComponent<Image>().DOFade(0, 2f).SetDelay(1f).OnComplete(() =>
        {
            Destroy(transform.parent.gameObject);
        });
    }
}
