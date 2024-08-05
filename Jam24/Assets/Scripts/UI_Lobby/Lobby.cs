using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Lobby : MonoBehaviourPunCallbacks
{
  public PhotonView PhotonView;

  public GameObject RoomItemPrefab;
  public GameObject RoomsContainer;

  public GameObject CreateRoomMenu;
  public GameObject LobbyMenu;

  public TMP_InputField CraeteRoomRoomName;
  public TextMeshProUGUI LobbyRoomName;

  public Button StartGame;

  private List<RoomItem> RoomItems = new List<RoomItem>();
  private List<RoomInfo> _rooms = new List<RoomInfo>();

  private string _roomName;

  private bool _startSolotGame = false;
  private bool _connectedToLobby = false;

  private void Start()
  {
    Debug.LogWarning("Connecting ...");

    SoundSystem.Instance.Play("MainMenu");

    PhotonNetwork.ConnectUsingSettings();
  }

  public override void OnConnectedToMaster()
  {
    base.OnConnectedToMaster();

    Debug.LogWarning("Connected to Server!");

    PhotonNetwork.JoinLobby();
  }

  public override void OnJoinedLobby()
  {
    base.OnJoinedLobby();

    Debug.LogWarning("Connected to Lobby!");

    _connectedToLobby = true;
  }

  public void JoinRoom(string roomName)
  {
    if (!_connectedToLobby)
    {
      Debug.LogWarning("Connecting to Lobby in process!");
      return;
    }

    _roomName = roomName;

    PhotonNetwork.JoinRoom(roomName, null);
    LobbyMenu.SetActive(true);

    LobbyRoomName.text = _roomName;
  }

  public void OpenCreateRoom()
  {
    CreateRoomMenu.SetActive(true);
  }

  public void CreateRoomConfirm()
  {
    CreateRoomMenu.SetActive(false);
    LobbyMenu.SetActive(true);

    _roomName = CraeteRoomRoomName.text;
    LobbyRoomName.text = _roomName;

    CreateRoom(CraeteRoomRoomName.text);
  }

  public void CreateRoomClose()
  {
    CreateRoomMenu.SetActive(false);
  }

  public void LobbyStartGame()
  {
    LobbyMenu.SetActive(false);

    PhotonView.RPC("ChangeScene", RpcTarget.All, "Level1");
  }

  public void LobbyClose()
  {
    LobbyMenu.SetActive(false);

    PhotonNetwork.LeaveLobby();
    PhotonNetwork.LeaveRoom();

    _roomName = null;
  }

  public void ExitGame()
  {
    Application.Quit();
  }

  public void CreateRoom(string roomName)
  {
    if (!_connectedToLobby)
    {
      Debug.LogWarning("Connecting to Lobby in process!");
      return;
    }

    var options = new RoomOptions();
    options.MaxPlayers = 2;
    _roomName = roomName;

    PhotonNetwork.CreateRoom(roomName, options, null);
  }

  public override void OnRoomListUpdate(List<RoomInfo> roomList)
  {
    base.OnRoomListUpdate(roomList);

    if (_rooms == null || _rooms.Count == 0)
    {
      _rooms = roomList;
    }
    else
    {
      foreach (var item in roomList)
      {
        var room = _rooms.Find(x => x.Name == item.Name);
        if (room == null)
        {
          _rooms.Add(item);
        }
        else
        {
          if (room.RemovedFromList)
          {
            _rooms.Remove(room);
          }
          else
          {
            room = item;
          }
        }
      }
    }  

    UpdateRoomsUI();
  }

  public void UpdateRoomsUI()
  {
    var diff = RoomItems.Count - _rooms.Count;

    if (diff > 0)
    {
      var unusedRooms = RoomItems.TakeLast(diff).ToList();
      foreach (var item in unusedRooms)
      {
        var ri = item.gameObject.GetComponent<RoomItem>();
        ri.OnJoinClick -= JoinRoom;

        Destroy(item.gameObject);
        RoomItems.Remove(item);
      }
    }
    else if (diff < 0)
    {
      var count = Mathf.Abs(diff);
      for (var i = 0; i < count; i++)
      {
        var go = Instantiate(RoomItemPrefab);
        go.transform.SetParent(RoomsContainer.transform);

        var ri = go.GetComponent<RoomItem>();
        ri.OnJoinClick += JoinRoom;

        RoomItems.Add(ri);
      }
    }

    for (var i = 0; i < RoomItems.Count; i++)
    {
      var updatedRoomInfo = _rooms[i];
      RoomItems[i].Name.text = updatedRoomInfo.Name;
    }
  }

  public void StartSoloGame()
  {
    var options = new RoomOptions();
    options.MaxPlayers = 1;
    options.IsVisible = false;
    _startSolotGame = true;

    PhotonNetwork.CreateRoom($"{DateTime.Now.ToString()}", options, null);
  }

  public override void OnCreatedRoom()
  {
    base.OnCreatedRoom();

    if (!_startSolotGame)
      return;

    SoundSystem.Instance.Stop();
    PhotonView.RPC("ChangeScene", RpcTarget.All, "Level1");
  }

  [PunRPC]
  public void ChangeScene(string sceneName)
  {
    PhotonNetwork.LoadLevel(sceneName);
  }

  private void Update()
  {
    if (_roomName == null)
      return;

    if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount < 2 && StartGame.IsInteractable())
      StartGame.interactable = false;
    else if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount >= 2 && !StartGame.IsInteractable())
      StartGame.interactable = true;
  }
}