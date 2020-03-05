using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    public string parent;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
