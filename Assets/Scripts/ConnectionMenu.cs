using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ConnectionMenu : MonoBehaviour
{
    [SerializeField]
    private InputField ipInput;

    public void OnClickHost()
    {
        NetworkManager.singleton.StartHost();
    }
    public void OnClickConnect()
    {
        NetworkManager.singleton.networkAddress = string.IsNullOrEmpty(ipInput.text) ? "localhost" : ipInput.text;
        NetworkManager.singleton.StartClient();   
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
