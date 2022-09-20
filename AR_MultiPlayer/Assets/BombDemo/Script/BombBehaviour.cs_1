using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    [Header("State")]
    public bool exploded;

    [Header("Object")]
    public GameObject bombRoot;

    [Header("Explosion")]
    public GameObject explosionEffect;

    public float explodeTime = 1;
    private float countingDownNow;
    private float countDownTotal;


    // Start is called before the first frame update
    void Start()
    {
        countDownTotal = explodeTime;

        //Facing Camera
        transform.LookAt(Camera.main.transform);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + Random.Range(-20, 20), 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Counting();
    }
    public void Explode()
    {
        if (!exploded)
        {
            exploded = true;
            //Debug.Log("Explode");

            //Spawn effect
            GameObject exEff = Instantiate(explosionEffect, transform.position, transform.rotation);
            exEff.transform.position = bombRoot.transform.position;
            exEff.transform.localScale = transform.localScale;
            exEff.SetActive(true);

            //Destroy this object
            Destroy(gameObject);
        }
    }

    public void Counting()
    {
        if (countingDownNow < countDownTotal)
            countingDownNow += Time.deltaTime;
        else
            Explode();//count reached
    }
}

