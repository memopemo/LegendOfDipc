using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SongPlayer : MonoBehaviour
{
    [SerializeField] SceneSongs sceneSongs;
    Song currentSong;
    AudioSource source;
    float targetVolume;

    // velocity is 1/volumeChangeInSecs
    float volumeChangeInSecs; //how many seconds should it take to go from max to mute and vice versa?

    void Start()
    {
        AudioMixer mixer = GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer;
        mixer.SetFloat(UIAudioVolume.exposedNames[0], PlayerPrefs.GetFloat(UIAudioVolume.exposedNames[0]));
        mixer.SetFloat(UIAudioVolume.exposedNames[1], PlayerPrefs.GetFloat(UIAudioVolume.exposedNames[1]));
        mixer.SetFloat(UIAudioVolume.exposedNames[2], PlayerPrefs.GetFloat(UIAudioVolume.exposedNames[2]));

        source = GetComponent<AudioSource>();
#if DEBUG
        if (FindObjectsByType<SongPlayer>(FindObjectsSortMode.None).Length > 1)
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
        targetVolume = 1;
        source.Play();
        DontDestroyOnLoad(gameObject);

    }
    void Update()
    {

        //print(source.timeSamples);
        if (!currentSong) return; //this is shit but i have no idea why its causing a fit.
        if (source.timeSamples >= currentSong.LoopEndSamples)
        {
            source.timeSamples = currentSong.LoopStartSamples + (source.timeSamples - currentSong.LoopEndSamples);
            source.Play();
        }
        if (source.volume < targetVolume)
        {
            source.volume += 1 / volumeChangeInSecs * Time.deltaTime;
        }
        else if (source.volume > targetVolume)
        {
            source.volume -= 1 / volumeChangeInSecs * Time.deltaTime;
        }
    }
    public void OnSceneChange(SceneReference sceneReference)
    {
        Song nextSong = sceneSongs.GetSong(sceneReference);
        if (nextSong == currentSong)
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
        const float speed = 1;
        StartFadeOut(speed);
        yield return new WaitForSeconds(speed);
        source.clip = currentSong.song;
        source.Play();
        StartFadeIn(speed);
    }
    public void StartFadeOut(float inSecs = 1)
    {
        targetVolume = 0;
        volumeChangeInSecs = inSecs;
    }
    public void StartFadeIn(float inSecs = 1)
    {
        targetVolume = 1;
        volumeChangeInSecs = inSecs;
    }
}
