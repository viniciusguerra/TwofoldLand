using UnityEngine;
using System.Collections;

public class ActiveEntity : Entity
{
    [Header("Stamina")]
    [SerializeField]
    protected float maxStamina;
    [SerializeField]
    protected float currentStamina;

    public float staminaPerSecond = 2;
    public float staminaWaitToRegen = 1;
    protected float staminaWaitToRegenCount;
    protected bool regeneratingStamina = false;

    protected bool IsRegeneratingStamina
    {
        get
        {
            return regeneratingStamina;
        }
        set
        {
            regeneratingStamina = value;

            if (regeneratingStamina && CurrentStamina < MaxStamina)
            {
                CurrentStamina += CurrentStamina * 0.01f;
            }
            else
            {
                staminaWaitToRegenCount = 0;
            }
        }
    }

    public virtual float CurrentStamina
    {
        get
        {
            return currentStamina;
        }
        set
        {
            if (value < currentStamina)
                IsRegeneratingStamina = false;

            currentStamina = Mathf.Min(Mathf.Max(0, value), MaxStamina);
        }
    }

    public virtual float MaxStamina
    {
        get
        {
            return maxStamina;
        }
        set
        {
            maxStamina = Mathf.Max(0, value);
        }
    }


    public virtual void Update()
    {
        if (regeneratingStamina)
        {
            if (currentStamina < maxStamina)
            {
                CurrentStamina += Time.deltaTime * staminaPerSecond;
            }
        }
        else
        {
            staminaWaitToRegenCount += Time.deltaTime;

            if (staminaWaitToRegenCount >= staminaWaitToRegen)
                IsRegeneratingStamina = true;
        }
    }
}
