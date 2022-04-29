using Configuration;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using Helpers;
using static Helpers.Literals;
using static Helpers.LiteralToStringParse;

namespace TOX
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip(
            "The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        Button _singleButton, _multiButton, _spectatorButton;

        [SerializeField] private MultiplayerConfigurationSO multiplayerConfiguration;

        #endregion


        #region Private Fields

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";

        private bool _isConnecting;
        private Hashtable _playerProperties;
        private UserTypes _userType;
        private TypedLobby sqlLobby = new TypedLobby("Lobby", LobbyType.SqlLobby);

        #endregion


        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
            _playerProperties = new Hashtable();
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            _userType = UserTypes.Player;
            _singleButton.onClick.AddListener(StartSingleplayer);
            _multiButton.onClick.AddListener(Connect);
            _spectatorButton.onClick.AddListener(StartFreeSpectator);
        }

        #endregion

        #region Private Methods

        private void StartSingleplayer()
        {
            PhotonNetwork.OfflineMode = true;
            Connect();
        }

        private void StartFreeSpectator()
        {
            _userType = UserTypes.FreeSpectator;
            Connect();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            DontDestroyOnLoad(gameObject);
            _playerProperties.Add(UserType, _userType);
            PhotonNetwork.LocalPlayer.SetCustomProperties(_playerProperties);
            SceneManager.LoadScene(LoadingScreen);
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            _isConnecting = true;
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            string sqlLobbyFilter = "";
            if (_isConnecting)
            {
                switch (_userType)
                {
                    case UserTypes.Player:
                        sqlLobbyFilter = ToxSqlProperties.FighterCount + "<" + ToxSqlProperties.MaxFighters;
                        break;
                    case UserTypes.FreeSpectator:
                        sqlLobbyFilter = ToxSqlProperties.SpectatorCount + "<" + ToxSqlProperties.MaxSpectators;
                        break;
                }

                PhotonNetwork.JoinRandomRoom(null, multiplayerConfiguration.maxPlayersPerRoom, MatchmakingMode.FillRoom,
                    sqlLobby,
                    sqlLobbyFilter);
            }
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}",
                cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log(
                "PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            RoomOptions options = new RoomOptions();
            options.CustomRoomProperties = new Hashtable
            {
                { ToxSqlProperties.FighterCount, 0 },
                { ToxSqlProperties.MaxFighters, multiplayerConfiguration.maxFighters },
                { ToxSqlProperties.SpectatorCount, 0 },
                { ToxSqlProperties.MaxSpectators, multiplayerConfiguration.maxSpectators }
            };


            options.CustomRoomPropertiesForLobby = new[]
            {
                ToxSqlProperties.FighterCount, ToxSqlProperties.MaxFighters, ToxSqlProperties.SpectatorCount,
                ToxSqlProperties.MaxSpectators
            };

            options.MaxPlayers = multiplayerConfiguration.maxPlayersPerRoom;
            PhotonNetwork.CreateRoom(null, options, sqlLobby);
        }

        public override void OnJoinedRoom()
        {
            if (!PhotonNetwork.OfflineMode)
            {
                switch (_userType)
                {
                    case UserTypes.Player:
                        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(ToxSqlProperties.FighterCount,
                            out object fighterCount);
                        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
                            { { ToxSqlProperties.FighterCount, (int)fighterCount + 1 } });
                        break;
                    case UserTypes.FreeSpectator:
                        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(ToxSqlProperties.SpectatorCount,
                            out object spectatorCount);
                        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
                            { { ToxSqlProperties.SpectatorCount, (int)spectatorCount + 1 } });
                        break;
                }
            }

            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
                PhotonNetwork.LoadLevel(OnlineScene);
            }
        }

        #endregion
    }
}