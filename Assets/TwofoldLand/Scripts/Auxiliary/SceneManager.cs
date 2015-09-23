using UnityEngine;
using System.Collections;

public class SceneManager : Singleton<SceneManager>
{
    public GameObject auraPrefab;

    public GameObject SpawnAura(int amount, Vector3 position)
    {
        GameObject aura = Instantiate<GameObject>(auraPrefab);
        aura.transform.position = position;
        aura.GetComponent<Aura>().auraAmount = amount;

        return aura;
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
