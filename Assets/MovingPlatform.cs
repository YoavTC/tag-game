using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Vector3 positionB;
    [SerializeField] private float platformSpeed;
    [SerializeField] private Ease easeType;

    private void Start()
    {
        positionB = transform.GetChild(0).transform.position;
        transform.DOMove(positionB, platformSpeed).SetEase(easeType).SetLoops(-1, LoopType.Yoyo);
    }
}
