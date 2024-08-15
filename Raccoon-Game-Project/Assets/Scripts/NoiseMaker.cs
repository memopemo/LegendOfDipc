using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Audio;

public class NoiseMaker : MonoBehaviour
{
    public List<ListWrapper> audioClipPools;
    public GameObject interruptableClip;

    //Plays at specific point.
    //-1 means ignore.
    public void Play(int clipPool, Vector3 position, bool interrupt = false, int specificClip = -1)
    {
        if(GetClip(clipPool, out AudioClip clipWanted, specificClip) == false) return;
        PlaySoundEffectAtPoint(clipWanted, position, audioClipPools[clipPool].isCancelable, interrupt);
    }

    //Plays at transform.
    public void Play(int clipPool, bool interrupt = false, int specificClip = -1)
    {
        if(GetClip(clipPool, out AudioClip clipWanted, specificClip) == false) return;
        PlaySoundEffectAtPoint(clipWanted, transform.position, audioClipPools[clipPool].isCancelable, interrupt);
        
    }

    bool GetClip(int clipPool, out AudioClip clip, int specificClip = -1)
    { 
        clip = null;
        //range checks
        if(clipPool < 0 || clipPool > audioClipPools.Count-1)
        {
            Debug.LogError($"Clip Pool: {clipPool} is not in range of 0 to {audioClipPools.Count-1} ");
            return false;
        }
        if(specificClip != -1 && (specificClip < 0 || specificClip > audioClipPools[clipPool].pool.Count-1))
        {
            Debug.LogError($"specific clip given: {specificClip} is not in range of 0 to {audioClipPools[clipPool].pool.Count-1} ");
            return false;
        }
        
        //is clip not null
        AudioClip clipWanted;
        if (specificClip != -1)
        {
            clipWanted = audioClipPools[clipPool].pool[specificClip];
        }
        else
        {
            clipWanted = audioClipPools[clipPool].pool[Random.Range(0, audioClipPools[clipPool].pool.Count)];
        }

        if (!clipWanted)
        {
            Debug.LogWarning($"Clip Pool: {clipPool} index {specificClip} is not assigned.");
            return false;
        }
        clip = clipWanted;
        return true;
    }
    
    //lul copied from the docs
    public void PlaySoundEffectAtPoint(AudioClip clip, Vector3 position, bool interuptable, bool isInterupting)
    {
        PlaySoundEffectAtPoint(clip, position, 1f, interuptable, isInterupting);
    }
    public void PlaySoundEffectAtPoint(AudioClip clip, Vector3 position, [UnityEngine.Internal.DefaultValue("1.0F")] float volume, bool interuptable, bool isInterupting)
    {
        GameObject gameObject = new(clip.name);
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.outputAudioMixerGroup = (Resources.Load("Base") as AudioMixer).FindMatchingGroups("Master/Effects")[0];
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.Play();
        if(interuptable)
        {
            Destroy(interruptableClip);
            interruptableClip = gameObject;
        }
        if(isInterupting)
        {
            Destroy(interruptableClip);
        }
        Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }
    public void StopInteruptableClip()
    {
        Destroy(interruptableClip);
    }
}
