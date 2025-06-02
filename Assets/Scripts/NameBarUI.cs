using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NameBarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    
    private Transform target;
    private Vector3 offset;

    public void Initialize(Transform target, Vector3 offset, string name)
    {
        this.target = target;
        this.offset = offset;
        nameText.text = name;
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