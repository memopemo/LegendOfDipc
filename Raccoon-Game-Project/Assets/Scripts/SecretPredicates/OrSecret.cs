using UnityEngine;

//Succeeds if any Secret Predicates within this predicate succeeded. (OR operator on multiple predicates)
public class OrSecret : SecretPredicate
{
    [SerializeField] SecretPredicate[] secretPredicates;
    public override string Evaluate()
    {
        foreach (var secret in secretPredicates)
        {
            if (secret == null || secret.Evaluate() != "")
            {
                continue;
            }
            else
            {
                return "";
            }

        }
        return "There was no predicate that succeeded.";
    }
}
