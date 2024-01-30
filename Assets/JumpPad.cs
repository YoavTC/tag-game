using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float boostStrength;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController playerController) && playerController.canBoost)
        {
            playerController.BoostPlayer(boostStrength);
            StartCoroutine(BoostCooldown(playerController));
        }
    }

    private IEnumerator BoostCooldown(PlayerController playerController)
    {
        playerController.canBoost = false;
        yield return HelperFunctions.GetWait(0.5f);
        playerController.canBoost = true;
    }
}
