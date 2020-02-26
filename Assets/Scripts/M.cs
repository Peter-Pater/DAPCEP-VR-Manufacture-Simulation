using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M : MonoBehaviour
{
    public int State = 1; //1: preparing, 2: working1, 3: working2, 4: working3, 5:done
    private float start = 0;
    Vector3 M2_POS;
    
    // Start is called before the first frame update
    void Start()
    {
        M2_POS = GameObject.Find("M_2").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("slots")[0].GetComponent<MeshRenderer>().enabled ||
            GameObject.FindGameObjectsWithTag("slots")[1].GetComponent<MeshRenderer>().enabled ||
            GameObject.FindGameObjectsWithTag("slots")[2].GetComponent<MeshRenderer>().enabled)
        {
            State = 1;
        }
        else if(State == 1)
        {
            State = 2;
            this.GetComponent<MeshRenderer>().enabled = true;
            string machine_name = GameObject.Find("slot1").GetComponent<slot>().catching;
            Vector3 target = GameObject.Find(machine_name + "workspace").transform.position;
            Vector3 dist = target - M2_POS;
            this.transform.position = this.transform.position + dist;
            M2_POS += dist;
            start = Time.time;
        }
        if (State == 1)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }else if(State == 2)
        {
            if (Time.time - start >= 5)
            {
                State = 3;
                this.GetComponent<MeshRenderer>().enabled = true;
                string machine_name = GameObject.Find("slot2").GetComponent<slot>().catching;
                Vector3 target = GameObject.Find(machine_name + "workspace").transform.position;
                Vector3 dist = target - M2_POS;
                this.transform.position = this.transform.position + dist;
                M2_POS += dist;
                start = Time.time;
            }
        }else if(State == 3)
        {
            if (Time.time - start >= 5)
            {
                State = 4;
                this.GetComponent<MeshRenderer>().enabled = true;
                string machine_name = GameObject.Find("slot3").GetComponent<slot>().catching;
                Vector3 target = GameObject.Find(machine_name + "workspace").transform.position;
                Vector3 dist = target - M2_POS;
                this.transform.position = this.transform.position + dist;
                M2_POS += dist;
                start = Time.time;
            }
        }else if(State == 4)
        {
            if (Time.time - start >= 5)
            {
                State = 5;
                Vector3 target = GameObject.Find("Done").transform.position;
                Vector3 dist = target - M2_POS;
                this.transform.position = this.transform.position + dist;
                M2_POS += dist;
                start = Time.time;
            }
        }
    }
}
