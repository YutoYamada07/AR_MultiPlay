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

    [Header("Detection")]
    [Range(0, 3)]
    public float detectionRadius = .32f;


    // Start is called before the first frame update
    void Start()
    {
        countDownTotal = explodeTime;

        //Facing Camera
        transform.LookAt(Camera.main.transform);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + Random.Range(-20, 20), 0);
    }


    #region Detection

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(bombRoot.transform.position, detectionRadius);
    }

    private void Update()
    {
        //if (!exploded)
        //    CheckOverlapping();
    }

    public void CheckOverlapping()
    {
        Collider[] colliders = Physics.OverlapSphere(bombRoot.transform.position, detectionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Player"))
            {
                //Debug.Log("nearbyObject.tag: " + nearbyObject.name);
                PlayerBehaviour pB = nearbyObject.GetComponentInParent<PlayerBehaviour>();
                pB.bombNearby = true;
                GameVal.gc.AddScore();
            }
        }
    }


    #endregion









    #region Explosion


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
            CheckOverlapping();

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

    #endregion
}


