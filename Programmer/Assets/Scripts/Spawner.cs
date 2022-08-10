using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<Wave> Waves;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            foreach (var wave in Waves)
            {
                for (int i = 0; i < wave.Count; i++)
                {
                    var enemy = wave.Enemy.GetFromPool();
                    enemy.transform.SetParent(transform);
                    enemy.transform.position = transform.position;
                    enemy.gameObject.SetActive(true);
                    yield return new WaitForSeconds(wave.Delay);
                }
                yield return new WaitForSeconds(5);
            }
        }
    }
}
