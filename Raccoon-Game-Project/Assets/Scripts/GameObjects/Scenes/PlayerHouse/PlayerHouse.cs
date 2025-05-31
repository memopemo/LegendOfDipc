using System.Linq;
using UnityEngine;

public class PlayerHouse : MonoBehaviour
{
    public enum Homeiness { Freshman, Sophomore, Junior, Senior, Graduate }
    public static Homeiness CalculateHouseProgression()
    {
        //determined by visual items in the house
        SaveFile saveFile = SaveManager.GetSave();

        int totalEquipment = saveFile.Swords.Count((x) => x);
        totalEquipment += saveFile.Shields.Count((x) => x);
        totalEquipment += saveFile.Armors.Count((x) => x);
        totalEquipment += saveFile.Boomerangs.Count((x) => x);

        int maxEquipment = saveFile.Swords.GetLength(0);
        maxEquipment += saveFile.Shields.GetLength(0);
        maxEquipment += saveFile.Armors.GetLength(0);
        maxEquipment += saveFile.Boomerangs.GetLength(0);

        int furnitureAreaFullness = saveFile.HouseLayout.Count((x) => x > 0);

        int maxFullness = saveFile.HouseLayout.GetLength(0);

        float percentageEquipment = (float)totalEquipment / maxEquipment;
        float percentageFurniture = (float)furnitureAreaFullness / maxFullness;

        percentageFurniture *= 2; //50% furniture area filled = 100%

        float mainPercentage = Mathf.Clamp01((percentageEquipment + percentageFurniture) / 2);

        //if and only if we are at 100%, then x = 4.
        return (Homeiness)Mathf.FloorToInt(mainPercentage * 4);
    }
}
