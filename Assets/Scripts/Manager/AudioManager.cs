﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    
    public SoundEffect[] soundEffects;

    private IEnumerator coroutineToStop;

    public bool FadeInOver = false;
    public bool canPlayMusic = false;

    public System.Action<int> onMusicPallierChanged;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        foreach (SoundEffect fx in soundEffects)
        {
            fx.source = gameObject.AddComponent<AudioSource>();
            fx.source.volume = fx.volume;
            fx.source.priority = fx.priority;
            fx.source.loop = fx.loop;
        }
    }

    private void Start()
    {
        MiniGameManager.instance.onChangeState += () =>
        {
            if (MiniGameManager.instance.state == State.NONE)
            {
                StartCoroutine(FadeOutBeforeNewMusic("Music", "Music", 0));
            }
            else if(MiniGameManager.instance.state == State.DEAD)
            {
                Stop("Music");
            }
            else
            {
                if (MiniGameManager.instance.previousState == State.NONE)
                    StartCoroutine(FadeOutBeforeNewMusic("Music", "Music", PlayerEntity.instance.previousPallier));
            }
        };

        onMusicPallierChanged += (int index) =>
        {
            print("change  " + index);
            Play("Music", index, true);
        };

        Play("Music", 0, false);
    }

    public void Play(string name)
    {
        SoundEffect fx = Array.Find(soundEffects, sound => sound.clipName == name);
        if (fx == null)
        {
            Debug.Log("/!\\ Sound : " + name + "not found /!\\");
            return;
        }
        fx.source.clip = fx.clip[UnityEngine.Random.Range(0, fx.clip.Length)];
        fx.source.Play();
    }

    public void Play(string name, int index, bool shouldFlowWithFirstMusic)
    {
        SoundEffect fx = Array.Find(soundEffects, sound => sound.clipName == name);
        if (fx == null)
        {
            Debug.Log("/!\\ Sound : " + name + "not found /!\\");
            return;
        }
        float temp = fx.source.time;
        fx.source.clip = fx.clip[index];
        if (shouldFlowWithFirstMusic)
        {
            fx.source.time = temp;
        }
        fx.source.Play();
    }

    public bool CheckAudioClip(int index)
    {
        SoundEffect fx = Array.Find(soundEffects, sound => sound.clipName == "Music");
        if(fx.source.clip == fx.clip[index])
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void Stop(string name)
    {
        SoundEffect fx = Array.Find(soundEffects, sound => sound.clipName == name);
        if (fx == null)
        {
            Debug.Log("/!\\ Sound : " + name + "not found /!\\");
            return;
        }
        fx.source.Stop();
    }

    public IEnumerator FadeOutBeforeNewMusic(string sourceToLower, string music, int index)
    {
        canPlayMusic = false;
        AudioSource fxSource = Array.Find(soundEffects, sound => sound.clipName == sourceToLower).source;
        if (fxSource.volume - 0.2f >= 0)
            fxSource.volume -= 0.2f;
        yield return new WaitForSeconds(0.3f);
        if(fxSource.volume <= 0.2f)
        {
            fxSource.volume = 0;
            StopCoroutine(FadeOutBeforeNewMusic(sourceToLower, music, index));
            Play(music, index, false);
            
            StartCoroutine(FadeInNewMusic("Music"));
        }
        else
        {
            StartCoroutine(FadeOutBeforeNewMusic(sourceToLower, music, index));
        }
    }

    private IEnumerator FadeInNewMusic(string sourceToUp)
    {
        AudioSource fxSource = Array.Find(soundEffects, sound => sound.clipName == sourceToUp).source;
        yield return new WaitForSeconds(0.3f);
        if (fxSource.volume + 0.2f <= 1)
        {
            fxSource.volume += 0.2f;
            StartCoroutine(FadeInNewMusic(sourceToUp));
        }
        else
        {
            fxSource.volume = 1;
            canPlayMusic = true;
            StopCoroutine(FadeInNewMusic(sourceToUp));
        }
    }

}
