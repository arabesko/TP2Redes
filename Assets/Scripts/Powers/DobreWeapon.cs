using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DobreWeapon : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority) return;

        if (other.TryGetComponent(out PlayerShooting playerShooting))
        {
            playerShooting.ActivatePower();
            GameManager.Instance.PowerCollected();
        }
    }
}
