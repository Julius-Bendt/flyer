using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Juto;

public class StreakText : PoolObject {

    private TextMeshProUGUI text;
    private Transform rect;
    private bool active = false;
    private float time = 3f, delay = 1.5f;
    private float ElapsedTime = 0.0f;
    public Color color;

    private const float MINTIME = 1.5f, MAXTIME = 3, DELAYMIN = 1, DELAYMAX = 2.5f;

    public void SetPos(Vector3 pos, int i)
    {
        Transform parent = GameObject.Find("Canvas (camera space)").transform;
        transform.SetParent(parent, false);
        transform.position = pos;

        i++; //Arrays start at 0!
        text.text = i.ToString();
    }

    public override void OnObjectReuse()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();

        if (rect == null)
            rect = GetComponent<Transform>();

        time = Mathf.Clamp(MINTIME + (App.Instance.streak.Count / 3), MINTIME, MAXTIME);
        delay = Mathf.Clamp(DELAYMIN + (App.Instance.streak.Count / 4), DELAYMIN, DELAYMAX);
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(-40, 40));
        active = true;
        ElapsedTime = 0;
        text.color = color;
        rect.localScale = Vector3.one;
    }

    private void OnDisable()
    {
        active = false;
    }

    private void Update()
    {
        if(active)
        {
            if (ElapsedTime < (time+delay))
            {

                if(ElapsedTime > delay)
                {
                    if (text == null)
                        return;

                    
                    text.color = Color.Lerp(color, Color.clear, ((ElapsedTime - delay) / time));
                    rect.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, (ElapsedTime - delay) / time);
                }

                ElapsedTime += Time.deltaTime;
            }
            else
            {
                Destroy();
            }
        }
    }
}
