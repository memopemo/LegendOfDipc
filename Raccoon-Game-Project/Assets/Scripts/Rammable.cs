using UnityEngine;
using UnityEngine.Events;

public class Rammable : MonoBehaviour
{
    [SerializeField] UnityEvent onRamInto;
    public bool isSolid;
    public void OnRamInto()
    {
        onRamInto.Invoke();
    }
}