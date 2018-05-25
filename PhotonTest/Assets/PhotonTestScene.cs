using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonTestScene : Photon.PunBehaviour {
	[SerializeField] InputField ChatInputField;
	[SerializeField] Text OutputText;
    [SerializeField] PhotonView PhotonViewObject;

    private readonly string GameVersion = "1";

	// Use this for initialization
	void Start () {
		OutputText.text = "";
        InitPhoton();
	}

    private void InitPhoton() {
        PhotonNetwork.ConnectUsingSettings(GameVersion);
        CheckPlayers();
   }

    // Update is called once per frame
    int count = 0;
    void Update () {
        if (count > 30) {
            Debug.Log("PlayerCount:" + PhotonNetwork.countOfPlayers);
            count = 0;
        }
        count++;
    }

    public void OnClickCreateRoomButton() {
        PhotonNetwork.CreateRoom("Room");// Createすると、自動的にJoinする
        CheckPlayers();
    }

    public void OnClickJoinRoomButton() {
        PhotonNetwork.JoinRoom("Room");
        CheckPlayers();
    }

    public void OnClickLeaveRoomButton() {
        PhotonNetwork.LeaveRoom(false);// これtrueにすると、勝手に部屋Createして勝手にJoinする
        CheckPlayers();
    }

    public void OnClickSendMessageButton() {
        AddOutputString(string.Format("自分の発言:{0}", ChatInputField.text));
        PhotonViewObject.RPC("RecvMessage", PhotonTargets.Others, PhotonNetwork.player.ID, ChatInputField.text);
    }

    private void AddOutputString(string str) {
		OutputText.text = string.Format("{0}\n{1}", OutputText.text, str);
	}

    private void CheckPlayers() {
        PhotonPlayer p = PhotonNetwork.player;
        AddOutputString("MyID:" + p.ID);
        PhotonPlayer[] pList = PhotonNetwork.otherPlayers;
        for (int i = 0; i < pList.Length; i++) {
            AddOutputString("PtherD:" + pList[i].ID);
        }
    }

    public override void OnJoinedRoom() {
        AddOutputString("-----OnJoinedRoom-----");
    }

    public override void OnCreatedRoom() {
        AddOutputString("-----OnCreatedRoom-----");
    }

    public override void OnLeftRoom() {
        AddOutputString("-----OnLeftRoom-----");
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
        AddOutputString("-----OnPhotonPlayerConnected-----");
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        AddOutputString("-----OnPhotonPlayerDisconnected-----");
    }

    [PunRPC]
    void RecvMessage(int ID, string outputString) {
        AddOutputString(string.Format("{0}さんの発言:{1}", ID, outputString));
    }
}
