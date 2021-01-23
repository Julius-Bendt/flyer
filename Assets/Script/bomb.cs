using Juto.Standard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using SpriteShatter; Paid asset
using Juto;

public class bomb : PoolObject {

    private const float SPEED = 1.25f;
    public GameObject explosion;

    private bool exploded = false;

    //private Shatter shatter;

    private void Start()
    {
        //shatter = GetComponent<Shatter>();
    }

    // Update is called once per frame
    void Update ()
    {
        transform.Translate(Vector3.right * SPEED * Time.deltaTime);	

        if(transform.position.y < -5.5f)
        {
            Destroy();
        }
	}

    private void OnTriggerEnter2D(Collider2D o)
    {
        if (o.tag == "Player" && !exploded)
        {
            if (App.Instance.isPlaying)
                FindObjectOfType<PlayerController>().TakeDamage();

            explode();
            
        }
    }

    public void explode()
    {
        if(!exploded)
        {
            AudioController.PlaySound(App.Instance.audioDB.explosion);
            App.Instance.pool.Instantiate(explosion, transform.position, Quaternion.identity);

            /*
            if (!shatter.shattered)
                shatter.shatter();
            */

            //App.Instance.pool.Destroy(gameObject, 4f);
            exploded = true;
        }
    }

    public override void OnObjectReuse()
    {
        /*
        if (shatter == null)
            shatter = GetComponent<Shatter>();

        if (shatter.shattered)
            shatter.reset();
        */

        exploded = false;
    }
}
