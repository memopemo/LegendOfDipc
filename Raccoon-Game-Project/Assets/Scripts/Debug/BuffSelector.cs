using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuffSelector : MonoBehaviour
{
    [SerializeField] Sprite inactive; //pendant not acquired
    [SerializeField] DemonBuffSpriteList selectable;
    [SerializeField] DemonBuffSpriteList disabled; //enforces limitations of 
    Image image;
    int index;
    Sprite selectableSprite;
    Sprite disabledSprite;

    public enum State { Inactive, Selectable, Disabled }
    public State state;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        index = GameObjectParser.GetIndexFromName(gameObject);
        InvokeRepeating(nameof(Tick), 0.1f, 0.1f);
        selectableSprite = selectable.sprites[index];
        disabledSprite = disabled.sprites[index];
#if DEBUG
        GetComponent<UnityEngine.UI.Button>()?.onClick.AddListener(() => DemonBuffs.AddBuff((DemonBuffs.DemonBuff)index + 1));
#endif
    }

    // Update is called once per frame
    void Tick()
    {
        if (SaveManager.GetSave().Pendants[index % 7] && (index / 7) < SaveManager.GetSave().Pendants.Count(a => a))
        {
            state = State.Selectable;

            foreach (DemonBuffs.DemonBuff demonBuff in DemonBuffs.demonBuffs)
            {
                if (demonBuff == DemonBuffs.DemonBuff.Inactive) continue;
                if ((int)demonBuff == index + 1) //cant select ourself again
                {
                    state = State.Disabled;
                    break;
                }
                if ((int)demonBuff % 7 == (index + 1) % 7) //cant select any other buff in this column
                {
                    state = State.Disabled;
                    break;
                }
                if (((int)demonBuff - 1) / 7 == index / 7) //or row
                {
                    state = State.Disabled;
                    break;
                }
            }
            //remove ghost if disabled
            if (state == State.Selectable && transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
        else
        {
            state = State.Inactive;
        }
        switch (state)
        {
            case State.Inactive:
                image.sprite = inactive;
                break;
            case State.Selectable:
                image.sprite = selectableSprite;
                break;
            case State.Disabled:
                image.sprite = disabledSprite;
                break;
        }
    }
#if DEBUG
    public void ClearBuffs()
    {
        DemonBuffs.ClearBuffs();
    }

#endif

}
