using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slot : MonoBehaviour
{
    public string catching = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (catching == "" && other.GetComponent<Lock>() != null && !other.GetComponent<Lock>().isCatched)
        {
            //Debug.Log("catched by " + this.name);
            other.GetComponent<Lock>().isCatched = true;
            catching = other.name;
            this.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (catching != "" && other.name == catching)
        {
            GameObject machine = other.gameObject;
            while (machine.transform.parent != null)
            {
                machine = machine.transform.parent.gameObject;
            }
            
            if (machine.name != "Player")
            {
                machine.transform.position = Vector3.MoveTowards(machine.transform.position, new Vector3(this.transform.position.x, machine.transform.position.y, this.transform.position.z), 0.1f);
                machine.transform.rotation = Quaternion.RotateTowards(machine.transform.rotation, this.transform.rotation, 2);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (catching != "" && other.GetComponent<Lock>() != null && other.GetComponent<Lock>().isCatched && other.name == catching)
        {
            other.GetComponent<Lock>().isCatched = false;
            catching = "";
            this.GetComponent<MeshRenderer>().enabled = true;
        }

    }
}
