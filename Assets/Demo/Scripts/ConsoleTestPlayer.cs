using UnityEngine;
using TMPro;

public class ConsoleTestPlayer : MonoBehaviour
{
    [SerializeField] int health = 100;

    TMP_Text healthText;

    private void Start()
    {
        healthText = FindObjectOfType<Canvas>().transform.Find("Player Health Text").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        healthText.text = "Player Health: " + health.ToString();
    }

    [ConsoleCommand]
    public void DealDamage(int damage)
    {
        health = Mathf.Max(health - damage, 0);

        if (health <= 0)
        {
            Debug.Log("Player died");
        }
    }
}
