using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Inventory : MonoBehaviour
{

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
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveManager.Save();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveManager.DebugMaxOut();

        }
    }
    public static int GetIndexFromName(GameObject go) 
    {
        return int.Parse(Regex.Match(go.name, @"\d+$", RegexOptions.RightToLeft).Value); //dont ask, copied from internet
    }

}
