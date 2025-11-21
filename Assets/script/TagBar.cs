using UnityEngine;
using UnityEngine.UI;

public class Tagbar : MonoBehaviour
{
    public Image gaugeBarFill;
    public float gaugeBarFillCooldown = 0f;
    public float gaugeBarFillSpeed = 2f;
    private bool gaugeBarFilling = false;
    public Image gaugeBarFillBackground;
    public GameObject target1, target2;
    public Vector3 Offset = new Vector3(-0.5f, 1.7f, 0);
    public bool tagAble = true;

    void Start()
    {
        gaugeBarFill.fillAmount = 0f;
        gaugeBarFill.enabled = false;
        gaugeBarFillBackground.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gaugeBarFilling)
        {
            gaugeBarFillCooldown += Time.deltaTime;

            float Ratio = Mathf.Clamp01(gaugeBarFillCooldown / gaugeBarFillSpeed);

            gaugeBarFill.fillAmount = Ratio;

            if (gaugeBarFillCooldown >= gaugeBarFillSpeed)
            {
                gaugeBarFilling = false;
                gaugeBarFillCooldown = 0f;

                gaugeBarFill.enabled = false;
                gaugeBarFillBackground.enabled = false;
                tagAble = true;
            }
        }
    }

    void LateUpdate()
    {
        if (target1.activeInHierarchy)
        {
            transform.position = target1.transform.position + Offset;
        }
        else if (target2.activeInHierarchy)
        {
            transform.position = target2.transform.position + Offset;
        }
    }

    public void TagPlayer()
    {
        gaugeBarFill.enabled = true;
        gaugeBarFillBackground.enabled = true;
        StartCooldown();
        tagAble = false;
    }

    void StartCooldown()
    {
        gaugeBarFillCooldown = 0f;

        gaugeBarFilling = true;

        gaugeBarFill.fillAmount = 0f;
    }
}