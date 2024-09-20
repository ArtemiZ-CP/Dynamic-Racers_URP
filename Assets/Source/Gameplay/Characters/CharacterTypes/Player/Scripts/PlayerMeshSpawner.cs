using UnityEngine;

public class PlayerMeshSpawner : MeshSpawner
{
    [SerializeField] private Avatar _avatar;
    [SerializeField] private RuntimeAnimatorController _animator;
    [SerializeField] private GameObject _skin;

    public override Animator Initialize()
    {
        return GetComponentInChildren<Animator>();
    }
}
