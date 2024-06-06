using UnityEngine;


public class PoofDestroy : MonoBehaviour
{
    [SerializeField] GameObject particle; //particle to instantiate if we get destroyed.

    private void Start()
    {
    }
    
    private void OnDestroy()
    {
        Poof();
    }
    private void Poof()
    {
        Instantiate(particle, transform.position, Quaternion.identity);
    }
}
