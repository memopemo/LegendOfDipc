using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHouseObject : MonoBehaviour
{
    [SerializeField] FurnitureList furnitureList;
    public GameObject gameObjectOnTopOfMe;
    // Start is called before the first frame update
    void Start()
    {
        gameObjectOnTopOfMe = Instantiate(furnitureList.furniture[SaveManager.GetSave().HouseLayout[GameObjectParser.GetIndexFromName(gameObject)]], transform.position, transform.rotation);    
    }

}
