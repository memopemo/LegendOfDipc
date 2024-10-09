using UnityEngine;

public class CircleFadeInUI : MonoBehaviour
{
    RectTransform trans;
    [SerializeField] bool isFadeIn;
    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<RectTransform>();
        trans.sizeDelta = Vector2.zero;
        isFadeIn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeIn)
        {
            if (trans.sizeDelta.y < 600)
            {
                trans.sizeDelta += 700 * Time.deltaTime * Vector2.one;
            }
        }
        else
        {
            if (trans.sizeDelta.y > 0)
            {
                trans.sizeDelta -= 700 * Time.deltaTime * Vector2.one;
            }
        }

    }
    public void Out()
    {
        isFadeIn = false;
    }

}
