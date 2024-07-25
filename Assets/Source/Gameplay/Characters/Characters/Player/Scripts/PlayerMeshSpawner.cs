using UnityEngine;

public class PlayerMeshSpawner : MeshSpawner
{
    public override Animator Initialize()
    {
        return GetComponentInChildren<Animator>();
    }
}
