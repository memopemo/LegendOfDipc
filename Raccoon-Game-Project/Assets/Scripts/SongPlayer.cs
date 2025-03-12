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
    int previousFrameTimeSamples;

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
        //skip by 4 seconds.
        if (Input.GetKeyDown(KeyCode.V))
        {
            source.timeSamples += 44100 * 4;
        }
        //print(source.timeSamples);
        if (!currentSong) return; //this is shit but i have no idea why its causing a fit.

        //ok so in order to have the song loop properly, your loop point should still be a ways into the song, and have some of the other side of the loop point sticking out at the end.
        // else it may try to loop and fail.
        if (source.timeSamples >= currentSong.LoopEndSamples)
        {
            int samplesMissed = source.timeSamples - currentSong.LoopEndSamples; //seamless looping
            source.timeSamples = currentSong.LoopStartSamples + samplesMissed;
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
