using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//Fire objects will attach to this and trigger something to happen on burning.
public class Burnable : MonoBehaviour
{
    [SerializeField] UnityEvent OnBurn;
    [SerializeField] UnityEvent OnStartBurn;

    public void Burn()
    {
        OnBurn.Invoke();
    }
    public void StartBurn()
    {
        OnStartBurn.Invoke();
    }

    //Some standard burn event functions for you :)
    public void Die()
    {
        Destroy(gameObject);
    }
    //could change sprite to a burnt version.
    public void ChangeSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
    public void PrematurelyEndFire()
    {
        GetComponentInChildren<Fire>().Die();
    }

    //remove this burnable script.
    public void SetBurnt()
    {
        Destroy(this);
    }

}
