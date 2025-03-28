using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private Slider slider;

    [SerializeField] private Image rollImage;
    [SerializeField] private Image potionImage;

    [SerializeField] private float cooldownRoll;

    private Entity player;
   
    private void Start()
    {
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;

       player = Player_Move.instance;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
            SetCoolDownOf(rollImage);

        if (Input.GetKeyUp(KeyCode.Alpha2) && Inventory.instance.GetEquipment(EquipmentType.Food) != null)
            SetCoolDownOf(potionImage);
            
        

       // CheckCooldowOf(potionImage, Inventory.instance.flaskCooldown);


    }
    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    private void SetCoolDownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldowOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }
}
