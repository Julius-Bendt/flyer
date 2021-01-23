using Juto.Standard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Camera cam;

    private int ammo = 10;

    public GameObject bulletPrefab;

    public Transform shootPos;

    public GameObject activeBullet;

    private int health = 3;

    [HideInInspector]
    private const float ROTATETIME = 1;

    public GameObject[] fire;

    bool fingerDown = true;
    Vector3 rotationPos = Vector3.zero;

    public int Ammo
    {
        get
        {
            return ammo;
        }

        set
        {
            ammo = value;
            App.Instance.uiManager.UpdateGameAmmo(ammo);

        }
    }

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
            if (Health == 3)
                return;

            fire[health].SetActive(true);
            fire[health].GetComponent<Animator>().enabled = true;
        }
    }

    public bool canShoot
    {
        get
        {
            if (activeBullet == null)
                return true;

            return !activeBullet.activeSelf;
        }
    }

    public void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(-65, 65));
        StartCoroutine(rotateCanonForCosmetics());
    }

    public void Update()
    {
        if(App.Instance.isPlaying)
        {
            Rotate();

            if (canShoot)
            {

#if UNITY_EDITOR || UNITY_STANDALONE
                if (Input.GetButtonDown("Fire"))
                    Shoot();
#else
                  if (Input.touches.Length < 2)
                    fingerDown = false;
                else if (Input.touches[1].phase == TouchPhase.Ended)
                    fingerDown = false;

                bool canFire = (Input.touches.Length > 1);

                if (canFire && !fingerDown)
                {
                    fingerDown = true;
                    Shoot();
                }
#endif



            }


            if (activeBullet != null)
                if (ammo == 0 && !activeBullet.activeSelf) //die condition
                    App.Instance.StopGame();
        }
    }

    IEnumerator rotateCanonForCosmetics()
    {
        while(true)
        {
            if(!App.Instance.isPlaying)
            {
                float ElapsedTime = 0.0f;
                Quaternion startingRotation = transform.rotation;
                Quaternion rotateTo = Quaternion.Euler(0, 0, Random.Range(-65, 65));

                while (ElapsedTime < ROTATETIME)
                {
                    ElapsedTime += Time.deltaTime;
                    transform.rotation = Quaternion.Lerp(startingRotation,rotateTo,(ElapsedTime/ROTATETIME));
                    yield return null;
                }

                yield return new WaitForSeconds(Random.Range(2f,4f));
            }

            yield return null;
        }
    }



    public void StartGame()
    {
        Ammo = App.Instance.upgrades.maxAmmo;
        Health = 3;

        foreach (GameObject f in fire)
        {
            f.GetComponent<Animator>().enabled = false;
            f.SetActive(false);
        }
    }


    public void Rotate()
    {


#if UNITY_EDITOR || UNITY_STANDALONE
        rotationPos = Input.mousePosition;
#else
    if (Input.touches.Length > 0)
            rotationPos = Input.touches[0].position;
#endif

        Vector3 diff = cam.ScreenToWorldPoint(rotationPos) - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(rot_z - 90,-50,50));
    }

    public void Shoot()
    {
        AudioController.PlaySound(App.Instance.audioDB.shot);
        activeBullet = App.Instance.pool.Instantiate(bulletPrefab, shootPos.position, Quaternion.identity);
        Ammo--;
        activeBullet.transform.up = transform.up;

        StartCoroutine(ShootEffect());
    }

    IEnumerator ShootEffect()
    {
        float ElapsedTime = 0.0f;
        const float TIME = 0.1f;
        const float MOVE = 0.4f;

        Vector3 down = transform.localPosition, up = transform.localPosition;

        while (ElapsedTime < TIME)
        {
            ElapsedTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(up, (down - transform.up * MOVE), (ElapsedTime / TIME));
            yield return null;
        }

        Vector3 startPos = transform.localPosition;
        ElapsedTime = 0;
        while (ElapsedTime < TIME)
        {
            ElapsedTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPos, up, (ElapsedTime / TIME));
            yield return null;
        }
    }

    //die condition
    public void TakeDamage()
    {
        if(App.Instance.isPlaying)
        {
            Health--;

            if (Health <= 0)
                App.Instance.StopGame();

            if (App.Instance.settings.shake)
                App.Instance.shake.TriggerShake(0.4f);
        }
    }

}
