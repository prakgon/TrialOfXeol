using System.Collections;
using System.Collections.Generic;
using Helpers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchManager : MonoBehaviourPun
{
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
        switch (result)
        {
            case Literals.MatchResults.Victory:
                Debug.Log("Game Over: Victory");
                break;
            case Literals.MatchResults.Defeat:
                Debug.Log("Game Over: Defeat");
                break;
        }
    }
}
