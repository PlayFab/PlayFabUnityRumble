using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Party;
using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DemoScript : MonoBehaviour
{
    public InputField networkIdTextBox;
    public Text output;

    // Start is called before the first frame update
    void Start()
    {
        var playFabMultiplayerManager = PlayFabMultiplayerManager.Get();

        // This will turn on verbose logging that is useful when debugging the SDK.
        //playFabMultiplayerManager.LogLevel = PlayFabMultiplayerManager.LogLevelType.Verbose;

        // Log into playfab. The SDK will use the logged in user when connecting to the network.
        var request = new LoginWithCustomIDRequest { CustomId = UnityEngine.Random.value.ToString(), CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);

        // Listen for this event to know when the SDK has joined the network and can start sending messages.
        playFabMultiplayerManager.OnNetworkJoined += OnNetworkJoined;

        // The simple way to receive data messages.
        playFabMultiplayerManager.OnDataMessageReceived += OnDataMessageReceived;

        // A more complex, but no copy way of recieving data messages.
        playFabMultiplayerManager.OnDataMessageNoCopyReceived += OnDataMessageNoCopyReceived;
    }

    public void CreateAndJoinToNetwork()
    {
        // Create and join a network
        PlayFabMultiplayerManager.Get().CreateAndJoinNetwork();
    }

    public void JoinNetwork()
    {
        PlayFabMultiplayerManager.Get().JoinNetwork(networkIdTextBox.text);
    }

    private void OnDataMessageReceived(object sender, PlayFabPlayer from, byte[] buffer)
    {
        Debug.Log("Got a message (simple).");
        output.text += "\r\n Got a message (simple).";
    }

    private void OnDataMessageNoCopyReceived(object sender, PlayFabPlayer from, System.IntPtr buffer, uint bufferSize)
    {
        Debug.Log("Got a message (no copy).");
        output.text += "\r\n Got a message (no copy).";
    }

    private void OnNetworkJoined(object sender, string networkId)
    {
        Debug.Log("Joined the network.");
        output.text += "\r\n Joined the network.";

        networkIdTextBox.text = networkId;

        // Simple send message.
        byte[] buffer = Encoding.ASCII.GetBytes("Hello world (simple message).");
        PlayFabMultiplayerManager.Get().SendDataMessageToAllPlayers(buffer);

        // Send a data message. There is a simpler version of this API available as well.
        byte[] buffer2 = Encoding.ASCII.GetBytes("Hello world (no garbage collection method).");
        IntPtr unmanagedPointer = Marshal.AllocHGlobal(buffer2.Length);
        Marshal.Copy(buffer2, 0, unmanagedPointer, buffer2.Length);
        PlayFabMultiplayerManager.Get().SendDataMessage(unmanagedPointer, (uint)buffer2.Length, PlayFabMultiplayerManager.Get().RemotePlayers, DeliveryOption.BestEffort);
        Marshal.FreeHGlobal(unmanagedPointer);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Logged into PlayFab.");
        output.text += "\r\n Logged into PlayFab.";
    }
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.Log("Error logging into PlayFab: " + error.ErrorMessage);
        output.text += "\r\n Error logging into PlayFab: " + error.ErrorMessage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
