using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrowserHandler : MonoBehaviour
{
    [SerializeField] private SessionItem _sessionItemPrefab;
    [SerializeField] private NetworkRunnerHandler _runnerHandler;
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private VerticalLayoutGroup _verticalLayout;


    private void OnEnable()
    {
        _runnerHandler.OnSessionListUpdate += ReceiveSessionList;

    }

    private void OnDisable()
    {
        _runnerHandler.OnSessionListUpdate -= ReceiveSessionList;
    }

    void ReceiveSessionList(List<SessionInfo> sessionList)
    {
        ClearBrowser();
        if (sessionList.Count == 0)
        {
            _statusText.gameObject.SetActive(true);
            return;
        }

        foreach (var sessionInfo in sessionList)
        {
            AddToSessionBrowser(sessionInfo);
        }
    }

    void AddToSessionBrowser(SessionInfo sessionInfo)
    {
        var newSessionItem = Instantiate(_sessionItemPrefab, _verticalLayout.transform);
        newSessionItem.Initialize(sessionInfo);
        newSessionItem.OnJoinSession += JoinSelectioinSession;
    }

    void JoinSelectioinSession(SessionInfo sessionInfo)
    {
        _runnerHandler.JoinGame(sessionInfo);
    }

    void ClearBrowser()
    {
        foreach (Transform chield in _verticalLayout.transform)
        {
            Destroy(chield.gameObject);
        }
        _statusText.gameObject.SetActive(false);
    }
}
