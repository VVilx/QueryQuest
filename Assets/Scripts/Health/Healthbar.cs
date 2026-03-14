using UnityEngine;

public class Healthbar : MonoBehaviour
{
   [SerializeField] private Health playerHealth;
   [SerializeField] private UnityEngine.UI.Image totalhealthbar;
   [SerializeField] private UnityEngine.UI.Image currenthealthbar;

    private void Start()
    {
        totalhealthbar.fillAmount = playerHealth.currentHealth / 10;
    }

    private void Update()
    {
        currenthealthbar.fillAmount = playerHealth.currentHealth / 10;
    }
}    