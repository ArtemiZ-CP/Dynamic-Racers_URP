using System.Collections.Generic;
using UnityEngine;

public class EnemyMeshSpawner : MeshSpawner
{
    [SerializeField] private List<GameObject> _enemyMeshes;

    public override Animator Initialize()
    {
        DestroyAllMeshes();
        SpawnMesh();
        return GetComponentInChildren<Animator>();
    }

    private void SpawnMesh()
    {
        int randomIndex = Random.Range(0, _enemyMeshes.Count);
        Instantiate(_enemyMeshes[randomIndex], transform);
    }

    private void DestroyAllMeshes()
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}
