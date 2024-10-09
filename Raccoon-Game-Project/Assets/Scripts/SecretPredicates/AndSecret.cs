using UnityEngine;

//Succeeds if all Secret Predicates within this predicate succeeded. (AND operator on multiple predicates)
public class AndSecret : SecretPredicate
{
    [SerializeField] SecretPredicate[] secretPredicates;
    public override string Evaluate()
    {
        string reason = "";
        foreach (var secret in secretPredicates)
        {
            if (secret == null)
            {
                reason += "\nOne of the predicates was not set.";
                continue;
            }
            string evaluation = secret.Evaluate();
            if (evaluation != "")
            {
                reason += ($"\nSecret predicate: {secret} was not fulfilled.\nReason: {evaluation}");
            }
        }
        return reason;
    }
}
