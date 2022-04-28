using System;
using System.Linq;
using static Helpers.LiteralToStringParse;
using static Helpers.Literals;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TOX;

namespace Helpers
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public GameObject playerPrefab;
        public GameObject freeSpectatorPrefab;
        public GameObject[] spawnPoints;

        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();
                }

                return _instance;
            }
        }

        #region Photon Callbacks

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        private void Start()
        {
            if (playerPrefab == null)
            {
                Debug.LogError(
                    "<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",
                    this);
            }
            else
            {
                if (PlayerMovement.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    var userType = (UserTypes)PhotonNetwork.LocalPlayer.CustomProperties[UserType];
                    switch (userType)
                    {
                        case UserTypes.Player:
                            int playerCount = DirectUserCount(UserTypes.Player);
                            Vector3 spawnPoint = playerCount > 1
                                ? spawnPoints[0].transform.position
                                : spawnPoints[1].transform.position;
                            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint, Quaternion.identity,
                                0);
                            break;
                        case UserTypes.FreeSpectator:
                            PhotonNetwork.Instantiate(freeSpectatorPrefab.name, new Vector3(0f, 5f, -10f),
                                Quaternion.identity, 0);
                            break;
                    }
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}",
                    PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                //LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}",
                    PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                //LoadArena();
            }
        }

        #endregion


        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Private Methods

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }

            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel(SampleScene);
        }

        private int DirectUserCount(UserTypes userType)
        {
            Player[] players = PhotonNetwork.PlayerList;

            int playerCount = players.Where(p =>
            {
                p.CustomProperties.TryGetValue(UserType, out object pType);
                return (UserTypes)pType == userType;
            }).Count();

            return playerCount;
        }

        #endregion
    }
}