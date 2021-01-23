using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicPlayer : MonoBehaviour {

    [HideInInspector]
    public AudioSource source;

    public void switchClip(UIManager.UIType state)
    {

        if(source == null)
            source = GetComponent<AudioSource>();

        if (App.Instance.settings.music)
        {

            source.loop = true;
            AudioClip clip;

            if (state == UIManager.UIType.game)
            {
                clip = App.Instance.audioDB.music;
            }
            else
            {
                clip = App.Instance.audioDB.title;
            }

            if (clip != source.clip)
            {
                source.clip = clip;
            }

            if (!source.isPlaying)
            {
                source.Play();
            }
        }
        else
        {
            source.Stop();
        }
    }
}
