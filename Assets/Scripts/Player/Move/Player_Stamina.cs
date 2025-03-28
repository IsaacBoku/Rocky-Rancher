using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Stamina : MonoBehaviour
{
    [Header("Stamina main parameters")]
    public float playerStamina = 100.0f;
    [SerializeField] private float maxStamina = 100.0f;
    [HideInInspector] public bool hasRegenerated = true;
    [HideInInspector] public bool weAreRoll = false;

    [Header("Stamina rege parameters")]
    [Range(0,50)][SerializeField] private float staminaDrain = 0.5f;
    [Range(0, 50)][SerializeField] private float staminaregen = 0.5f;

    [Header("Stamina UI parameters")]
    [SerializeField]private Image staminaProgressUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    private Player_Move playerController;
    private void Start()
    {
        playerController = GetComponent<Player_Move>();
    }
    private void Update()
    {
        if (!weAreRoll)
        {
            if(playerStamina <= maxStamina - 0.01)
            {
                playerStamina += staminaregen * Time.deltaTime;
                UpdateStamina(1);
                if(playerStamina >= maxStamina)
                {
                    sliderCanvasGroup.alpha = 0;
                    hasRegenerated = true;
                }
            }
        }
    }
    public void Sprinting()
    {
        if (hasRegenerated)
        {
            weAreRoll = true;
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);
            if(playerStamina<= 0)
            {
                hasRegenerated = false;
                sliderCanvasGroup.alpha = 0;
            }
        }
    }
    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;
        if(value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }
}
