using UnityEngine;
using UnityEngine.UI;

public class PlayerSystemController : MonoBehaviour
{
    public PlayerSystems.BaseSystem playerFood;
    public PlayerSystems.BaseSystemUI playerFoodUI;
    public PlayerSystemConfig playerFoodConfig;
    public Slider sliderFood;

    public bool addPoints;
    public bool removePoints;
    public float amountOfAddedPoints;
    void Start()
    {
        playerFood = new PlayerSystems.BaseSystem();
        playerFood.InitSystem(playerFoodConfig);

        playerFoodUI = new PlayerSystems.BaseSystemUI();
        playerFoodUI.InitUI(playerFoodConfig, playerFood, sliderFood);
        playerFood.OnValueUpdate += () => playerFoodUI.SetUI(playerFood);

        

        
    }

    void Update()
    {
        if(addPoints)
        {
            addPoints = false;
            playerFood.AddToValue(amountOfAddedPoints);
        }
        if(removePoints)
        {
            removePoints = false;
            playerFood.SubstractFromValue(amountOfAddedPoints);
        }
    }
}
