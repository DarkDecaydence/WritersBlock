using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource AudioPlayer;
    public List<AudioClip> MusicClips;
    public List<AudioClip> SoundClips;
    public Dictionary<string, AudioClip> dictSoundClips = new Dictionary<string, AudioClip>();

    void Awake()
    {
        GameData.audioManager = this;
        foreach (AudioClip ac in SoundClips) {
            dictSoundClips.Add(ac.name, ac);
        }
    }

    public void OneShotSound(string id)
    {
        AudioClip target;
        if (dictSoundClips.TryGetValue(id, out target)) {
            AudioPlayer.PlayOneShot(target);
        }
    }
}
