using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Linq;

[CreateAssetMenu()]
public class SceneSongs : ScriptableObject
{
    public SerializedDictionary<SceneReference, Song> sceneSongs;
    public Song errorSong;
    public Song GetSong(SceneReference sceneReference)
    {
        try
        {
            return sceneSongs.FirstOrDefault((x)=>x.Key.ScenePath == sceneReference.ScenePath).Value;
        }
        catch
        {
            return errorSong;
        }
        
    }
}
