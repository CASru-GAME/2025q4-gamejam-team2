using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] tetrominoPrefabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    public void Spawn ()
    {
        Instantiate(
            tetrominoPrefabs[Random.Range(0, tetrominoPrefabs.Length)],
            transform.position,
            Quaternion.identity

        );
    }
}
