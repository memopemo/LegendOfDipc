using UnityEngine;
using UnityEngine.Video;

public class ello : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VideoPlayer>().loopPointReached += Ello_loopPointReached;
    }

    private void Ello_loopPointReached(VideoPlayer source)
    {
        print("Quit");
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
