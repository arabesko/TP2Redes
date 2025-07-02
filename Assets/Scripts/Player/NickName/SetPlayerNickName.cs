using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerNickName : NetworkBehaviour
{
    [Networked] private NetworkString<_16> CurrentNickName { get; set; }

    private ChangeDetector _changeDetector;

    public event Action OnLeft;

    private NickNameItem _myNickNameItem;

    public override void Spawned()
    {

        _myNickNameItem = NickNameHandler.Instance.CreateNewNickName(this);

        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        if (HasInputAuthority)
        {
            NetworkString<_16> loadedNickName;
            if (PlayerPrefs.HasKey("NickName"))
            {
                loadedNickName = PlayerPrefs.GetString("NickName");
            }
            else
            {
                loadedNickName = $"Player {Runner.LocalPlayer.PlayerId}";
            }
            RPC_SendNickName(loadedNickName);
        }
        else
        {
            UpdateNickName();
        }
    }

    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(CurrentNickName):
                {
                     UpdateNickName();
                     break;
                }
            }
        }
    }

    void UpdateNickName()
    {
        _myNickNameItem.UpdateText(CurrentNickName.Value);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_SendNickName(NetworkString<_16> nickName)
    {
        CurrentNickName = nickName;
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnLeft?.Invoke();
    }
}
