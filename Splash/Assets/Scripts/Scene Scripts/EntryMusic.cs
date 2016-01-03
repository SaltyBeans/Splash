using UnityEngine;
using System.Collections;

public class EntryMusic : MonoBehaviour {

    public AudioClip[] entryMusic;
    AudioSource source;
	
	void Start () {

        //entryMusic = Resources.LoadAll("Music") as AudioClip[];

        source = GetComponent<AudioSource>();

        int i = Random.Range(0, entryMusic.Length);

        source.clip = entryMusic[i];
        source.playOnAwake = true;
        source.Play();
        source.volume = 0.25f;
	}

}
