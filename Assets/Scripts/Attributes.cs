using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour
{
    public float purchase_cost;
    public float setup_cost;
    public float power_rating;
    public float nominal_cycle_time;
    public float nominal_quality;
    public float quality_std;
    public float precision;
    public bool debug;

    // Start is called before the first frame update
    void Start()
    {
        this.Calculate_score();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float[] Calculate_score()
    {
        float[] attri = new float[4];
        attri[0] = gaussian(this.nominal_quality, this.quality_std); // effective quality
        attri[1] = gaussian(this.nominal_cycle_time + attri[0], Mathf.Pow(1.0f - attri[0], 2)); // machine cycle time
        attri[2] = this.purchase_cost + this.setup_cost + this.power_rating * attri[1]; // machine cost
        attri[3] = (attri[0] * this.nominal_cycle_time) / attri[1]; // machine effectiveness
        if (debug)
        {
            Debug.Log("effective quality:" + attri[0].ToString());
            Debug.Log("machine cycle time:" + attri[1].ToString());
            Debug.Log("machine cost:" + attri[2].ToString());
            Debug.Log("machine effectiveness:" + attri[3].ToString());
            Debug.Log("time takes:" + (5 / attri[3]).ToString());
        }
        return attri;
    }

    float gaussian(float mean, float std)
    {
        float u1 = 1.0f - Random.Range(0.0f, 1.0f);
        float u2 = 1.0f - Random.Range(0.0f, 1.0f);
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + std * randStdNormal;
    }
}
