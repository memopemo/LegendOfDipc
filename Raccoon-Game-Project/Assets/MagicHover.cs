using UnityEngine;
[RequireComponent(typeof(Heightable))]
public class MagicHover : MonoBehaviour
{
    Heightable heightable;
    // Start is called before the first frame update
    void Start()
    {
        heightable = GetComponent<Heightable>();
    }

    // Update is called once per frame
    void Update()
    {
        heightable.height = (Mathf.Sin(Time.time * 4) / 4) + 0.5f;
    }
}
