using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DemonBuffPendantCount : MonoBehaviour
{
    int index = 0;
    Image sprite;
    private void Start()
    {
        sprite = GetComponent<Image>();
        //get index via gameobject name :)
        index = GameObjectParser.GetIndexFromName(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        sprite.enabled = index < SaveManager.GetSave().Pendants.Count((x) => x);
    }
}
