using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace Com.TankWarfareOnline
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Properties


        public GameObject playerPrefab;
        public GameObject spawnPrefab;
        public GameObject wallPrefab;

        private bool playerHasSpawned;


        #endregion


        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }


        #endregion


        #region Public Methods


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        #endregion


        #region Private Methods


        private void Start()
        {
            playerHasSpawned = false;

            if (PlayerManager.LocalPlayerInstance == null)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    CreateSpawns();

                    CreateWalls();
                }
            }
        }

        private void CreateSpawns()
        {
            Debug.Log("Creating Spawns");

            PhotonNetwork.Instantiate(
                            "Prefabs/" + spawnPrefab.name,
                            new Vector3(-40, 0, -40),
                            Quaternion.identity,
                            0);

            PhotonNetwork.Instantiate(
                            "Prefabs/" + spawnPrefab.name,
                            new Vector3(-40, 0, 40),
                            Quaternion.identity,
                            0);
        }

        private void CreateWalls()
        {
            Debug.Log("Creating Walls");

            Wall[] walls = Resources.FindObjectsOfTypeAll(typeof(Wall)) as Wall[];

            foreach(Wall wall in walls)
            {
                PhotonNetwork.InstantiateSceneObject(
                    "Prefabs/" + wallPrefab.name,
                    wall.transform.localPosition,
                    wall.transform.rotation);
            }
        }

        private Spawn GetAvailableSpawn()
        {
            Spawn[] spawns = FindObjectsOfType<Spawn>();

            foreach(Spawn spawn in spawns)
            {
                if (spawn.GetIsAvailable())
                    return spawn;
            }

            return null;
        }

        private void Update()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab " +
                    "Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if (!playerHasSpawned && PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}",
                        SceneManager.GetActiveScene().name);

                    Spawn spawn = GetAvailableSpawn();

                    if (spawn == null)
                        Debug.Log("Spawn not available");
                    else
                    {
                        // We're in a room. spawn a character for the local player.
                        // It gets synced by using PhotonNetwork.Instantiate
                        PhotonNetwork.Instantiate(
                            "Prefabs/" + playerPrefab.name,
                            spawn.transform.position,
                            Quaternion.identity,
                            0);


                        spawn.SetIsAvailable(false);
                        playerHasSpawned = true;
                    }
                }
            }
        }

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we " +
                    "are not the master Client");
            }

            Debug.Log("PhotonNetwork : Loading Level");
            PhotonNetwork.LoadLevel("Game");
        }


        #endregion


        #region Photon Callbacks


        public override void OnPlayerEnteredRoom(Player other)
        {
            // Not seen if you're the player connecting
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); 


            if (PhotonNetwork.IsMasterClient)
            {
                // Called before OnPlayerLeftRoom
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}",
                    PhotonNetwork.IsMasterClient);

                LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            // Seen when other disconnects
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); 

            if (PhotonNetwork.IsMasterClient)
            {
                // Called before OnPlayerLeftRoom
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}",
                    PhotonNetwork.IsMasterClient);

                LoadArena();
            }
        }


        #endregion
    }
}
