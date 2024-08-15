using UnityEngine;


public class PoofDestroy : MonoBehaviour
{
    [SerializeField] GameObject particle; //particle to instantiate if we get destroyed.

    public void Poof()
    {
        Instantiate(particle, transform.position, Quaternion.identity);
    }
    
    public void PoofAndDestroy()
    {
        Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
