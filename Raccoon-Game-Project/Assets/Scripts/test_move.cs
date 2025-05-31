using UnityEngine;

public class test_move : MonoBehaviour
{
    DirectionedObject directioned;
    float f;
    [SerializeField] float length;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        directioned = GetComponent<DirectionedObject>();
    }
    void Update()
    {
        transform.position += (Vector3)(Vector2)directioned.direction*Time.deltaTime;
        f+=Time.deltaTime;
        if(f >= length)
        {
            Flip();
        }
    }
    void Flip()
    {
        directioned.direction *= -1;
        f = 0;
    }

}
