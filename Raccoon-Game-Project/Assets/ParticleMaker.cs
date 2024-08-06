using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMaker : MonoBehaviour
{
    public List<GameObject> particles;
    
    public void CreateParticle(int index)
    {
        Debug.Assert(index < particles.Count, "above limit on particles.");
        Debug.Assert(index >= 0, "index is below 0");
        Instantiate(particles[index], transform.position, transform.rotation);
    }
    public void CreateCustomParticle(GameObject go)
    {
        Instantiate(go, transform.position, transform.rotation);
    }
}
