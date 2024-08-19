using UnityEngine;

public class FireworkStar : MonoBehaviour
{
    [SerializeField] GameObject trail;
    [SerializeField] GameObject startParticle;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(MakeTrail), 0, 0.05f);
        Instantiate(startParticle, transform.position, transform.rotation);
    }
    void MakeTrail()
    {
        Instantiate(trail, transform.position, Quaternion.identity);
    }
}
