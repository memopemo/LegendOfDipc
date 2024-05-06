using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDemonKeys : MonoBehaviour
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
        sprite.enabled = SaveManager.GetSave().DemonKeys[index];
    }
}
