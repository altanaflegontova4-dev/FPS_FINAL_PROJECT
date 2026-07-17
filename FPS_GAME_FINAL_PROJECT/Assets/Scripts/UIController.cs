using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public static UIController instance;

    public Slider healthSlider;
    public Text healthText;
    public Text ammoText;

    public void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

 
    void Update()
    {
        
    }
}
