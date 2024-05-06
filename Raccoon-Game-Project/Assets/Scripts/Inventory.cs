using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Sprite[] itemPreviews;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        }
        
        //Debug Keys
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveManager.Save();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveManager.DebugMaxOut();

        }
    }
}
