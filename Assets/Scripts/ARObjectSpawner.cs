using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && spawnedObject == null)
            {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    spawnedObject = Instantiate(objectToSpawn, hitPose.position, hitPose.rotation);
                }
            }

            if (spawnedObject != null && touch.phase == TouchPhase.Moved && Input.touchCount == 1)
            {
                spawnedObject.transform.Rotate(0, touch.deltaPosition.x * 0.1f, 0);
            }

            if (spawnedObject != null && Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
                float prevTouchDeltaMag = (touchZero.position - touchOne.position).magnitude;
                float touchDeltaMag = ((touchZero.position + touchZero.deltaPosition) - (touchOne.position + touchOne.deltaPosition)).magnitude;
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                spawnedObject.transform.localScale -= new Vector3(deltaMagnitudeDiff * 0.01f, deltaMagnitudeDiff * 0.01f, deltaMagnitudeDiff * 0.01f);
            }
        }
    }
}
