using UnityEngine;
using UnityEngine.UI;

public class StickInput : MonoBehaviour
{
    Vector2 start;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 a = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (a != Vector2.zero)
        {
            transform.position = start + (Vector2Int.RoundToInt(a.normalized) * 100);
            GetComponent<RawImage>().color = Color.white;
        }
        else
        {
            GetComponent<RawImage>().color = Color.gray;
        }
    }
}
