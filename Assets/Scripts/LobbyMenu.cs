using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField]
    private LobbyPlayerDisplay[] playerDisplays;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button readyButton;
    [SerializeField]
    private InputField playerNameInput;

    private GameNetworkManager network;
    private NetworkPlayer localPlayer;

    public void OnPlayerConnect(NetworkPlayer _player)
    {
        for (int i = 0; i < playerDisplays.Length; i++)
        {
            LobbyPlayerDisplay display = playerDisplays[i];
            if (!display.Filled)
            {
                display.AssignPlayer(_player, i);
                if (_player.isLocalPlayer)
                {
                    localPlayer = _player;
                    readyButton.onClick.AddListener(display.ToggleReadyState);
                }

                break;
            }
        }
    }
 
    public void OnClickStart() => localPlayer.StartGame();
    
    public void SetReadyPlayer(int _index, bool _isReady)
    {
        playerDisplays[_index].SetReadyState(_isReady);
    }
    // Start is called before the first frame update
    void Start()
    {
        network = GameNetworkManager.singleton as GameNetworkManager;
        playerNameInput.onEndEdit.AddListener(OnEndEditName);
        startButton.interactable = false;
    }
    private void OnEndEditName(string _name)
    {

        if (localPlayer != null)
        {
            localPlayer.SetName(_name);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (network.IsHost)
        {
            foreach (LobbyPlayerDisplay display in playerDisplays)
            {
                if (!display.Ready && display.Filled)
                {
                    if (startButton.interactable)
                    {
                        startButton.interactable = false;
                    }
                    return;
                }
            }
            if (!startButton.interactable)
            {
                startButton.interactable = true;
            }
        }
    }
}
