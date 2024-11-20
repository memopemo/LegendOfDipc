using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SavePoint : TrashBin
{
    public int SavePointID;
    [SerializeField] GameObject particle;
    public override void OnAction()
    {
        SaveManager.GetSave().SavePoint = SavePointID;
        SaveManager.Save();
        Instantiate(particle, transform.position - Vector3.one * 0.5f + Vector3.back * 2, quaternion.identity);
        GetComponent<Flasher>().StartFlash();
        Invoke(nameof(SpitOutPlayer), 0.75f);
    }
}
