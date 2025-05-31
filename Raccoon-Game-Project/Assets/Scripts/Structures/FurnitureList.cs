using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FurnitureList", menuName = "Furniture List", order = 3)]
public class FurnitureList : ScriptableObject
{
    public Furniture[] allFurniture;
}
[System.Serializable]
public class Furniture
{
    
    public GameObject GObject;
    public Vector2Int size;
    public PlacementType type;
    public Theme theme;
    public enum PlacementType { None, Container, Small }
    public enum Theme { None, Natural, Urban, Magic }
    public bool isSpeccial;
    public bool isInteractable;
    public Sprite icon;


}
