using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGSelector : MonoBehaviour
{
    int actionSelection;
    int itemSelection;
    Vector2 initPosition;
    const float SPEED = 20;
    const float SELECTION_WIDTH = 40/16f;
    Vector2 ItemSelectionOffset = new Vector2(3.5f, -1.5f);
    const int SELECTABLE_ITEMS = 5;
    [SerializeField] Sprite selectedActionBorder;
    [SerializeField] Sprite deselectedActionBorder;
    [SerializeField] SpriteRenderer[] actionBorders;
    [SerializeField] 
    BattleScene battleScene;
    RPGItemSelectorWindow itemWindow;
    bool isSelectingItem;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.localPosition;
        battleScene = FindFirstObjectByType<BattleScene>();
        itemWindow = FindFirstObjectByType<RPGItemSelectorWindow>();
    }
    void OnDisable()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
    void OnEnable()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSelectingItem)
        {
            HandleActionSelectionInput();
        }
        else
        {
            HandleItemSelectionInput();
        }

        //cursor position
        
        transform.localPosition += Vector3.back*1.5f;
        
    }
    void HandleActionSelectionInput()
    {
        //handle input
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            actionBorders[actionSelection].sprite = deselectedActionBorder;
            if(actionSelection == SELECTABLE_ITEMS - 1) 
                actionSelection = 0;
            else 
                actionSelection++;
            actionBorders[actionSelection].sprite = selectedActionBorder;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            actionBorders[actionSelection].sprite = deselectedActionBorder;
            if(actionSelection == 0) 
                actionSelection = SELECTABLE_ITEMS-1;
            else 
                actionSelection--;
            actionBorders[actionSelection].sprite = selectedActionBorder;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow)
                 && (actionSelection == (int)BattleScene.PlayerAction.ConsumableItem || actionSelection == (int)BattleScene.PlayerAction.KeyItem))
        {
            isSelectingItem = true;
            itemSelection = 0; //using different selection
            GameObject.Find($"Slot_{itemSelection}").GetComponent<SpriteRenderer>().sprite = selectedActionBorder;
            return;
        }
        if(actionSelection == (int)BattleScene.PlayerAction.ConsumableItem)
        {
            itemWindow.isActive = true;
            itemWindow.isConsumableMode = true;
        }
        else if(actionSelection == (int)BattleScene.PlayerAction.KeyItem)
        {
            itemWindow.isActive = true;
            itemWindow.isConsumableMode = false;
        }
        else
        {
            itemWindow.isActive = false;
        }
        if(Input.GetButtonDown("Fire1"))
        {
            itemWindow.isActive = false;
            battleScene.OnPlayerSelectedAction((BattleScene.PlayerAction)actionSelection);
        }

        transform.localPosition = Vector2.Lerp(transform.localPosition, initPosition + (SELECTION_WIDTH * actionSelection * Vector2.down), Time.deltaTime*SPEED);
    }
    void HandleItemSelectionInput()
    {
        int width = itemWindow.isConsumableMode ? 4 : 3;
        int ogSelection = itemSelection;
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            itemSelection += width;
            if(itemSelection >= 6*width)
            {
                itemSelection %= width;
            }
            
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            itemSelection -= width;
            if(itemSelection < 0)
            {
                itemSelection = (itemSelection % width) + 5 * width;
            }
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            itemSelection++;
            if(itemSelection >= 6 * width)
            {
                itemSelection = 0;
            }
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(itemSelection % width == 0)
            {
                exit();
                return;
            }
            itemSelection--;
            if(itemSelection == -1)
            {
                itemSelection = (6*width)-1;
            }
        }
        if(Input.GetButtonDown("Fire2"))
        {
            exit();
            return;
        }
        if(Input.GetButtonDown("Fire1"))
        {
            battleScene.itemIndex = itemSelection;
            itemWindow.isActive = false;
            battleScene.OnPlayerSelectedAction((BattleScene.PlayerAction)actionSelection); 
            exit();
            return;
        }
        itemSelection %= width*6; //just in case.
        print(itemSelection);
        //if there is a difference, change sprites. This may be stupid.
        if(ogSelection != itemSelection)
        {
            int previousSlot = itemWindow.isConsumableMode? ogSelection : KeyIndexToConsumableIndex(ogSelection);
            int currentSlot = itemWindow.isConsumableMode? itemSelection : KeyIndexToConsumableIndex(itemSelection);
            GameObject.Find($"Slot_{previousSlot}").GetComponent<SpriteRenderer>().sprite = deselectedActionBorder;
            GameObject.Find($"Slot_{currentSlot}").GetComponent<SpriteRenderer>().sprite = selectedActionBorder;
        }
        
        Vector2 selectionPosition = 2 * new Vector2(itemSelection % width , itemSelection / width * -1);
        transform.localPosition = Vector2.Lerp(transform.localPosition, initPosition + ItemSelectionOffset + selectionPosition, Time.deltaTime*10);

        void exit()
        {
            isSelectingItem = false;
            GameObject.Find($"Slot_{itemSelection}").GetComponent<SpriteRenderer>().sprite = deselectedActionBorder;         
        }
    }
    int ConsumableIndexToKeyIndex(int i)
    {
        return i - (i/4);
    }
    int KeyIndexToConsumableIndex(int i)
    {
        return i + (i/3);
    }
}