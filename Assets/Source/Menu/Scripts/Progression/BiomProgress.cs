using System;
using System.Collections.Generic;
using UnityEngine;

public class BiomProgress : MonoBehaviour
{
    [Serializable]
    private class BiomPoint
    {
        [SerializeField, Range(0, 1)] private float _progress;
        [SerializeField] private Reward _reward;

        public BiomPoint(int progress, Reward reward)
        {
            _progress = progress;
            _reward = reward;
        }

        public float Progress => _progress;
        public Reward Reward => _reward;
    }

    [SerializeField] private Reward _endReward;
    [SerializeField] private List<BiomPoint> _biomPoints = new();
    [SerializeField] private RectTransform _biomPointPrefab;
    [SerializeField] private RectTransform _biomParent;
    [SerializeField] private RectTransform _progressBar;

    [ContextMenu("Set Biom Points")]
    public void SetBiomPoints()
    {
        if (_biomPoints.Count == 0 || _biomPointPrefab == null)
        {
            return;
        }

        ClearBiomPoints();

        foreach (BiomPoint biomPoint in _biomPoints)
        {
            SpawnBiomPoint(biomPoint);
        }
    }

    private void ClearBiomPoints()
    {
        if (_biomParent.childCount == 0)
        {
            return;
        }

        for (int i = _biomParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(_biomParent.GetChild(i).gameObject);
        }
    }

    private void SpawnBiomPoint(BiomPoint biomPoint)
    {
        float leftPosition = _progressBar.rect.xMin * transform.lossyScale.x + _progressBar.position.x;
        float rightPosition = _progressBar.rect.xMax * transform.lossyScale.x + _progressBar.position.x;

        RectTransform pointTransform = Instantiate(_biomPointPrefab, _biomParent);
        pointTransform.position = new Vector3(
            Mathf.Lerp(leftPosition, rightPosition, biomPoint.Progress),
            _biomParent.position.y,
            _biomParent.position.z
        );
    }
}
