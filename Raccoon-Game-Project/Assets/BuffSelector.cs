using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuffSelector : MonoBehaviour
{
    [SerializeField] Sprite inactive;
    Sprite active;
    Image image;
    int index;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        active = image.sprite;
        index = GameObjectParser.GetIndexFromName(gameObject);
        InvokeRepeating(nameof(Tick),0.1f, 0.1f);
    }

    // Update is called once per frame
    void Tick()
    {
        if (!SaveManager.GetSave().Pendants[index % 7] || (index / 7) >= SaveManager.GetSave().Pendants.Count(a => a))
        {
            image.sprite = inactive;
        }
        else
        {
            image.sprite = active;
        }
    }
}
