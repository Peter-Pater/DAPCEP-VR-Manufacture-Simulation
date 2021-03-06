using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public float amount_of_work;
    public float budget = 1100;
    public float power = 1000;
    public float final_score;
    public float final_power;
    public float effective_quality;
    public string total_cost;
    public string total_time;
    bool scored = false;
    List<float> budget_scores = new List<float>();
    List<float> throughput_scores = new List<float>();
    List<float> efficiency_scores = new List<float>();
    List<float> total_power = new List<float>();
    List<float> scores = new List<float>();
    Dictionary<string, float> score_map = new Dictionary<string, float>();

    // answer: 3, 2, 2

    // Start is called before the first frame update
    void Start()
    {
        this.possible_comb();
        GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "hahaha";
    }

    // Update is called once per frame
    void Update()
    {
        // not for webgl
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //}
        if (GameObject.Find("M_2").GetComponent<M>().State == 1) {
            scored = false;
        }
        if (GameObject.Find("M_2").GetComponent<M>().State == 2 && !scored)
        {
            Attributes machine1 = GameObject.Find(GameObject.Find("slot1").GetComponent<slot>().catching).GetComponent<Attributes>();
            Attributes machine2 = GameObject.Find(GameObject.Find("slot2").GetComponent<slot>().catching).GetComponent<Attributes>();
            Attributes machine3 = GameObject.Find(GameObject.Find("slot3").GetComponent<slot>().catching).GetComponent<Attributes>();

            float[] machine1_score = machine1.Calculate_score();
            float[] machine2_score = machine2.Calculate_score();
            float[] machine3_score = machine3.Calculate_score();

            float raw_power = (this.amount_of_work / machine1_score[3]) * machine1.power_rating +
                    (this.amount_of_work / machine2_score[3]) * machine2.power_rating +
                    (this.amount_of_work / machine3_score[3]) * machine3.power_rating;
            float budget_score;
            if ((machine1_score[2] + machine2_score[2] + machine3_score[2]) > this.budget)
            {
                budget_score = this.budget / (machine1_score[2] + machine2_score[2] + machine3_score[2]);
            }
            else
            {
                if (this.budget - (machine1_score[2] + machine2_score[2] + machine3_score[2]) < this.budget * 0.1)
                {
                    budget_score = this.budget / (machine1_score[2] + machine2_score[2] + machine3_score[2]);
                }
                else
                {
                    budget_score = 1;
                }
            }
            effective_quality = (machine1_score[0] + machine2_score[0] + machine3_score[0]) / 3.0f;
            float raw_score = budget_score + 
                            + 1 / (machine1_score[1] + machine2_score[1] + machine3_score[1])
                            + machine1_score[3] * machine2_score[3] * machine3_score[3];
            float raw_scaled_score = (this.scale(this.budget_scores, budget_score, 100, this.budget_scores.ToArray().Length) +
                                 this.scale(this.throughput_scores, 1 / (machine1_score[1] + machine2_score[1] + machine3_score[1]), 100, this.throughput_scores.ToArray().Length) +
                                 this.scale(this.efficiency_scores, machine1_score[3] * machine2_score[3] * machine3_score[3], 100, this.efficiency_scores.ToArray().Length)) / 3.0f;

            final_score = this.scale(this.scores, raw_scaled_score, 100, this.scores.ToArray().Length);
            final_power = scale(this.total_power, raw_power, 100, this.total_power.ToArray().Length);
            total_cost = (machine1_score[2] + machine2_score[2] + machine3_score[2]).ToString();
            total_time = (this.amount_of_work / machine1_score[3] + this.amount_of_work / machine2_score[3] + this.amount_of_work / machine3_score[3]).ToString() + " seconds";

            print("Average effective quality: " + effective_quality);
            print("total cost: " + total_cost);
            print("time taken: " + total_time);
            print("raw_power: " + raw_power.ToString() + ", raw_score: " + raw_scaled_score.ToString());
            print("scaled_power: " + final_power.ToString() + ", scaled_score: " + final_score.ToString());

            // simplified
            float total_cost_num = machine1.purchase_cost + machine2.purchase_cost + machine3.purchase_cost;
            float total_time_num = machine1.nominal_cycle_time + machine2.nominal_cycle_time + machine3.nominal_cycle_time;
            this.total_cost = total_cost_num.ToString();
            this.total_time = total_time_num.ToString();
            effective_quality = (gaussian(machine1.nominal_quality, machine1.quality_std) +
                                gaussian(machine2.nominal_quality, machine2.quality_std) +
                                gaussian(machine3.nominal_quality, machine3.quality_std)) / 3.0f;
            final_score = Mathf.Max(this.budget - total_cost_num, 0) / 200 + 8.0f / total_time_num + effective_quality;
            scored = true;
        }
    }

    float gaussian(float mean, float std)
    {
        float u1 = 1.0f - Random.Range(0.0f, 1.0f);
        float u2 = 1.0f - Random.Range(0.0f, 1.0f);
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + std * randStdNormal;
    }

    float scale(List<float> range, float score, float highest, float size)
    {
        List<float> sorted_range = new List<float>();
        for (int i = 0; i < size; i++)
        {
            sorted_range.Add(range[i]);
        }
        sorted_range.Sort();
        for (int i = 0; i < size; i++)
        {
            if (score <= sorted_range[i])
            {
                return highest * (i / size);
            }
        }
        return highest;
    }

    void possible_comb()
    {
        float power = 0;
        //GameObject[] machines = GameObject.FindGameObjectsWithTag("machines");
        GameObject[] smallPrinters = GameObject.FindGameObjectsWithTag("smallPrinter");
        GameObject[] smallbots = GameObject.FindGameObjectsWithTag("smallBot");
        GameObject[] largePrinters = GameObject.FindGameObjectsWithTag("largePrinter");

        for (int i = 0; i < smallPrinters.Length; i++)
        {
            power = 0;
            float[] system_costs = new float[3];
            float[] system_throughputs = new float[3];
            float[] system_efficiencies = new float[3];
            system_costs[0] = smallPrinters[i].GetComponent<Attributes>().Calculate_score()[2];
            system_throughputs[0] = smallPrinters[i].GetComponent<Attributes>().Calculate_score()[1];
            system_efficiencies[0] = smallPrinters[i].GetComponent<Attributes>().Calculate_score()[3];

            for (int j = 0; j < smallbots.Length; j++)
            {
                system_costs[1] = smallbots[j].GetComponent<Attributes>().Calculate_score()[2];
                system_throughputs[1] = smallbots[j].GetComponent<Attributes>().Calculate_score()[1];
                system_efficiencies[1] = smallbots[j].GetComponent<Attributes>().Calculate_score()[3];

                for (int k = 0; k < largePrinters.Length; k++)
                {
                    system_costs[2] = largePrinters[k].GetComponent<Attributes>().Calculate_score()[2];
                    system_throughputs[2] = largePrinters[k].GetComponent<Attributes>().Calculate_score()[1];
                    system_efficiencies[2] = largePrinters[k].GetComponent<Attributes>().Calculate_score()[3];
                    power = (this.amount_of_work / system_efficiencies[0]) * smallPrinters[i].GetComponent<Attributes>().power_rating +
                            (this.amount_of_work / system_efficiencies[1]) * smallbots[j].GetComponent<Attributes>().power_rating +
                            (this.amount_of_work / system_efficiencies[2]) * largePrinters[k].GetComponent<Attributes>().power_rating;
                    float budget_score;
                    if ((system_costs[0] + system_costs[1] + system_costs[2]) > this.budget)
                    {
                        budget_score = this.budget / (system_costs[0] + system_costs[1] + system_costs[2]);
                    }
                    else
                    {
                        if (this.budget - (system_costs[0] + system_costs[1] + system_costs[2]) < this.budget * 0.1){
                            budget_score = this.budget / (system_costs[0] + system_costs[1] + system_costs[2]);
                        }
                        else
                        {
                            budget_score = 1;
                        }
                    }
                    this.budget_scores.Add(budget_score);
                    this.throughput_scores.Add(1 / (system_throughputs[0] + system_throughputs[1] + system_throughputs[2]));
                    this.efficiency_scores.Add(system_efficiencies[0] * system_efficiencies[1] * system_efficiencies[2]);
                    this.total_power.Add(power);
                }
            }
        }

        for (int i = 0; i < this.budget_scores.ToArray().Length; i++)
        {
            this.scores.Add((this.scale(this.budget_scores, this.budget_scores[i], 100, this.budget_scores.ToArray().Length) +
                            this.scale(this.throughput_scores, this.throughput_scores[i], 100, this.throughput_scores.ToArray().Length) +
                            this.scale(this.efficiency_scores, this.efficiency_scores[i], 100, this.efficiency_scores.ToArray().Length)) / 3.0f);
        }

        this.scores.Sort();
        this.budget_scores.Sort();
        this.throughput_scores.Sort();
        this.efficiency_scores.Sort();
        this.total_power.Sort();

        foreach (float e in this.budget_scores)
        {
            print(e);
        }
        print("raw budget_scores from " + this.budget_scores[0].ToString() + " to " + this.budget_scores[this.budget_scores.ToArray().Length - 1].ToString());
        print("raw throughput_scores from " + this.throughput_scores[0].ToString() + " to " + this.throughput_scores[this.throughput_scores.ToArray().Length - 1].ToString());
        print("raw efficiency_scores from " + this.efficiency_scores[0].ToString() + " to " + this.efficiency_scores[this.efficiency_scores.ToArray().Length - 1].ToString());
        print("raw scores from " + this.scores[0].ToString() + " to " + this.scores[this.scores.ToArray().Length - 1].ToString());
        print("raw power from " + this.total_power[0].ToString() + " to " + this.total_power[this.total_power.ToArray().Length - 1].ToString());
    }
}
