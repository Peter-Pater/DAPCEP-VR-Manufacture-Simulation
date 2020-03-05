using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M : MonoBehaviour
{
    public int State; //1: preparing, 2: working1, 3: working2, 4: working3, 5:done, 6:fail
    private float start = 0;
    Main main;
    Vector3 M2_POS;
    
    // Start is called before the first frame update
    void Start()
    {
        State = 1;
        main = GameObject.Find("Plane").GetComponent<Main>();
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
            if (this.name == "Cube_1" || this.name == "Cube_2" || this.name == "Cube_3")
            {
                this.GetComponent<MeshRenderer>().enabled = true;
            }
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
            if (Time.time - start >= main.amount_of_work / GameObject.Find(GameObject.Find("slot1").GetComponent<slot>().catching).GetComponent<Attributes>().Calculate_score()[3])
            {
                State = 3;
                if (this.name == "M_2" || this.name == "M_3")
                {
                    this.GetComponent<MeshRenderer>().enabled = true;
                }
                string machine_name = GameObject.Find("slot2").GetComponent<slot>().catching;
                Vector3 target = GameObject.Find(machine_name + "workspace").transform.position;
                Vector3 dist = target - M2_POS;
                this.transform.position = this.transform.position + dist;
                M2_POS += dist;
                start = Time.time;
            }
        }else if(State == 3)
        {
            if (Time.time - start >= main.amount_of_work / GameObject.Find(GameObject.Find("slot2").GetComponent<slot>().catching).GetComponent<Attributes>().Calculate_score()[3])
            {
                State = 4;
                if (this.name == "Cube_5" || this.name == "Cube_6" || this.name == "Cube_7")
                {
                    this.GetComponent<MeshRenderer>().enabled = true;
                }
                string machine_name = GameObject.Find("slot3").GetComponent<slot>().catching;
                Vector3 target = GameObject.Find(machine_name + "workspace").transform.position;
                Vector3 dist = target - M2_POS;
                this.transform.position = this.transform.position + dist;
                M2_POS += dist;
                start = Time.time;
            }
        }else if(State == 4)
        {
            if (Time.time - start >= main.amount_of_work / GameObject.Find(GameObject.Find("slot2").GetComponent<slot>().catching).GetComponent<Attributes>().Calculate_score()[3])
            {
                State = 5;
                Vector3 target = GameObject.Find("Done").transform.position;
                Vector3 dist = target - M2_POS;
                this.transform.position = this.transform.position + dist;
                M2_POS += dist;
                if ((int)Random.Range(0, 2) == 1)
                {
                    float scaling;
                    if (main.effective_quality < 0.8)
                    {
                        scaling = main.effective_quality;
                    }else if(main.effective_quality < 0.9)
                    {
                        scaling = (main.effective_quality + 0.9f) / 2.0f;
                    }
                    else
                    {
                        scaling = 1.0f - (1.0f - main.effective_quality)/2.0f;
                    }
                    if ((int)Random.Range(0, 2) == 0) {
                        this.transform.localScale *= scaling;
                    }
                    else
                    {
                        this.transform.localScale /= scaling;
                    }
                }
                start = Time.time;
            }
        }
    }
}
