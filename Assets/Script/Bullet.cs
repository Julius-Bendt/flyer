using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Juto.Standard;
using Juto;

public class Bullet : PoolObject {

    private const float SPEED = 7.5f, MINRAN = -1.25f, MAXRAN = 1.25f;
    private const float DISTANCE = 1.5f;

    public GameObject explosion;

    bool fingerBeenDown = false, fingerBeenUp = false;

	// Update is called once per frame
	void Update ()
    {
        if(App.Instance.isPlaying)
        {
            transform.Translate((Vector3.up * SPEED + (Vector3.right * Random.Range(MINRAN, MAXRAN))) * Time.deltaTime);

#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetButtonDown("Fire"))
                Explode();
#else
    if (Input.touches.Length < 2) // 1 finger
            {
                Debug.Log("Finger up");
                fingerBeenDown = true;

                if(fingerBeenUp)
                    Explode();
            }
            else // 2 eller over
            {
                Debug.Log("Finger down");

                if (fingerBeenDown)
                    fingerBeenUp = true;
            }
#endif

            if (transform.position.x > 9 || transform.position.x < -9 ||transform.position.y > 5.5f)
            {
                Destroy();
            }
        }
        else
        {
            Destroy();
        }
	}

    public void Explode()
    {
        int hit;

        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            if (Vector2.Distance(e.transform.position, transform.position) < DISTANCE)
            {
                e.Kill();
            }
        }

        foreach (bomb b in FindObjectsOfType<bomb>())
        {
            if (Vector2.Distance(b.transform.position, transform.position) < DISTANCE)
            {
                b.explode();
            }
        }

        AudioController.PlaySound(App.Instance.audioDB.explosion);
        App.Instance.pool.Instantiate(explosion, transform.position, Quaternion.identity);

        fingerBeenDown = false;
        fingerBeenUp = false;

        Destroy();
    }
}
