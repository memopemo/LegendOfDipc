using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseFurnitureCreator : MonoBehaviour
{
    [SerializeField] FurnitureList furnitureList;
    void Start()
    {
        int[] houseLayout = SaveManager.GetSave().HouseLayout;
        for (int position = 0; position < houseLayout.Length; position++)
        {
            Vector2 placement = new Vector2Int(position % 16, position / 16);
            Instantiate(furnitureList.furniture[houseLayout[position]], (Vector3)placement, Quaternion.identity);
        }
    }
}
