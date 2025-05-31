using UnityEngine;

public class FurnitureEditor : MonoBehaviour
{
    public int furnitureID;

    enum Placeable { Cant, Can, Table }
    Placeable placeable;
    FurnitureManager furnitureManager;
    [SerializeField] FurnitureList furnitureList;
    PlayerStateManager player;
    SaveFile saveFile;
    SpriteRenderer sr;
    BoxCollider2D bc;
    CollisionCheck boxcheck;
    Furniture furniture;
    FurnitureSlot currentSlot;
    int parentLocalIndex;
    int currentIndex;
    bool isActive;
    public bool isDestroying;
    public void Awake()
    {
        player = FindFirstObjectByType<PlayerStateManager>();
        furnitureManager = FindFirstObjectByType<FurnitureManager>();
        saveFile = SaveManager.GetSave();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(int id)
    {
        furnitureID = id;
        furniture = furnitureList.allFurniture[id];
        boxcheck = new CollisionCheck(bc);
        boxcheck.SetType(CollisionCheck.CollisionType.SingleBox)
            .SetBoxSize(furniture.size - (Vector2.one * 0.4f))
            .SetRelativePosition(furniture.GObject.GetComponent<BoxCollider2D>().offset)
            .SetDebug(true);
        sr.sprite = furniture.icon;
        isActive = true;

    }
    public void InitDestroy()
    {
        furnitureID = -1;
        boxcheck = new CollisionCheck(bc);
        boxcheck.SetType(CollisionCheck.CollisionType.SingleBox)
            .SetBoxSize(furniture.size - (Vector2.one * 0.4f))
            .SetRelativePosition(furniture.GObject.GetComponent<BoxCollider2D>().offset)
            .SetDebug(true);
        isDestroying = true;
        isActive = true;

    }
    public void Stop()
    {
        isActive = false;
        sr.sprite = null;
        transform.position = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        if (UIInput.IsBackPressed)
        {
            Stop();
        }
        SnapGrid();
        placeable = Placeable.Cant;
        if (isDestroying)
        {
            CheckIfDestroyable();
        }
        else
        {
            CheckIfPlacable();
        }
    }

    private void SnapGrid()
    {
        transform.position = player.transform.position + (Vector3)(Vector2)player.directionedObject.direction;
        transform.position = global::SnapGrid.SnapToGridCentered(transform.position);
        transform.position += Vector3.back * 3;
    }

    void CheckIfPlacable()
    {
        switch (placeable)
        {
            case Placeable.Cant when UIInput.IsConfirmPressed:
                break;
            case Placeable.Can when UIInput.IsConfirmPressed:
                furnitureManager.PlaceFurniture(furnitureID, currentIndex);
                break;
            case Placeable.Table when UIInput.IsConfirmPressed:
                furnitureManager.PlaceFurnitureOnFurniture(furnitureID, currentIndex, ref currentSlot);
                break;
        }
        if (Time.frameCount % 3 != 0) return; //expensive!

        //set color
        sr.color = placeable switch
        {
            Placeable.Cant => Color.red,
            Placeable.Can => Color.green,
            Placeable.Table => Color.blue,
            _ => Color.red,
        };
        sr.color *= new Color(1, 1, 1, 0.5f);

        placeable = Placeable.Cant;

        Vector2Int localPosition = Vector2Int.RoundToInt(transform.position - furnitureManager.transform.position);
        localPosition.y *= -1;

        if (localPosition.x < 0 //not outside of bounds
            || localPosition.x > 15
            || localPosition.y < 0
            || localPosition.y > 7
            || saveFile.HouseLayout[(localPosition.y * 16) + localPosition.x] > 0 //nothing takes up slot
            || furniture.size != Vector2Int.one && boxcheck.EvaluateAnythingBut<PlayerStateManager>((x) => { }) //must not bump into anything
            )
            return;

        boxcheck.SetFindTriggers(true);
        //can place on slot
        if (furniture.type == Furniture.PlacementType.Small && boxcheck.Evaluate<FurnitureSlot>((x) => { currentSlot = x; }))
        {
            placeable = Placeable.Table;
            return;
        }


        placeable = Placeable.Can;
    }

    void CheckIfDestroyable()
    {
        switch (placeable)
        {
            case Placeable.Cant when UIInput.IsConfirmPressed:
                break;
            case Placeable.Can when UIInput.IsConfirmPressed:
                furnitureManager.DestroyFurniture(currentIndex);
                break;
        }
        if (Time.frameCount % 3 != 0) return; //expensive!
        placeable = Placeable.Cant;
        //set color
        sr.color = placeable switch
        {
            Placeable.Cant => Color.red,
            Placeable.Can => Color.green,
            _ => Color.red,
        };
        Vector2Int localPosition = Vector2Int.RoundToInt(transform.position - furnitureManager.transform.position);
        localPosition.y *= -1;

        if (localPosition.x < 0 //not outside of bounds
            || localPosition.x > 15
            || localPosition.y < 0
            || localPosition.y > 7
            || saveFile.HouseLayout[(localPosition.y * 16) + localPosition.x] <= 0 //nothing takes up slot
            )
            return;
        placeable = Placeable.Can;

    }
}
