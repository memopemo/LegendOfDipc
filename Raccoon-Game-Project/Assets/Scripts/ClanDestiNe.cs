using UnityEngine;
using UnityEngine.Events;

//Activates if all secret predicates are true. Predicates have their own custom evaluations.
public class Secret : MonoBehaviour
{
    [SerializeField] SecretPredicate secretPredicate;
    [SerializeField] UnityEvent OnActivated;
    public void ActivateSecret()
    {
        string evaluation = secretPredicate.Evaluate();
        if (evaluation != "")
        {
            Debug.Log($"Secret predicate: {secretPredicate} was not fulfilled.\nReason: {evaluation}");
            return;
        }
        OnActivated.Invoke();
    }
    public void DebugSecretActivation()
    {
        print("Secret Activated!");
    }
}

public abstract class SecretPredicate : MonoBehaviour
{
    public abstract string Evaluate(); //give reason to why it was not fulfilled.
}

