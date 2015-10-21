using UnityEngine;
using System.Collections;

public class SceneManager : Singleton<SceneManager>
{
    public GameObject auraPrefab;
    public GameObject skillPrefab;
    public GameObject itemPrefab;

    public GameObject SpawnCollectable(CollectableData data, Vector3 position)
    {
        if (data.GetType() == typeof(SkillData))
            return SpawnSkill(data as SkillData, position);

        if (data.GetType() == typeof(ItemData))
            return SpawnItem(data as ItemData, position);

        Debug.LogWarning("Unable to spawn from given CollectableData");

        return null;
    }

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

    public GameObject SpawnItem(ItemData itemData, Vector3 position)
    {
        GameObject item = Instantiate<GameObject>(itemPrefab);
        item.transform.position = position;
        item.GetComponent<ItemContainer>().itemData = itemData;

        return item;
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
