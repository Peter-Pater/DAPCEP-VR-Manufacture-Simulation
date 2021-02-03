using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabControl : MonoBehaviour
{
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
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Hand" && Input.GetMouseButton(0) && (GameObject.Find("Hand").GetComponent<Hand>().inHandObject == null || GameObject.Find("Hand").GetComponent<Hand>().inHandObject == this.gameObject))
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
            if (GameObject.Find("Hand").GetComponent<Hand>().inHandObject == null)
            {
                GameObject.Find("Hand").GetComponent<Hand>().inHandObject = this.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Hand" && GameObject.Find("Hand").GetComponent<Hand>().inHandObject == this.gameObject)
        {
            GameObject.Find("Hand").GetComponent<Hand>().inHandObject = null;
        }
    }
}
