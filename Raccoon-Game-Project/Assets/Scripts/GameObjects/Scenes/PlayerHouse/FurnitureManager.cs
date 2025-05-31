using UnityEditor;
using UnityEngine;


public class FurnitureManager : MonoBehaviour
{
    SaveFile saveFile;
    [SerializeField] FurnitureList furnitureList;

    void Start()
    {
        saveFile = SaveManager.GetSave();
    }
    public void MoveFurniture(int index, int newIndex, Furniture furniture)
    {
        int[] storage = new int[furniture.size.x * furniture.size.y];

        //store and 0 out old info
        for (int i = 0; i < storage.Length; i++)
        {
            int localPrevIndex = index + (HouseFurnitureCreator.LAYOUT_LENGTH * (i / furniture.size.x)) + (i % furniture.size.y);
            storage[i] = saveFile.HouseLayout[localPrevIndex];
            saveFile.HouseLayout[localPrevIndex] = 0;

        }
        //place new info in new location
        for (int i = 0; i < storage.Length; i++)
        {
            int localNextIndex = newIndex + (HouseFurnitureCreator.LAYOUT_LENGTH * (i / furniture.size.x)) + (i % furniture.size.y);
            saveFile.HouseLayout[localNextIndex] = storage[i];
        }
    }
    // this is assumed to be able to be placed ( inside of bounds, not touching anything)
    public void PlaceFurniture(int id, int index)
    {
        saveFile.HouseLayout[index] = id;
        GameObject go = Instantiate(furnitureList.allFurniture[id].GObject, transform);
        go.transform.localPosition = new Vector3(index % 16, index / -16, 0);
        //create particles
    }
    public void PlaceFurnitureOnFurniture(int id, int index, ref FurnitureSlot slot)
    {
        saveFile.HouseLayout[index] = id;
        GameObject go = Instantiate(furnitureList.allFurniture[id].GObject);
        go.transform.parent = slot.transform;
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<BoxCollider2D>().isTrigger = true;
        // find if index is equal to a slot index
        // create furniture item and parent to slot
        // position furniture item.
        // write id to save
    }
    public void DestroyFurniture(int index)
    {
        saveFile.HouseLayout[index] = 0;
        //create particles
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        SaveFile sf = SaveManager.GetSave();
        for (int i = 0; i < 128; i++)
        {
            Vector2 pos = (Vector2)transform.position + new Vector2(i % 16, i / -16);
            //visible
            Gizmos.color = new(0, 0.5f, 1f, 0.5f);
            if (sf.HouseLayout[i] > 0)
            {
                Gizmos.DrawCube(pos, Vector3.one);
                Handles.Label(pos, sf.HouseLayout[i].ToString());
            }

            //index label
            Handles.Label(pos + Vector2.one * 0.25f, i.ToString());

        }
    }
#endif

}
