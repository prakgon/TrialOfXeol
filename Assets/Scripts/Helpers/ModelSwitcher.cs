using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using Photon.Pun;
using UnityEngine;

public class ModelSwitcher : MonoBehaviour
{
    [SerializeField] private Model[] models;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    private void Awake()
    {
        models[0].gameObject.SetActive(false);
        switch (PhotonView.Get(this).Owner.CustomProperties["playerIndex"])
        {
            case 1:
                models[0].Set(animator);
                break;
            default:
                models[1].Set(animator);
                break;
        }
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    [Serializable]
    private struct Model
    {
        public GameObject gameObject;
        public Avatar avatar;

        public void Set(Animator animator)
        {
            gameObject.SetActive(true);
            animator.avatar = avatar;
        }
    }
}