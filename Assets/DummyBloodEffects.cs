using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class DummyBloodEffects : MonoBehaviour
{
    public bool InfiniteDecal;
    public Light DirLight;
    public bool isVR = true;
    public GameObject BloodAttach;
    public GameObject[] BloodFX;

    Transform GetNearestObject(Transform hit, Vector3 hitPos)
    {
        var closestPos = 100f;
        Transform closestBone = null;
        var childs = hit.GetComponentsInChildren<Transform>();

        foreach (var child in childs)
        {
            var dist = Vector3.Distance(child.position, hitPos);
            if (dist < closestPos)
            {
                closestPos = dist;
                closestBone = child;
            }
        }

        var distRoot = Vector3.Distance(hit.position, hitPos);
        if (distRoot < closestPos)
        {
            closestPos = distRoot;
            closestBone = hit;
        }

        return closestBone;
    }

    public Vector3 direction;
    int effectIdx;

    private void OnTriggerEnter(Collider other)
    {
        PlayBloodV1(other);
        
        /*var myPosition = transform.position;
        var otherPosition = other.transform.position;
        var differencePosition = otherPosition - myPosition;
        differencePosition = differencePosition.normalized;

        var collisionPoint = other.ClosestPoint(myPosition);

        Ray ray = new Ray(collisionPoint, differencePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.green, 5f);

        InstantiateBloodEffect(ray);*/
        /*var transformPosition = transform.position;
        //var transformPosition = transform.position + new Vector3 (0, 0, 0.03f);
        collisionPoint = other.ClosestPoint(transformPosition);
        var collisionNormal = transformPosition - collisionPoint;
        var ray = new Ray(collisionPoint, collisionNormal);
        Debug.DrawRay(ray.origin, ray.direction, Color.magenta, 5f);*/
        /*StartCoroutine(DelayPosition(other));*/
    }


    void PlayBloodV1(Collider other)
    {
        var transformPosition = transform.position;
        var collisionPoint = other.ClosestPoint(transformPosition);
        var collisionNormal =  collisionPoint - transformPosition;
        var ray = new Ray(collisionPoint, collisionNormal);
        InstantiateBloodEffect(ray);
    }
    
    void PlayBloodV2(Collider other)
    {
        var myPosition = transform.position;
        var otherPosition = other.transform.position;
        var differencePosition = otherPosition - myPosition;
        differencePosition = differencePosition.normalized;

        var collisionPoint = other.ClosestPoint(myPosition);

        Ray ray = new Ray(collisionPoint, differencePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.green, 5f);

        InstantiateBloodEffect(ray);
    }
    
    private void OnTriggerStay(Collider other)
    {
        /*var delayPoint = other.ClosestPoint(transform.position);
        var collisionDirection = delayPoint - collisionPoint;
        var ray = new Ray(collisionPoint, collisionDirection);
        Debug.DrawRay(ray.origin, ray.direction, Color.green, 5f);
        InstantiateBloodEffect(ray);*/

        /*var myPosition = transform.position;
        var otherPosition = other.transform.position;
        var differencePosition = otherPosition - myPosition;
        differencePosition = differencePosition.normalized;

        var collisionPoint = other.ClosestPoint(myPosition);

        Ray ray = new Ray(collisionPoint, differencePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.green, 5f);

        InstantiateBloodEffect(ray);*/
        
        PlayBloodV1(other);
    }

    private void OnTriggerExit(Collider other)
    {
        PlayBloodV1(other);
        /*var myPosition = transform.position;
        var otherPosition = other.transform.position;
        var differencePosition = otherPosition - myPosition;
        differencePosition = differencePosition.normalized;

        var collisionPoint = other.ClosestPoint(myPosition);

        Ray ray = new Ray(collisionPoint, differencePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.green, 5f);

        InstantiateBloodEffect(ray);*/
    }

    public void InstantiateBloodEffect(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // var randRotation = new Vector3(0, Random.value * 360f, 0);
            // var dir = CalculateAngle(Vector3.forward, hit.normal);
            float angle = Mathf.Atan2(hit.normal.x, hit.normal.z) * Mathf.Rad2Deg + 180;

            //var effectIdx = Random.Range(0, BloodFX.Length);
            if (effectIdx == BloodFX.Length) effectIdx = 0;

            var instance = Instantiate(BloodFX[effectIdx], hit.point, Quaternion.Euler(0, angle + 90, 0));
            effectIdx++;

            var settings = instance.GetComponent<BFX_BloodSettings>();
            settings.DecalLiveTimeInfinite = InfiniteDecal;
            settings.LightIntensityMultiplier = DirLight.intensity;

            var nearestBone = GetNearestObject(hit.transform.root, hit.point);
            if (nearestBone != null)
            {
                var attachBloodInstance = Instantiate(BloodAttach);
                var bloodT = attachBloodInstance.transform;
                bloodT.position = hit.point;
                bloodT.localRotation = Quaternion.identity;
                bloodT.localScale = Vector3.one * Random.Range(0.75f, 1.2f);
                bloodT.LookAt(hit.point + hit.normal, direction);
                bloodT.Rotate(90, 0, 0);
                bloodT.transform.parent = nearestBone;
                Destroy(attachBloodInstance, 20);
            }

            if (!InfiniteDecal) Destroy(instance, 20);
        }
    }


    public float CalculateAngle(Vector3 from, Vector3 to) =>
        Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
}