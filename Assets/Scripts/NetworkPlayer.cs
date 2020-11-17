using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar]
    public string username = "Player";
    [SerializeField]
    private GameObject lobbyPlayer;
    [SerializeField]
    private GameObject gameplayPlayer;

    private bool connectionToLobbyUI = false;

    private LobbyMenu lobby;
    // Start is called before the first frame update
    void Start()
    {
        lobbyPlayer.SetActive(true);
        gameplayPlayer.SetActive(false);
        
    }
    public void StartGame()
    {
        if (isLocalPlayer)
        {
            CmdStartGame();
        }
    }
    public void SetName(string _name)
    {
        if (isLocalPlayer)
        {
            CmdSetPlayerName(_name);
        }
    }
    public override void OnStartLocalPlayer()
    {
        SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive);
    }
    public void ReadyPlayer(int _index, bool _isReady)
    {
        if (isLocalPlayer)
        {
            CmdReadyPlayer(_index, _isReady);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (lobby == null && lobbyPlayer.activeSelf)
        {
            lobby = FindObjectOfType<LobbyMenu>();
        }
        if (!connectionToLobbyUI && lobby != null)
        {
            lobby.OnPlayerConnect(this);
            connectionToLobbyUI = true;
        }
    }

    [Command] public void CmdReadyPlayer(int _index, bool _isReady) => RpcReadyPlayer(_index, _isReady);
    [ClientRpc] public void RpcReadyPlayer(int _index, bool _isReady) => lobby?.SetReadyPlayer(_index, _isReady);

    [Command] public void CmdSetPlayerName(string _name) => RpcSetPlayerName(_name);
    [ClientRpc] public void RpcSetPlayerName(string _name) => username = _name;
    [Command] public void CmdStartGame() => RpcStartGame();
    public void RpcStartGame()
    {
        NetworkPlayer[] players = FindObjectsOfType<NetworkPlayer>();
        foreach (NetworkPlayer player in players)
        {
            player.lobbyPlayer.SetActive(false);
            player.gameplayPlayer.SetActive(true);
            if (player.isLocalPlayer)
            {
                SceneManager.UnloadSceneAsync("Lobby");
                SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Additive);
            }
        }
    }
}
