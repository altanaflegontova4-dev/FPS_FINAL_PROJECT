using UnityEngine;

public class HealController : MonoBehaviour
{
    public int healAmount = 15;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Player touch heal");
            PlayerHealthController.instance.healPlayer(healAmount);
            Destroy(gameObject);

        }
    }
}
