using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Heart : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public Sprite shinyFullHeart;
    public Sprite shinyHalfHeart;
    public Sprite shinyEmptyHeart;
    public int currentMaxHealth;
    public int currentHealth;

    // Update is called once per frame
    void Update()
    {
        currentHealth = FindFirstObjectByType<PlayerHealth>().currentHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, currentMaxHealth);

        currentMaxHealth = (SaveManager.GetSave().HeartContainersCollected.Count((x) => x) + 3) * 2;
        currentMaxHealth = Mathf.Clamp(currentMaxHealth, 0, 48);
        Image[] hearts = GetComponentsInChildren<Image>(true);


        //apply max health (show/disable)
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(currentMaxHealth > i * 2);
        }
        //set each one to be either full or empty
        for (int i = 0; i < hearts.Length; i++)
        {
            Image heart = hearts[i];
            if ((i + 1) * 2 <= currentHealth)
            {
                if (i < 16)
                    heart.sprite = fullHeart;
                else
                    heart.sprite = shinyFullHeart;
            }
            else
            {
                if (i < 16)
                    heart.sprite = emptyHeart;
                else
                    heart.sprite = shinyEmptyHeart;
            }
        }
        //if health is odd, set the boundary one to be half.
        if (currentHealth % 2 == 1)
        {
            if (currentHealth / 2 < 16)
                hearts[Mathf.FloorToInt(currentHealth / 2)].sprite = halfHeart;
            else
                hearts[Mathf.FloorToInt(currentHealth / 2)].sprite = shinyHalfHeart;
        }

    }
}
