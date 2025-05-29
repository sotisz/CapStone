using UnityEngine;
using UnityEngine.UI;

public class Tagebar : MonoBehaviour
{
    public Image gaugeBarFill;
    public float gaugeBarFillCooldown = 0f;
    public float gaugeBarFillSpeed = 2f;
    private bool gaugeBarFilling = false;
    public Image gaugeBarFillBackground;
    
    void Start()
    {
        gaugeBarFill.fillAmount = 0f;
        gaugeBarFill.enabled = false;
        gaugeBarFillBackground.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !gaugeBarFilling)
        {
            gaugeBarFill.enabled = true;
            gaugeBarFillBackground.enabled = true;
            StartCooldown();
        }
        if (gaugeBarFilling)
        {
            gaugeBarFillCooldown += Time.deltaTime;

            float Ratio = Mathf.Clamp01(gaugeBarFillCooldown / gaugeBarFillSpeed);
           
            gaugeBarFill.fillAmount = Ratio;
            
            Debug.Log("fillAmount: " + gaugeBarFill.fillAmount);
            if (gaugeBarFillCooldown >= gaugeBarFillSpeed)
            {
                gaugeBarFilling = false;
                gaugeBarFillCooldown = 0f;
                
                gaugeBarFill.enabled = false;
               gaugeBarFillBackground.enabled = false;
            }
        }
        
    }
    void StartCooldown()
    {
        Debug.Log("StartCooldown");
        gaugeBarFillCooldown = 0f;
            
        gaugeBarFilling = true;
            
        gaugeBarFill.fillAmount = 0f;
    }
}
