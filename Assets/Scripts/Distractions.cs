using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distractions : MonoBehaviour
{
    public AudioSource rain;
    public AudioSource clock;

    public AudioClip rain_audio;
    public AudioClip clock_audio;

    public GameObject camera_obj;

    // Start is called before the first frame update
    void Start()
    {
        rain.loop = true;
        rain.volume = 0.1f;
        rain.clip = rain_audio;
        rain.Play();

        clock.loop = true;
        clock.volume = 0.04f;
        clock.clip = clock_audio;
    }

    // Update is called once per frame
    void Update()
    {
        if (camera_obj.transform.position.z < 10 && clock.isPlaying == false)
        {
            clock.Play();
        }
    }
}
