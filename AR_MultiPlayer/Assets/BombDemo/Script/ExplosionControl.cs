using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionControl : MonoBehaviour
{
    //public GameObject[] toDelete;
    public float countdown = .75f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDownSet());
    }

    IEnumerator CountDownSet()
    {
        yield return new WaitForSeconds(countdown);
        //Debug.Log("Bye Bye Bye Bye Bye Bye Bye Bye ");
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("explosionControl");
        //for (var i = 0; i < transform.childCount; i++)
        //{
        //    countdowns[i] -= Time.deltaTime;
        //    if (countdowns[i] <= 0)
        //        Destroy(ps[i].gameObject);
        //}
        //if (transform.childCount == 0)
        //    Destroy(this.gameObject);

    }
}
