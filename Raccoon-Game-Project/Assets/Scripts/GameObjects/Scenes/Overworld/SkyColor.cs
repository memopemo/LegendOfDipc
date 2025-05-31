using UnityEngine;

public class CameraBGColor : MonoBehaviour
{
    [SerializeField] Gradient DayColors;
    Camera camera;
    void Start()
    {
        camera = GetComponent<Camera>();
    }
    void Update()
    {
        Color color = DayColors.Evaluate(DayNightSystem.GetDayProgress());
        camera.backgroundColor = color;
    }
}
