using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabControl : MonoBehaviour
{
    private bool handDetected;
    private Quaternion inHandRotation;
    private GameObject grabPoint;
    // Start is called before the first frame update
    void Start()
    {
        grabPoint = GameObject.Find("GrabPoint");
        inHandRotation = this.gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (handDetected && Input.GetMouseButton(0))
        {
            this.gameObject.transform.position = grabPoint.transform.position;
            this.gameObject.transform.rotation = this.inHandRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Hand")
        {
            handDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Hand")
        {
            handDetected = false;
        }
    }
}
