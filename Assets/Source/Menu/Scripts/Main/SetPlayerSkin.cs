using System;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerSkin : MonoBehaviour
{
    [Serializable]
    private class SkinPlace
    {
        public Transform Parent;
        public Material SkinMaterial;
        public RuntimeAnimatorController AnimatorController;
    }

    [SerializeField] private GameObject _playerSkin;
    [SerializeField] private List<SkinPlace> _skinPlaces;

    private void Start()
    {
        SetSkin();
    }

    private void SetSkin()
    {
        foreach (SkinPlace skinPlace in _skinPlaces)
        {
            if (skinPlace.Parent.childCount > 0)
            {
                foreach (Transform child in skinPlace.Parent)
                {
                    Destroy(child.gameObject);
                }
            }

            GameObject skin = Instantiate(_playerSkin, skinPlace.Parent);
            skin.AddComponent<Animator>().runtimeAnimatorController = skinPlace.AnimatorController;
            skin.GetComponentInChildren<SkinnedMeshRenderer>().material = skinPlace.SkinMaterial;
        }
    }
}
