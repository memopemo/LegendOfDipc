using UnityEngine;


public class PoofDestroy : MonoBehaviour
{
    [SerializeField] GameObject particle; //particle to instantiate if we get destroyed.

    private void Start()
    {
    }
    
    private void OnDisable()
    {
        if(!gameObject.activeSelf)
        {
            Poof();
        }
    }
    private void OnEnable()
    {
        if(!gameObject.activeSelf)
        {
            Poof();
        }
    }
    private void Poof()
    {
        Instantiate(particle, transform.position, Quaternion.identity);
    }
}
