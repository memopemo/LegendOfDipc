using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemonDoor : MonoBehaviour
{
    public void OnInteract()
    {
        if (SaveManager.GetSave().DemonKeys.All(x => x == true))
        {
            GetComponent<PoofDestroy>().PoofAndDestroy();
        }
    }
}
