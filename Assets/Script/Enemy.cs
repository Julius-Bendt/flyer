using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using SpriteShatter; Paid asset
using Juto;

public class Enemy : PoolObject {

    public Type type;

    public int dir = 1;

    public const float SPEEDMIN = 1.5f, SPEEDMAX = 2.5f;

    bool droppedBomb = false;
    public GameObject bombPrefab, explosion;
    public Transform dropPos;

    private const float DISTANCE = 1.25f;

    //private Shatter shatter;

    private bool killed = false;
    private float speed;
    public enum Type
    {
        enemy,
        civ,
        ally
    };

    private void Start()
    {
        //shatter = GetComponent<Shatter>();
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.right * speed * dir * Time.deltaTime);

        if (transform.position.x < -10 || transform.position.x > 10)
            Destroy();


        if(!killed)
        {
            if (transform.position.x > -0.5f && transform.position.x < 0.5f && !droppedBomb && App.Instance.isPlaying)
            {
                droppedBomb = true;
                App.Instance.pool.Instantiate(bombPrefab, transform.position, Quaternion.Euler(0, 0, -90));
            }
        }
	}

    public void Init(Vector3 pos, Quaternion rotation, int _dir)
    {
        speed = Random.Range(SPEEDMIN, SPEEDMAX);
        transform.position = pos;
        transform.rotation = rotation;
        dir = _dir;
    }

    public void Kill()
    {
        if (!killed)
        {
            killed = true;
            App.Instance.streak.Add(transform.position);
            App.Instance.streakCooldown = 0.3f;
            App.Instance.enemiesKilled++;
            App.Instance.Score += 100 * (int)Mathf.Clamp(App.Instance.streak.Count, 1, Mathf.Infinity);

            App.Instance.pool.Instantiate(explosion, transform.position, Quaternion.identity);

            if (App.Instance.settings.shake)
                App.Instance.shake.TriggerShake(Mathf.Clamp(0.25f * (App.Instance.streak.Count / 2), 0.25f, 1));

            App.Instance.uiManager.MakeStreakText();

            /*
            if (!shatter.shattered)
                shatter.shatter();
            */

            foreach (Enemy e in FindObjectsOfType<Enemy>())
            {
                if (e == this || e.killed)
                    continue;

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

            App.Instance.pool.Destroy(gameObject, 1.5f);
        }
    }

    public override void OnObjectReuse()
    {
        /*
        if(shatter == null)
            shatter = GetComponent<Shatter>();

        if (shatter.shattered)
            shatter.reset();
        */

        killed = droppedBomb = false;
    }
}
