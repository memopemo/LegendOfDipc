using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent OnEnter;
    [SerializeField] UnityEvent OnExit;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.TryGetComponent(out PlayerStateManager player))
        {
            OnEnter.Invoke();
        }
    }
    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.TryGetComponent(out PlayerStateManager player))
        {
            OnExit.Invoke();
        }
    }
}