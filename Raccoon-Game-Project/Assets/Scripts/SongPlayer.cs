using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SongPlayer : MonoBehaviour
{
    [SerializeField] SceneSongs sceneSongs;
    Song currentSong;
    Song nextSong;
    AudioSource source;
    float targetVolume;
    int previousFrameTimeSamples;

    // velocity is 1/volumeChangeInSecs
    float volumeChangeInSecs; //how many seconds should it take to go from max to mute and vice versa?
    AudioMixer mixer;

    void Start()
    {
        mixer = GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer;
        mixer.SetFloat(UIAudioVolume.exposedNames[0], UIAudioVolume.volumes[PlayerPrefs.GetInt(UIAudioVolume.exposedNames[0], 10)]);
        mixer.SetFloat(UIAudioVolume.exposedNames[1], UIAudioVolume.volumes[PlayerPrefs.GetInt(UIAudioVolume.exposedNames[1], 7)]);
        mixer.SetFloat(UIAudioVolume.exposedNames[2], UIAudioVolume.volumes[PlayerPrefs.GetInt(UIAudioVolume.exposedNames[2], 7)]);
        PlayerPrefs.SetInt(UIAudioVolume.exposedNames[0], PlayerPrefs.GetInt(UIAudioVolume.exposedNames[0], 10));
        PlayerPrefs.SetInt(UIAudioVolume.exposedNames[1], PlayerPrefs.GetInt(UIAudioVolume.exposedNames[1], 7));
        PlayerPrefs.SetInt(UIAudioVolume.exposedNames[2], PlayerPrefs.GetInt(UIAudioVolume.exposedNames[2], 7));

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
            print("a");
            source.volume += 1 / volumeChangeInSecs * Time.deltaTime;
        }
        else if (source.volume > targetVolume)
        {
            print("b");
            source.volume -= 1 / volumeChangeInSecs * Time.deltaTime;
        }

        //on lost focus, dim volume
        mixer.SetFloat(UIAudioVolume.exposedNames[0], UIAudioVolume.volumes[PlayerPrefs.GetInt(UIAudioVolume.exposedNames[0])]-(Application.isFocused?0f:10f));
    }
    public void OnSceneChange(SceneReference sceneReference)
    {
        MidGameSwitchSong(sceneSongs.GetSong(sceneReference));
    }
    public void RevertScenesSong()
    {
        MidGameSwitchSong(sceneSongs.GetSong(new SceneReference
        {
            ScenePath = SceneManager.GetActiveScene().path
        }));
    }
    public void MidGameSwitchSong(Song song)
    {
        if(song == currentSong) return;
        // we use a new variable so that the looping still applies for the current song, not the next song.
        // this is so that (as a random hypothetical example that never happened) if someone were to try a 0 second loop for silence, when fading out it wouldn't stutter.
        nextSong = song; 
        StartCoroutine(nameof(FadeAndSwitchSong));
    }
    IEnumerator FadeAndSwitchSong()
    {
        const float speed = 1;
        StartFadeOut(speed);
        yield return new WaitForSeconds(speed);
        source.clip = nextSong.song;
        source.Play();
        currentSong = nextSong;
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
    public void Restart()
    {
        StartFadeIn();
        source.Play();
    }
}
