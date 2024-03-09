using UnityEngine;


public class PoofDestroy : MonoBehaviour
{
    [SerializeField] GameObject particle; //particle to instantiate if we get destroyed.

    bool isQuitingGame = false;
    private void Start()
    {
    }

    private void OnDestroy()
    {
        //prevent instantiation when not playing or switching scenes
        if (particle == null) return;
        if (isQuitingGame) return;
        Poof();
    }
    private void OnDisable()
    {
        Poof();
    }
    private void OnEnable()
    {
        Poof();
    }
    private void Poof()
    {
        Instantiate(particle, transform.position, Quaternion.identity);
    }
    private void OnApplicationQuit()
    {
        isQuitingGame = true;
    }
}
