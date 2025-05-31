using UnityEngine;

[RequireComponent(typeof(PlayerTrigger))]
public class SongChangeArea : MonoBehaviour
{
    [SerializeField] Song song;
    public void TryChangeSong()
    {
        FindFirstObjectByType<SongPlayer>()?.MidGameSwitchSong(song);
    }
    public void RevertSong()
    {
        FindFirstObjectByType<SongPlayer>()?.RevertScenesSong();
    }

}
