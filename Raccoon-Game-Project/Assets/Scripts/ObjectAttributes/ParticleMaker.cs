using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMaker : MonoBehaviour
{
    public List<GameObject> particles;
    
    public GameObject CreateParticle(int index)
    {
        Debug.Assert(index < particles.Count, "above limit on particles.");
        Debug.Assert(index >= 0, "index is below 0");
        return Instantiate(particles[index], transform.position, transform.rotation);
    }
    public void BasicCreateParticle(int index)
    {
        CreateParticle(index);
    }
    public GameObject CreateCustomParticle(GameObject go)
    {
        return Instantiate(go, transform.position, transform.rotation);
    }
}
