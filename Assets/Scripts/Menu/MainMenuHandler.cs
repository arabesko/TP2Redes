using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private NetworkRunnerHandler _runnerHandler;

    [Header("Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _connectingPanel;
    [SerializeField] private GameObject _browserPanel;
    [SerializeField] private GameObject _hostPanel;

    [Header("Buttons")]
    [SerializeField] private Button _connectButton; 
    [SerializeField] private Button _goToHostButton;
    [SerializeField] private Button _hostButton; 

    [Header("Texts")]
    [SerializeField] private TMP_Text _connectingText; 

    [Header("InputFields")]
    [SerializeField] private TMP_InputField _sessionNameField;

    private void Awake()
    {
        _connectButton.onClick.AddListener(JoinLobbyButton);
        _goToHostButton.onClick.AddListener(ShowHostPanel);
        _hostButton.onClick.AddListener(CreateGameSession);

        _runnerHandler.OnLobbyJoined += () =>
        {
            _connectingPanel.SetActive(false);
            _browserPanel.SetActive(true);
        };
    }

    void JoinLobbyButton()
    {
        _runnerHandler.JoinLobby();
        _mainMenuPanel.SetActive(false);
        _connectingPanel.SetActive(true);

        _connectingText.text = "Joining Lobby...";
    }

    void ShowHostPanel()
    {
        _browserPanel.SetActive(false);
        _hostPanel.SetActive(true);
    }

    void CreateGameSession()
    {
        _hostButton.interactable = false;
        _runnerHandler.CreateGame(_sessionNameField.text, "Game");
    }
}
