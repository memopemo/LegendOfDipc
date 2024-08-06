
using UnityEngine;

/* Visualizes a health bar above an enemy
 * Displays: 
 *  A background bar to show its max health
 *  A red bar to show its actual health
 *  A flashing bar to show how much damage was done.
 *  
 * The moving displays move in a smooth manner.
 * 
 * It also fades out after some time of not being hit.
 */
public class EnemyHealthBar : MonoBehaviour
{
    /* 
     * directly used in the alpha of the health bars.
     * since alphas over 1 are treated as 1, this can fade out after a while by setting this to a value higher than 1.
    */
    float fadeoutTimer = 0;
    Transform damageBar;

    const float FADEOUT_IN_SECS = 5;
    const float DAMAGE_FLASH_RATE = 0.05f;
    const float HEALTH_LERP_SPEED = 5;
    const float DAMAGE_LERP_SPEED = 1;
    [SerializeField] Color[] damageFlashColors;
    int damageFlashincrement;
    int startHealth;

    void Start()
    {
        damageBar = transform.parent.GetChild(1);
        InvokeRepeating(nameof(FlickerHitAmountBar), DAMAGE_FLASH_RATE, DAMAGE_FLASH_RATE);
        startHealth = GetComponentInParent<Health>().currentHealth;
    }

    void FlickerHitAmountBar()
    {
        SpriteRenderer damageBarSprite = damageBar.GetComponent<SpriteRenderer>();
        //flash White and Yellow
        damageBarSprite.color = damageFlashColors[damageFlashincrement];
        damageFlashincrement++;
        damageFlashincrement %= damageFlashColors.Length;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float currentHealth = transform.parent.GetComponentInParent<Health>().currentHealth;
        float percentHealth = currentHealth / startHealth;

        //Set Red Area (fills up percentage of the bg from the left
        transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, percentHealth, Time.deltaTime * HEALTH_LERP_SPEED), 1, 1);

        //Set White Area 
        damageBar.localScale = new Vector3(Mathf.Lerp(damageBar.localScale.x, percentHealth, Time.deltaTime * DAMAGE_LERP_SPEED), 1, 1);



        //Set alpha depending on time

        //us
        SpriteRenderer healthBarSprite = GetComponent<SpriteRenderer>();
        healthBarSprite.color = new Color(healthBarSprite.color.r, healthBarSprite.color.g, healthBarSprite.color.b, fadeoutTimer);

        //bg
        SpriteRenderer bgBarSprite = transform.parent.GetComponent<SpriteRenderer>();
        bgBarSprite.color = new Color(bgBarSprite.color.r, bgBarSprite.color.g, bgBarSprite.color.b, fadeoutTimer);

        //hit amount
        SpriteRenderer damageBarSprite = damageBar.GetComponent<SpriteRenderer>();
        damageBarSprite.color = new Color(damageBarSprite.color.r, damageBarSprite.color.g, damageBarSprite.color.b, fadeoutTimer);

        if(fadeoutTimer < 1)
        {
            damageBarSprite.color = Color.clear;
        }



        Timer.DecrementTimer(ref fadeoutTimer);

        transform.parent.SetLocalPositionAndRotation(Vector3.up * (transform.parent.GetComponentInParent<Heightable>().height + 1.5f), Quaternion.identity);
    }
    public void ShowHealthBar()
    {
        fadeoutTimer = FADEOUT_IN_SECS;
    }
}
