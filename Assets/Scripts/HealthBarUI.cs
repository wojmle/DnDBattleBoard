using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image backgroundImage;
    
    private Transform target;
    private Vector3 offset;

    public void Initialize(Transform target, Vector3 offset)
    {
        this.target = target;
        this.offset = offset;
    }

    public void SetHealth(int current)
    {
        healthText.text = $"{current}";
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Position the health bar above the target in world space
            transform.position = target.position + offset;
            // Or, if using a screen-space canvas:
            // transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
            // Make the health bar face the camera
            var cam = Camera.main;
            if (cam != null)
            {
                // Option 1: LookAt with up vector
                transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                    cam.transform.rotation * Vector3.up);

            }
        }
    }
}