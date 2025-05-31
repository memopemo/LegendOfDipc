using UnityEngine;

public class Platform : MonoBehaviour
{
    void Start()
    {
        if(gameObject.layer != 12) gameObject.layer = 12;
    }
}
