using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCollisionController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"trigger enter {other.name}");
        
        if (other.gameObject.CompareTag("Weapon"))
        {
            StartCoroutine(DummyFeedback());
        }
    }
    
    private IEnumerator DummyFeedback()
    {
        this.gameObject.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<SkinnedMeshRenderer>().material.color = Color.white;
    }
}
