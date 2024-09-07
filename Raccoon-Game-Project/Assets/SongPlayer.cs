using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SongPlayer : MonoBehaviour
{
    [SerializeField] SceneSongs sceneSongs;
    Song currentSong;
    AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
#if DEBUG
        if(FindObjectsByType<SongPlayer>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        SceneReference reference = new SceneReference
        {
            ScenePath = SceneManager.GetActiveScene().path
        };
        currentSong = sceneSongs.GetSong(reference);
#else
            if(SceneManager.GetActiveScene().buildIndex != 0)
            {
                Destroy(gameObject);
                return;
            }
            currentSong = sceneSongs.sceneSongs.First().Value;
#endif
        
        source.clip = currentSong.song;
        
        source.Play();
        DontDestroyOnLoad(gameObject);
        
    }
    void Update()
    {
        //print(source.timeSamples);
        if(source.timeSamples >= currentSong.LoopEndSamples)
        {
            source.timeSamples = currentSong.LoopStartSamples + (source.timeSamples - currentSong.LoopEndSamples);
            source.Play();
        }
    }
    public void OnSceneChange(SceneReference sceneReference)
    {
        Song nextSong = sceneSongs.GetSong(sceneReference);
        if(nextSong == currentSong)
        {
            return;
        }
        else
        {
            currentSong = nextSong;
            StartCoroutine(nameof(FadeAndSwitchSong));
        }
    }
    IEnumerator FadeAndSwitchSong()
    {
        yield return FadeOut();
        source.clip = currentSong.song;
        source.Play();
        yield return FadeIn();
    }
    public void StartFadeOut(float inSecs = 1)
    {
        StartCoroutine(FadeOut(inSecs));
    }
    public void StartFadeIn(float inSecs = 1)
    {
        StartCoroutine(FadeIn(inSecs));
    }
    IEnumerator FadeOut(float speed = 1)
    {
        while(source.volume > 0)
        {
            source.volume -= Time.deltaTime * (1/speed);
            yield return null; //wait for next frame.
        }
        source.volume = 0;
    }
    IEnumerator FadeIn(float speed = 1)
    {
        while(source.volume < 1)
        {
            source.volume += Time.deltaTime * (1/speed);
            yield return null; //wait for next frame.
        }
        source.volume = 1;
    }
}
