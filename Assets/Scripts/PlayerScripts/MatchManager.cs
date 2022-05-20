using System.Collections;
using System.Collections.Generic;
using Audio;
using ExitGames.Client.Photon;
using Helpers;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchManager : MonoBehaviourPun, IOnEventCallback
{
    [SerializeField] private TMP_Text gameOverText;
    public const byte GameOverEvent = 1;


    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == GameOverEvent)
        {
            GameOver((Literals.MatchResults)photonEvent.CustomData);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int PlayerDied()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(GameOverEvent, Literals.MatchResults.Victory, raiseEventOptions,
            SendOptions.SendReliable);
        GameOver(Literals.MatchResults.Defeat);
        return 1;
    }

    [PunRPC]
    public void GameOver(Literals.MatchResults result)
    {
        GetComponent<PlayerInput>().enabled = false;
        switch (result)
        {
            case Literals.MatchResults.Victory:
                //StartCoroutine(GameOverSequence("VICTORY"));
                AudioManager.Instance.OneShot(Literals.AudioType.Victory);

                StartCoroutine(nameof(GameOverSequence), "VICTORY");
                break;
            case Literals.MatchResults.Defeat:
                //StartCoroutine(GameOverSequence("DEFEAT"));
                AudioManager.Instance.OneShot(Literals.AudioType.Death);

                StartCoroutine(nameof(GameOverSequence), "DEFEAT");
                break;
        }
    }

    private IEnumerator GameOverSequence(string gameOverMessage)
    {
        if (!photonView.IsMine) yield break;
        gameOverText.text = gameOverMessage;
        yield return new WaitForSeconds(10);
        Debug.Log("disc");
        PhotonNetwork.Disconnect();
    }
}