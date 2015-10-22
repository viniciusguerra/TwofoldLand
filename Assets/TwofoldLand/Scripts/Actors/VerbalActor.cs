using UnityEngine;
using System.Collections;

public class VerbalActor : Actor
{
    public override void SetSelected()
    {
        base.SetSelected();

        if(Player.Instance.KnowsInterface(typeof(IVerbal)))
            HUD.Instance.messageBox.Show(Entity as IVerbal);
    }

    public override void Deselect()
    {
        base.Deselect();

        if(HUD.Instance.messageBox.CurrentVerbal == Entity as IVerbal)
            HUD.Instance.messageBox.Hide();
    }
}
