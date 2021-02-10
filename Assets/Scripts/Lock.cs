using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public bool isCatched = false;
    private Vector3 myPosition;
    private Quaternion myRotation;
    private GameObject floor;
    // Start is called before the first frame update
    void Start()
    {
        this.myPosition = this.gameObject.transform.position;
        this.myRotation = this.gameObject.transform.rotation;
        this.floor = GameObject.Find("Plane");
    }

    // Update is called once per frame
    void Update()
    {
        // prevent getting out of the world
        if (this.gameObject.transform.position.y < (floor.transform.position - Vector3.down).y)
        {
            this.gameObject.transform.position = this.myPosition;
            this.gameObject.transform.rotation = this.myRotation;
        }
    }
}
