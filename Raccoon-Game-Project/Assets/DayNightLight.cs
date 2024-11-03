using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightLight : MonoBehaviour
{
    Light2D light2D;
    [SerializeField] Gradient DayColors;

    // Start is called before the first frame update
    void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        DayNightSystem.UpdateTimeOutside();
        light2D.color = DayColors.Evaluate(DayNightSystem.GetDayProgress());
    }
}
