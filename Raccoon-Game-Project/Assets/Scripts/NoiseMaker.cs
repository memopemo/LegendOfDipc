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
    public void Play(int clipPool, Vector3 position, bool interrupt = false)
    {
        if(GetClip(clipPool, out AudioClip clipWanted) == false) return;
        PlaySoundEffectAtPoint(clipWanted, position, interrupt, audioClipPools[clipPool]);
    }

    //Plays at transform.
    public void Play(int clipPool, bool interrupt = false)
    {
        if(GetClip(clipPool, out AudioClip clipWanted) == false) return;
        PlaySoundEffectAtPoint(clipWanted, transform.position, interrupt, audioClipPools[clipPool]);
        
    }

    bool GetClip(int clipPool, out AudioClip clip)
    { 
        clip = null;
        //range checks
        if(clipPool < 0 || clipPool > audioClipPools.Count-1)
        {
            Debug.LogError($"Clip Pool: {clipPool} is not in range of 0 to {audioClipPools.Count-1} ");
            return false;
        }
        //is clip not null
        AudioClip clipWanted;
        clipWanted = audioClipPools[clipPool].pool[Random.Range(0, audioClipPools[clipPool].pool.Count)];
        clip = clipWanted;
        if(clip == null)
        {
            Debug.LogError("Clip is null");
            return false;
        }
        return true;
    }
    
    //lul copied from the docs
    public void PlaySoundEffectAtPoint(AudioClip clip, Vector3 position, bool isInterupting, ListWrapper clipDetails)
    {
        GameObject gameObject = new(clip.name);
        gameObject.transform.position = new Vector3(position.x, position.y, -10); //play specifically at camera's level.
        if(clipDetails.followNoiseMaker)
        {
            gameObject.transform.parent = transform;
        }
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.outputAudioMixerGroup = (Resources.Load("Base") as AudioMixer).FindMatchingGroups("Master/Effects")[0];
        audioSource.clip = clip;
        audioSource.pitch = clipDetails.varyPitch ? Random.Range(0.8f, 1.2f): 1;
        audioSource.loop = clipDetails.looping;
        audioSource.spatialBlend = clipDetails.spatialBlend;
        audioSource.volume = 1f;
        audioSource.Play();
        if(clipDetails.isCancelable)
        {
            Destroy(interruptableClip);
            interruptableClip = gameObject;
        }
        if(isInterupting)
        {
            Destroy(interruptableClip);
        }
        if(!clipDetails.looping)
        {
            Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
        }
    }
    public void StopInteruptableClip(bool cut = true)
    {
        if(cut)
        {
            Destroy(interruptableClip);
        }
        else
        {
            StartCoroutine("FadeOutInteruptableClip");
        }
    }
    IEnumerator FadeOutInteruptableClip()
    {
        AudioSource interruptableAudioSource = interruptableClip.GetComponent<AudioSource>();
        while (interruptableAudioSource.volume > 0)
        {
            interruptableAudioSource.volume -= Time.deltaTime*5;
            yield return null;
        }
        Destroy(interruptableClip); //destroy when done.
        
    }
}
