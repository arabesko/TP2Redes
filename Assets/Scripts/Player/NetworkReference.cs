using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LocalInput))]
public class NetworkReference : NetworkBehaviour
{
    public static NetworkReference Local {  get; private set; }

    public LocalInput localInputs {  get; private set; }

    public override void Spawned()
    {
        localInputs = GetComponent<LocalInput>();

        if (Object.HasInputAuthority)
        {
            Local = this;
            localInputs.enabled = true;
        }
        else
        {
            localInputs.enabled = false;
        }
    }
}
