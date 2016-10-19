using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource AudioPlayer;

    public List<AudioClip> MusicClips;

    private Dictionary<string, AudioClip> dictSpellClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> dictPlayerClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> dictMonsterClips = new Dictionary<string, AudioClip>();

    private AudioClip victoryClip;

    void Awake()
    {
        GameData.audioManager = this;

        Debug.Log("Loading spell audio...");
        var spells = Resources.LoadAll<AudioClip>("Audio/Sounds/Spells");
        foreach (AudioClip ac in spells) {
            dictSpellClips.Add(ac.name, ac);
        }

        Debug.Log("Loading player audio...");
        var player = Resources.LoadAll<AudioClip>("Audio/Sounds/Player");
        foreach (AudioClip ac in player) {
            dictPlayerClips.Add(ac.name, ac);
        }

        Debug.Log("Loading monsters audio...");
        var monsters = Resources.LoadAll<AudioClip>("Audio/Sounds/Monsters");
        foreach (AudioClip ac in monsters) {
            dictMonsterClips.Add(ac.name, ac);
        }

        Debug.Log("Loading remaining audio...");
        victoryClip = Resources.Load<AudioClip>("Audio/Sounds/NewFloorSound");
    }

    

    public void PlaySpell(string id)
    {
        AudioClip target;
        if (dictSpellClips.TryGetValue(id, out target)) {
            AudioPlayer.PlayOneShot(target);
        } else {
            Debug.LogWarning(string.Format("No spell with ID '{0}' available.", id));
        }
    }

    public void PlayPlayer(string id)
    {
        AudioClip target;
        if (dictPlayerClips.TryGetValue(id, out target)) {
            AudioPlayer.PlayOneShot(target);
        } else {
            Debug.LogWarning(string.Format("No player sound with ID '{0}' available.", id));
        }
    }

    public void PlayMonster(string id)
    {
        AudioClip target;
        if (dictMonsterClips.TryGetValue(id, out target)) {
            AudioPlayer.PlayOneShot(target);
        } else {
            Debug.LogWarning(string.Format("No monster sound with ID '{0}' available.", id));
        }
    }

    public void PlayVictory()
    {
        AudioPlayer.PlayOneShot(victoryClip);
    }
}
