using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

public class CollisionController : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;
    [SerializeField] private GameObject playerWeapon;

    //Dummy
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"trigger enter {other.name}");
        
        if (other.gameObject.CompareTag("Weapon") && other.gameObject != playerWeapon)
        {
            StartCoroutine(DamageFeedback());
            /*switch (gameObject.tag)
            {
            case "Player":
                StartCoroutine(DamageFeedback());
                break;
            case "Dummy":
                StartCoroutine(DummyFeedback());
                break;
            default:
                return;
            }*/
        }
    }

    private IEnumerator DummyFeedback()
    {
        this.gameObject.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<SkinnedMeshRenderer>().material.color = Color.white;
    }

    //Player
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log($"collider enter {hit.gameObject.name}");
        if (hit.gameObject.CompareTag("Weapon"))
        {
            StartCoroutine(DamageFeedback());
        }
    }

    private IEnumerator DamageFeedback()
    {
        /*List<Material> materials = null;
        foreach (var material in playerMeshRenderer.materials)
        {
            material.color = Color.red;
            materials!.Add(material);
        }
        playerMeshRenderer.materials = materials!.ToArray();*/
        playerMeshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(1f);
        playerMeshRenderer.material.color = Color.white;
        /*foreach (var material in materials) material.color = Color.white;
        playerMeshRenderer.materials = materials!.ToArray();*/
    }
}