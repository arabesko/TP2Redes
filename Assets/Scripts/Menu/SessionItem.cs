using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _playerCount;
    [SerializeField] private Button _joinButtom;

    public event Action<SessionInfo> OnJoinSession;
    public void Initialize(SessionInfo sessioninfo)
    {
        _name.text = sessioninfo.Name;
        _playerCount.text = $"{sessioninfo.PlayerCount} / {sessioninfo.MaxPlayers}";

        if(sessioninfo.PlayerCount < sessioninfo.MaxPlayers)

        _joinButtom.onClick.AddListener(() =>
        {
            OnJoinSession?.Invoke(sessioninfo);
        });
    }
}
