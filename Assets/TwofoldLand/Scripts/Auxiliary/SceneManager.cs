using UnityEngine;
using System.Collections;

public class SceneManager : Singleton<SceneManager>
{
    public GameObject auraPrefab;
    public GameObject skillPrefab;

    public GameObject SpawnAura(int amount, Vector3 position)
    {
        GameObject aura = Instantiate<GameObject>(auraPrefab);
        aura.transform.position = position;
        aura.GetComponent<AuraContainer>().auraAmount = amount;

        return aura;
    }

    public GameObject SpawnSkill(SkillData skillData, Vector3 position)
    {
        GameObject skill = Instantiate<GameObject>(skillPrefab);
        skill.transform.position = position;
        skill.GetComponent<SkillContainer>().skillData = skillData;

        return skill;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }    
}
