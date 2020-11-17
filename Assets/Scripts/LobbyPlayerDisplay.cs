using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerDisplay : MonoBehaviour
{
    public bool Filled { get { return button.interactable; } }
    public bool Ready { get; private set; } = false;
    [SerializeField]
    private Text playerName;
    [SerializeField]
    private Button button;
    [SerializeField]
    private Image readyIndicator;
    [SerializeField]
    private Color readyColor = Color.green;
    [SerializeField]
    private Color notReadyColor = Color.red;

    private NetworkPlayer player;
    private int index;

    public void AssignPlayer(NetworkPlayer _player, int _index)
    {
        player = _player;
        index = _index;
        button.interactable = true;
        readyIndicator.color = notReadyColor;
    }

    public void ToggleReadyState()
    {
        SetReadyState(!Ready);
        player.ReadyPlayer(index, Ready);
    }
    public void SetReadyState(bool _isReady) => Ready = _isReady;
    // Start is called before the first frame update
    void Start()
    {
        button.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        playerName.text = player != null && !string.IsNullOrEmpty(player.username) ? player.username : "Player";
        readyIndicator.color = Ready ? readyColor : notReadyColor;
    }
}
