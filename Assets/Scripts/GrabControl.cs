using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabControl : MonoBehaviour
{
    private Quaternion inHandRotation;
    private GameObject grabPoint;
    private GameObject hand;
    public bool handDetected;

    public int grabState; // 0: nothing, 1: grabing, 2: releasing (not used)


    // Start is called before the first frame update
    void Start()
    {
        this.grabPoint = GameObject.Find("GrabPoint");
        this.hand = GameObject.Find("Hand");
        this.inHandRotation = this.gameObject.transform.rotation;
        this.grabState = 0;
        this.handDetected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((this.grabState == 0) && this.handDetected && Input.GetMouseButtonDown(0) && this.hand.GetComponent<Hand>().inHandObject == null)
        {
            this.grabState = 1;
            this.hand.GetComponent<Hand>().inHandObject = this.gameObject;
        }
        else if (this.grabState == 1 && this.hand.GetComponent<Hand>().inHandObject == this.gameObject)
        {
            if (this.gameObject.name == "SMALLBOT_A1P2_02" || this.gameObject.name == "SMALLBOT_A1P2_03")
            {
                this.gameObject.transform.position = grabPoint.transform.position - 0.8f * Vector3.down;
            }
            else
            {
                this.gameObject.transform.position = grabPoint.transform.position;
            }
            this.gameObject.transform.rotation = this.inHandRotation;

            if (Input.GetMouseButtonDown(0))
            {
                this.grabState = 0;
                this.hand.GetComponent<Hand>().inHandObject = null;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == this.hand && this.hand.GetComponent<Hand>().inHandObject == null)
        {
            this.handDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == this.hand)
        {
            //if (this.grabState == 2 && this.hand.GetComponent<Hand>().inHandObject == this.gameObject)
            //{
            //    this.hand.GetComponent<Hand>().inHandObject = null;
            //    this.grabState = 0;
            //}
            this.handDetected = false;
        }
    }
}
