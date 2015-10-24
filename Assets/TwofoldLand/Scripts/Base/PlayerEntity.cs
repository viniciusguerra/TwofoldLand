using UnityEngine;
using System.Collections;

public class PlayerEntity : ActiveEntity
{
    public override float CurrentStamina
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

            HUD.Instance.SetStaminaBarValueOverride(currentStamina);
        }
    }

    public override float MaxStamina
    {
        get
        {
            return maxStamina;
        }
        set
        {
            maxStamina = Mathf.Max(0, value);

            HUD.Instance.SetMaxStamina(maxStamina);
        }
    }
}
