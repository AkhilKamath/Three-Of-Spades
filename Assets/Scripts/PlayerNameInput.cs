using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SWNetwork;
using System;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;
    [SerializeField] private Button playButton = null;

    public Image[] plimg;
    public TMP_Text[] pltext;
    public string playerName;
    public enum LobbyState
    {
        Default,
        JoinedRoom,
    }
    public LobbyState State = LobbyState.Default;

    private void Start()
    {
        continueButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        NetworkClient.Lobby.OnLobbyConnectedEvent += Lobby_OnLobbyConnected;
        NetworkClient.Lobby.OnNewPlayerJoinRoomEvent += Lobby_OnNewPlayerJoinRoomEvent;

        foreach(Image img in plimg)
        {
            img.gameObject.SetActive(false);
        }
        foreach(TMP_Text text in pltext)
        {
            text.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        NetworkClient.Lobby.OnLobbyConnectedEvent -= Lobby_OnLobbyConnected;
        NetworkClient.Lobby.OnNewPlayerJoinRoomEvent -= Lobby_OnNewPlayerJoinRoomEvent;
    }

    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    //********* Matchmaking ***********
    public void Checkin()
    {
        playerName = nameInputField.text;
        continueButton.interactable = false;

        NetworkClient.Instance.CheckIn(playerName, (bool successful, string error) =>
        {
            if (!successful)
            {
                continueButton.interactable = true;
                Debug.LogError(error);
            }
        });
    }

    //*********** Lobby events ***********
    private void Lobby_OnLobbyConnected()
    {
        RegisterToLobbyServer();
    }
    private void Lobby_OnNewPlayerJoinRoomEvent(SWJoinRoomEventData eventData)
    {
        Debug.Log("New Player Added.");
        GetPlayersInTheRoom();
    }


    private void RegisterToLobbyServer()
    {
        NetworkClient.Lobby.Register(playerName, (successful, reply, error) => {
            if (successful)
            {
                Debug.Log("Lobby registered " + reply);
                if(string.IsNullOrEmpty(reply.roomId))
                {
                    JoinOrCreateRoom();
                }
                else if(reply.started)
                {
                    State = LobbyState.JoinedRoom;
                    ConnectToRoom();
                }
                else
                {
                    State = LobbyState.JoinedRoom;
                    GetPlayersInTheRoom();
                }
            }
            else
            {
                Debug.Log("Lobby failed to register " + reply);
            }
        });
    }

    private void JoinOrCreateRoom()
    {
        NetworkClient.Lobby.JoinOrCreateRoom(false, 4, 0, (successful, reply, error) => {
            if (successful)
            {
                Debug.Log("Joined or created room " + reply);
                State = LobbyState.JoinedRoom;
                //ShowJoinedRoomPopover();
                GetPlayersInTheRoom();
            }
            else
            {
                Debug.Log("Failed to join or create room " + error);
            }
        });
    }

    private void GetPlayersInTheRoom()
    {
        NetworkClient.Lobby.GetPlayersInRoom((successful, reply, error) => {
            if (successful)
            {
                ShowPlayersEnteredUI(reply);
                Debug.Log("Got players " + reply);
                if (reply.players.Count < 4)
                {
                    Debug.Log("Waiting for" + (4 - reply.players.Count) + "players to join");
                }
                else if (reply.players.Count > 4)
                {
                    Debug.Log("There are extra players. Only 4 supported");
                }
                else
                {
                    if(NetworkClient.Lobby.IsOwner)
                    {
                        continueButton.gameObject.SetActive(false);
                        playButton.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                Debug.Log("Failed to get players " + error);
            }
        });
    }

    /*void ShowJoinedRoomPopover()
    {
        EnterNicknamePopover.SetActive(false);
        WaitForOpponentPopover.SetActive(true);
        StartRoomButton.SetActive(false);
        Player1Portrait.SetActive(false);
        Player2Portrait.SetActive(false);
    }*/

    private void ConnectToRoom()
    {
        //TODO: implement this func
    }

    void ShowPlayersEnteredUI(SWGetPlayersReply reply)
    {
        for(int i=0; i <reply.players.Count && i<4; i++)
        {
            plimg[i].gameObject.SetActive(true);
            pltext[i].gameObject.SetActive(true);
            string suffix = i == 0 ? " - Host" : "";
            pltext[i].SetText(reply.players[i].data + suffix);
        }
    }

    private void LeaveRoom()
    {
        NetworkClient.Lobby.LeaveRoom((successful, error) => {
            if (successful)
            {
                Debug.Log("Left room");
                State = LobbyState.Default;
            }
            else
            {
                Debug.Log("Failed to leave room " + error);
            }
        });
    }

    public void OnCancelClick()
    {
        if(State == LobbyState.JoinedRoom)
        {
            LeaveRoom();
        }
        else
        {
            Debug.Log("Not inside a room.");
        }
    }
}
