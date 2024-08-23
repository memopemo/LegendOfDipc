using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGSelector : MonoBehaviour
{
    int selection;
    Vector2 initPosition;
    const float SPEED = 20;
    const float SELECTION_WIDTH = 40/16f;
    const int SELECTABLE_ITEMS = 5;
    [SerializeField] Sprite selectedActionBorder;
    [SerializeField] Sprite deselectedActionBorder;
    [SerializeField] SpriteRenderer[] actionBorders;
    BattleScene battleScene;
    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.localPosition;
        battleScene = FindFirstObjectByType<BattleScene>();
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
        //handle input
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            actionBorders[selection].sprite = deselectedActionBorder;
            if(selection == SELECTABLE_ITEMS - 1) 
                selection = 0;
            else 
                selection++;
            actionBorders[selection].sprite = selectedActionBorder;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            actionBorders[selection].sprite = deselectedActionBorder;
            if(selection == 0) 
                selection = SELECTABLE_ITEMS-1;
            else 
                selection--;
            actionBorders[selection].sprite = selectedActionBorder;
        }
        if(Input.GetButtonDown("Fire1"))
        {
            FindFirstObjectByType<BattleScene>().OnPlayerSelectedAction((BattleScene.PlayerAction)selection);
        }

        //cursor position
        transform.localPosition = Vector2.Lerp(transform.localPosition, initPosition + (SELECTION_WIDTH * selection * Vector2.down), Time.deltaTime*SPEED);
        transform.localPosition += Vector3.back;
        
    }
}
