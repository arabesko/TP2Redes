using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner _runnerPrefab;
    [SerializeField] private NetworkRunner _currentRunner;

    public event Action OnLobbyJoined; 
    public event Action<List<SessionInfo>> OnSessionListUpdate;

    private void Start()
    {
        //JoinLobby();
    }

    #region Lobby

    public void JoinLobby()
    {
        if (_currentRunner)
            Destroy(_currentRunner.gameObject);

        _currentRunner = Instantiate(_runnerPrefab);
        
        _currentRunner.AddCallbacks(this);

        JoinLobbyAsync();
    }

    async void JoinLobbyAsync()
    {
        var result = await _currentRunner.JoinSessionLobby(SessionLobby.Custom, "Normal");

        if (!result.Ok)
        {
            Debug.LogError($"[Custom Error] Unable to join Lobby");
            return;
        }
        
        Debug.Log($"[Custom Msg] Joined Lobby");
        OnLobbyJoined?.Invoke();
    }
    
    #endregion

    #region Session

    public async void CreateGame(string sessionName, string sceneName)
    {
        await InitializeGame(GameMode.Host, sessionName, SceneUtility.GetBuildIndexByScenePath($"Scenes/{sceneName}"));
    }

    public async void JoinGame(SessionInfo sessionInfo)
    {
        await InitializeGame(GameMode.Client, sessionInfo.Name);
    }
    
    async Task InitializeGame(GameMode gameMode, string sessionName, int sceneIndex = 0)
    {
        _currentRunner.ProvideInput = true;

        var result = await _currentRunner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            Scene = SceneRef.FromIndex(sceneIndex),
            SessionName = sessionName,
            PlayerCount = 2
        });
        
        if (!result.Ok)
        {
            Debug.LogError($"[Custom Error] Unable to Start Game");
            return;
        }
        
        Debug.Log($"[Custom Msg] Game Started");
    }

    #endregion
    
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        OnSessionListUpdate?.Invoke(sessionList);
        
        // //Si no hay ninguna sala creada
        // if (sessionList.Count == 0)
        // {
        //     //Creo una:
        //     CreateGame("Room 0", "Game");
        // }
        // else //De lo contrario
        // {
        //     foreach (var sessionInfo in sessionList)
        //     {
        //         if (sessionInfo.PlayerCount >= sessionInfo.MaxPlayers) continue;
        //         
        //         JoinGame(sessionInfo);
        //         return;
        //     }
        // }
    }
    
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){ }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){ }
}
