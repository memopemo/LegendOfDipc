using AYellowpaper.SerializedCollections;
using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu()]
public class SceneSongs : ScriptableObject
{
    public SerializedDictionary<SceneReference, Song> sceneSongs;
    public Song errorSong;
    public Song GetSong(SceneReference sceneReference)
    {
        try
        {
            return sceneSongs.First((x) => x.Key.ScenePath == sceneReference.ScenePath).Value;
        }
        catch (InvalidOperationException)
        {
            return errorSong;
        }



    }
}
