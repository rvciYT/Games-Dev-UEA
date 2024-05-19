using UnityEngine;
using UnityEngine.UI;

public class HealthBarTownhouse : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private float reducespeed = 2;
    //[SerializeField] private float hideDelay = 3f; 
    private float target = 1;
    private Camera cam;
    //private bool isVisible = false; 
    //private float hitTimer = 0f; 

    void Start()
    {
        cam = Camera.main;
        //healthBarFill.enabled = false;
        //isVisible = false;
        
    }

    public void UpdateHealthBar(float currentHealth, float totalHealth)
    {
        float fillAmountChange = currentHealth / totalHealth;
        target = fillAmountChange;

       
        // if (!isVisible)
        // {
        //     healthBarFill.enabled = true;
        //     isVisible = true;
        // }

        
        // hitTimer = 0f;

        
        UpdateHealthBarColor(target);
    }

    void Update() 
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        healthBarFill.fillAmount = Mathf.MoveTowards(healthBarFill.fillAmount, target, reducespeed * Time.deltaTime);

        // if (isVisible && Mathf.Approximately(healthBarFill.fillAmount, target))
        // {
            
        //     hitTimer += Time.deltaTime;
        //     if (hitTimer >= hideDelay)
        //     {
                
        //         healthBarFill.enabled = false;
        //         isVisible = false;
        //     }
        // }
    }

    void UpdateHealthBarColor(float fillAmount)
    {
        if (fillAmount <= 0.6f && fillAmount > 0.3f)
        {
            healthBarFill.color = new Color(1f, 0.92f, 0.016f); // Yellow
        }
        else if (fillAmount <= 0.3f)
        {
            healthBarFill.color = new Color(0.831f, 0.031f, 0.031f); // Red
        }
        else
        {
            healthBarFill.color = new Color(0.094f, 0.655f, 0.031f); // Green
        }
    }
}
