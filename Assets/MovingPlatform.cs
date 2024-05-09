using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovingPlatform : Singleton<MovingPlatform>
{
    [SerializeField] private Transform positionB;
    [SerializeField] private float platformSpeed;
    [SerializeField] private Ease easeType;

    public void StartMoving()
    {
        transform.DOMove(positionB.position, platformSpeed).SetEase(easeType).SetLoops(-1, LoopType.Yoyo);
    }
}
