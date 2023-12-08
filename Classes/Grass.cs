using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    System.Random random = new System.Random();

    public int reprod_time;
    public int live_time;
    void Start() {
        reprod_time = 10;
        live_time = 15;
        InvokeRepeating("CustUpdate", 1f, 1f);
    }
    protected void CustUpdate() {
        reprod_time--;
        live_time--;
        if(reprod_time <= 0) {
            GameObject child = Instantiate(gameObject);
            child.GetComponent<Grass>().reprod_time = random.Next(5, 15);
            reprod_time = random.Next(5, 15);
            child.transform.position = transform.position + new Vector3(0.01f, 0.01f, 0);
        }
        if(live_time <= 0) Destroy(gameObject);
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
}
