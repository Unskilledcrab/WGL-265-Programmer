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
        foreach (var wave in Waves)
        {
            for (int i = 0; i < wave.Count; i++)
            {
                Instantiate(wave.Enemy, transform);
                yield return new WaitForSeconds(wave.Delay);
            }
            yield return new WaitForSeconds(wave.Delay * 4);
        }
    }
}
