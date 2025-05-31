using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class HouseFurnitureCreator : MonoBehaviour
{
    /* assumptions:
        1. origin is in the top left corner
        2. pushes can succeed if we are pushing a furniture object onto a container
        3. not all spaces that are taken up by a furniture object are slots, but can be if the container has a slot there.
        4. furniture is stored as a single number but can take up multiple spaces
        5. if a furniture

    */
    // Terminology:
    // index = position related to the index into a 1D array, made 2D by
    // local index = 0 starts from the initial index related to the, and goes top down, right to left. bound by the size of the parent furniture object.
    // position = x,y coordinate relative to the creator (this game object), going top down, right to left.
    // local position = x,y coordinate relative to the parent furniture object, going top down, right to left.

    [SerializeField] FurnitureList furnitureList;
    SaveFile saveFile;
    public const int LAYOUT_LENGTH = 16;
    public const int LAYOUT_HEIGHT = 8;


    void Start()
    {
        saveFile = SaveManager.GetSave();
        bool[] generated = new bool[LAYOUT_HEIGHT * LAYOUT_LENGTH];
        for (int index = 0; index < saveFile.HouseLayout.Length; index++)
        {
            if (generated[index]) continue;
            GenerateFurnitureFromSave(index, out List<int> indexesGenerated);
            indexesGenerated.ForEach((x) => generated[x] = true); //label this and any other as generated.
        }
    }

    void GenerateFurnitureFromSave(int index, out List<int> indexesGenerated)
    {
        indexesGenerated = new();
        Debug.Assert(index >= 0 && index < LAYOUT_HEIGHT * LAYOUT_LENGTH, $"invalid index: {index}");


        Vector2Int targetPosition = new(index % 16, index / -16); //negative because top down
        int furnitureID = saveFile.HouseLayout[index];
        if (saveFile.HouseLayout[index] == 0) return;
        Furniture tryingToGenerateFurniture = furnitureList.allFurniture[furnitureID];

        if (!CanPlaceHere(tryingToGenerateFurniture, index, tryingToGenerateFurniture.size))
        {
            CrushHisSkullThankYou(index);
            return;
        }

        //generate and setup furniture.
        FurnitureObject instantiatedFurniture = Instantiate(tryingToGenerateFurniture.GObject, transform).GetComponent<FurnitureObject>();
        instantiatedFurniture.furniture = tryingToGenerateFurniture;
        indexesGenerated.Add(index);
        instantiatedFurniture.transform.localPosition = (Vector2)targetPosition;

        //try to generate children.
        if (tryingToGenerateFurniture.type == Furniture.PlacementType.Container)
        {
            GenerateChildren(instantiatedFurniture, index, out List<int> childIndexesGenerated);
            indexesGenerated.AddRange(childIndexesGenerated);
        }
    }
    bool CanPlaceHere(Furniture furniture, int index, Vector2Int size)
    {
        //Check for other objects in the way (besides child slots)
        for (int i = 0; i < size.x * size.y; i++)
        {
            if (IsChild(i)) continue;
            int localIndex = index + (LAYOUT_LENGTH * (i / size.x)) + (i % size.y);
            if (saveFile.HouseLayout[localIndex] <= 0) return false;
        }
        bool IsChild(int i)
        {
            if (furniture.GObject == null) return false;
            foreach (var item in furniture.GObject.GetComponentsInChildren<FurnitureSlot>())
            {
                if (item.localSlotIndex == i) return true;
            }
            return false;
        }

        //Check if any part of the furniture would be out of bounds if placed (right and bottom border)
        if (LAYOUT_LENGTH - size.x - (index % LAYOUT_LENGTH) < 0) return false; //is too far right?
        if (LAYOUT_HEIGHT - size.y - (index / LAYOUT_LENGTH) < 0) return false; //is too far down?

        return true;
    }

    void GenerateChildren(FurnitureObject furnitureObject, int originIndex, out List<int> indexesGenerated)
    {
        indexesGenerated = new();
        foreach (FurnitureSlot v in furnitureObject.GetComponentsInChildren<FurnitureSlot>())
        {
            int sizeX = furnitureObject.furniture.size.x;
            int sizeY = furnitureObject.furniture.size.y;

            //is slot index sane
            if (v.localSlotIndex >= sizeX * sizeY)
            {
                Debug.LogWarning($"Child slot's local index is outside of range for the size of the parent. \nFurniture size = X{sizeX} * Y{sizeY} = {sizeX * sizeY}\nRecieved: {v.localSlotIndex} \n Go fix the prefab you fuckhead.");
                continue;
            }

            // get child furniture
            int childIndex = originIndex + (LAYOUT_LENGTH * (v.localSlotIndex / sizeX)) + (v.localSlotIndex % sizeX);
            int childFurnitureID = saveFile.HouseLayout[childIndex];
            if (childFurnitureID == 0) continue;
            Furniture childFurniture = furnitureList.allFurniture[childFurnitureID];

            //is furniture sane
            if (childFurniture.type != Furniture.PlacementType.Small || childFurniture.size != Vector2Int.one)
            {
                CrushHisSkullThankYou(childIndex);
                continue;
            }

            //generate furniture as child.
            FurnitureObject childGO = Instantiate(childFurniture.GObject, v.transform).GetComponent<FurnitureObject>();
            childGO.furniture = childFurniture;
            childGO.transform.localPosition = Vector2.zero;
            //set child as trigger so that we cant touch it
            childGO.GetComponent<Collider2D>().isTrigger = true;

            //add to indexes generated
            indexesGenerated.Add(childIndex);
        }

    }
    void CrushHisSkullThankYou(int index)
    {
        Debug.Log($"Could Not Place Furniture Here. Correcting Save file and aborting. Index:{index}");
        saveFile.HouseLayout[index] = 0;
    }


}
