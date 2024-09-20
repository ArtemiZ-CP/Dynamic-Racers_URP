using UnityEngine;

public class EnemyMeshSpawner : MeshSpawner
{
    [SerializeField] private Avatar _avatar;
    [SerializeField] private RuntimeAnimatorController _animator;
    [SerializeField] private GameObject _skin;

    public override Animator Initialize()
    {
        DestroyAllMeshes();
        SpawnMesh();
        return GetComponentInChildren<Animator>();
    }

    private void SpawnMesh()
    {
        GameObject skin;

        if (_skin != null)
        {
            skin = _skin;
        }
        else
        {
            skin = GlobalSettings.Instance.GetRandomSkin();
        }

        skin = Instantiate(skin, transform);
        Animator animator = skin.AddComponent<Animator>();
        animator.avatar = _avatar;
        animator.runtimeAnimatorController = _animator;
    }

    private void DestroyAllMeshes()
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}
