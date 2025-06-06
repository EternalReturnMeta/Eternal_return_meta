
using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Menu;
using Fusion.Sockets;
using UnityEngine;


public class MatchingManagerSpawner : MonoBehaviour , INetworkRunnerCallbacks
{
    // 인스펙터에서 할당할 프리팹 및 UI 컨트롤러
    [SerializeField] private NetworkObject _matchingManagerPrefab;
    [SerializeField] private NetworkObject _playerNetworkObjectPrefab;
    [SerializeField] public MenuUIController Controller;
    public MatchingManager MatchingManagerInstance { get; private set; }
    private NetworkObject _spawnedManager;
    private NetworkRunner _runner;

    public void Initialize(NetworkRunner runner)
    {
        _runner = runner;
        _runner.AddCallbacks(this);
    }

    // 세션 시작 시 서버에서 한 번만 호출
    // public void SpawnMatchingManager()
    // {
    //     if (_spawnedManager != null)
    //     {
    //         Debug.LogWarning("MatchingManager는 세션당 1개만 생성됩니다.");
    //         return;
    //     }
    //     if (_runner.IsServer)
    //     {
    //         _spawnedManager = _runner.Spawn(_matchingManagerPrefab, Vector3.zero, Quaternion.identity, null);
    //         MatchingManagerInstance = _spawnedManager.GetComponent<MatchingManager>();
    //         MatchingManagerInstance.Controller = Controller;
    //         Debug.Log("MatchingManager 싱글톤 네트워크 오브젝트 스폰 완료");
    //     }
    // }
    
    //
    //
    // //플레이어 참가 시 호출
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer) // 서버에서만 실행
        {
            // 매칭 매니저 프리팹 스폰
            var playerObj = runner.Spawn(_playerNetworkObjectPrefab, inputAuthority: player,
                onBeforeSpawned: (runner, obj) =>
                {
                }
            );
            runner.SetPlayerObject(player, playerObj);
            
            if (MatchingManagerInstance == null)
            {
                MatchingManagerInstance = runner.Spawn(
                    _matchingManagerPrefab,
                    position: Vector3.zero,
                    rotation: Quaternion.identity,
                    inputAuthority: null
                ).GetComponent<MatchingManager>();
                MatchingManagerInstance.Controller = Controller;
            }
        }
    }
    
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

    }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){ }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){ }
}

        //     public void SpawnMatchingManager()
        //     {
        //         if (Runner == null)
        //             Debug.Log("No runner");
        //         if (!Runner.IsServer)
        //             Debug.Log("No Server");
        //         if (_matchingManagerPrefab == null)
        //             Debug.Log("No matching manager");
        //         if (Runner != null && Runner.IsServer && _matchingManagerPrefab != null)
        //         {
        //             Debug.Log("안되나2?");
        //             var spawnedObj = Runner.Spawn(_matchingManagerPrefab, Vector3.zero, Quaternion.identity, Runner.LocalPlayer);
        //             _spawnedPlayers.Add(player, networkPlayerObject);
        //             Debug.Log("안되나3?");
        //             CurrentManager = spawnedObj.GetComponent<MatchingManager>();
        //             Debug.Log("안되나4?");
        //             if (Controller == null)
        //                 Debug.Log("컨트롤러가 없음");
        //             CurrentManager.Controller = this.Controller;
        //             if (CurrentManager == null)
        //                 Debug.Log("내부에 컨트롤러가 없음");
        //             Debug.Log("생성 완료됨");
        //         }
        //     }
        // }
