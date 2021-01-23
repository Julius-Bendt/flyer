using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public Vector2 sides; // x = right, y = left
    public Vector2 minMax; // x = min, y = max

    private const float TTSMIN = 0.5f, TTSMAX = 1.5f; 

    private float timeToSpawn;

    public GameObject plane;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Spawn());
    }


    public IEnumerator Spawn()
    {
        while(true)
        {

            if (App.Instance.isPlaying)
                timeToSpawn = Random.Range(TTSMIN, TTSMAX);
            else
                timeToSpawn = Random.Range(4f, 5.5f);

            yield return new WaitForSeconds(timeToSpawn);

            GameObject p = App.Instance.pool.Instantiate(plane);
            Transform pTrans = p.transform;

            float x, y;
            int dir = 1;
            Quaternion rotation = Quaternion.identity;

            if (Random.Range(0, 100) >= 50)
            {
                x = sides.x;
            }
            else
            {
                x = sides.y;
                dir = 1;
                rotation = Quaternion.Euler(0, 180, 0);
            }
            y = Random.Range(minMax.x, minMax.y);

            Vector3 pos = new Vector3(x, y, 0);
            p.GetComponent<Enemy>().Init(pos, rotation, dir);
               
            

            yield return null;
        }
    }
}
