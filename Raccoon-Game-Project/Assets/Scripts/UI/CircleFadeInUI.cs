using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: thing is fading in way too fast when loading scenes and IDK WHY T-T
public class CircleFadeInUI : MonoBehaviour
{
    RectTransform trans;
    [SerializeField] bool isFadeIn;
    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<RectTransform>();
        trans.sizeDelta = Vector2.zero;
    }


    // Update is called once per frame
    void Update()
    {
        if (isFadeIn)
        {
            if (trans.sizeDelta.y < 600)
            {
                trans.sizeDelta += 10 * Vector2.one;
            }
        }
        else
        {
            if (trans.sizeDelta.y > 0)
            {
                trans.sizeDelta -= 10 * Vector2.one;
            }
        }

    }
    public void Out()
    {
        isFadeIn = false;
    }

}
