using System.Collections;
using System.Collections.Generic;
using Helpers;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchManager : MonoBehaviourPun
{
    [SerializeField] private TMP_Text gameOverText;

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
        GameOver(Literals.MatchResults.Defeat);
        photonView.RPC("GameOver", RpcTarget.Others, Literals.MatchResults.Victory);
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
                StartCoroutine(nameof(GameOverSequence), "VICTORY");
                break;
            case Literals.MatchResults.Defeat:
                //StartCoroutine(GameOverSequence("DEFEAT"));
                StartCoroutine(nameof(GameOverSequence), "DEFEAT");
                break;
        }
    }

    private IEnumerator GameOverSequence(string gameOverMessage)
    {
        gameOverText.text = gameOverMessage;
        gameOverText.enabled = true;
        yield return new WaitForSeconds(10);
        Debug.Log("disc");
        PhotonNetwork.Disconnect();
    }
}