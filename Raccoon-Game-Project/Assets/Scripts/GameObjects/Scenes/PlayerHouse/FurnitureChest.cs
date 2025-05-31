using System.Linq;
using UnityEngine;

public class FurnitureChest : MonoBehaviour
{
    [SerializeField] FurnitureList furnitureList;
    public void Awake()
    {
        if(SaveManager.GetSave().HouseItems.Count((x)=>x == true) == 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void OnInteract()
    {
        PlayerStateManager player = FindFirstObjectByType<PlayerStateManager>();
        player.SwitchState(new NoInputPlayerState());
        GetComponent<UIFurnitureSelector>().Activate();
    }
    public void Cancel()
    {
        Invoke(nameof(ActuallyCancel), 0.5f);

    }
    public void ActuallyCancel()
    {
        PlayerStateManager player = FindFirstObjectByType<PlayerStateManager>();
        player.SwitchState(new DefaultPlayerState());
    }
    public void Yay(int selection)
    {
        FindFirstObjectByType<FurnitureEditor>().Init(selection);
        Invoke(nameof(ActuallyYay), 0.5f);
    }
    public void ActuallyYay()
    {
        PlayerStateManager player = FindFirstObjectByType<PlayerStateManager>();
        player.SwitchState(new DefaultPlayerState());
    }

    void TempSetup()
    {
    }
}
