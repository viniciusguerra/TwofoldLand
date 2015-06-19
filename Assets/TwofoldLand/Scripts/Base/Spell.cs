using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour
{
    public string spellName;
    public Command[] commands;    

    void Start()
    {

    }

    //Called when a particle emitted by the Spell hits an Actor
    void OnParticleCollision(GameObject hitObject)
    {
        //ActorIA actor = hitObject.GetComponent<ActorIA>();

        //if (actor != null)
        //    actor.SubmitSpell(this);
    }
}
