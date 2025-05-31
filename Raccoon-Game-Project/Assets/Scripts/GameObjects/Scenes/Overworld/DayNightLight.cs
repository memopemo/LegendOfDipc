using UnityEngine;
using UnityEngine.Rendering.Universal;

//shades anything with a light or sprite renderer with day/night colors.
public class DayNightLight : MonoBehaviour
{
    Light2D light2D;
    SpriteRenderer sprite;
    [SerializeField] Gradient DayColors;
    [SerializeField] bool updateTime = true;

    // Start is called before the first frame update
    void Start()
    {
        light2D = GetComponent<Light2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(updateTime) DayNightSystem.UpdateTimeOutside();
        Color color = DayColors.Evaluate(DayNightSystem.GetDayProgress());
        if(light2D)
            light2D.color = color;
        if(sprite)
            sprite.color = color;
#if DEBUG
        if (Input.GetKeyDown(KeyCode.Semicolon)&&updateTime)
        {
            DayNightSystem.AddTime(100);
        }
#endif
    }
}
