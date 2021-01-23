using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Author: Julius Bendt
Created: 11/16/2018 6:35:29 PM
Project name: Juto Standard Asset
Company: Juto Studio
Unity Version: 2018.1.0f2


This script is under the CC0 license.
https://creativecommons.org/publicdomain/zero/1.0/

Credit isn't needed, but I would greatly appreciate it.
Give me credit as following:

"Using assets by Julius bendt,
https://www.juto.dk"

*/

namespace Juto.Standard
{
    public class AudioController : MonoBehaviour
    {
        /// <summary>
        /// Returns a GameObject with a AudioSource on.
        /// </summary>
        /// <param name="clip">The clip to be played</param>
        /// <returns></returns>
        public static GameObject PlaySound(AudioClip clip)
        {
            return PlaySound(clip, false, 1, true);
        }

        /// <summary>
        /// Returns a GameObject with a AudioSource on.
        /// </summary>
        /// <param name="clip">The clip to be played</param>
        /// <param name="loop">Should the sound loop</param>
        /// <param name="volume">Volume of the audiosource</param>
        /// <returns></returns>
        public static GameObject PlaySound(AudioClip clip, bool loop = false, float volume = 1)
        {
            return PlaySound(clip, loop, volume, true);
        }

        /// <summary>
        /// Returns a GameObject with a AudioSource on.
        /// </summary>
        /// <param name="clip">The clip to be played</param>
        /// <param name="loop">Should the sound loop</param>
        /// <param name="volume">Volume of the audiosource</param>
        /// <param name="_2d">is the sound 2d</param>
        /// <returns></returns>
        public static GameObject PlaySound(AudioClip clip, bool loop = false, float volume = 1, bool _2d = true)
        {

            if (!App.Instance.settings.sfx)
                return null;

            if (clip == null)
            {
                Debug.LogException(new System.Exception("Cannot play a null AudioClip!"));
                return null;
            }
                

            GameObject g = new GameObject("Audio source " + clip.name);

            AudioSource s = g.AddComponent<AudioSource>();

            s.loop = loop;
            s.clip = clip;
            s.volume = Mathf.Clamp(volume, 0, 1);
            s.spatialBlend = (_2d) ? 0 : 1;

            if (!loop)
            {
                Destroy(g, clip.length);
            }


            s.Play();

            return g;
        }
    }

}
