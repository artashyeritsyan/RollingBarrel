using System.Collections.Generic;
using UnityEngine;

public class FootstepsController : MonoBehaviour
{
    [Header("Footsteps")]
    public AudioSource walkSounds;
    public List<AudioClip> footsteps;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFootsteps()
    {
        walkSounds.clip = footsteps[Random.Range(0, footsteps.Count)];
        //walkSounds.volume = Random.Range(0.02f, 0.05f);
        walkSounds.volume = 0.05f;
        walkSounds.pitch = Random.Range(0.8f, 1.2f);
        walkSounds.Play();
    }
}
