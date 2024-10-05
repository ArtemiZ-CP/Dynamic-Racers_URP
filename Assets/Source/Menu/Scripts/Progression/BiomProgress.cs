using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class BiomProgress : MonoBehaviour
{
    [SerializeField] private TMP_Text _biomName;
    [SerializeField] private BiomProgressPoint _biomPointPrefab;
    [SerializeField] private RectTransform _biomParent;
    [SerializeField] private RectTransform _progressBar;
    [SerializeField] private Image _progressSlider;
    [SerializeField] private TMP_Text _coinsRewardPerStar;

    private ICompanyBiomInfoReadOnly _currentBiomInfo;

    public void SetBiomPoints(ICompanyBiomInfoReadOnly companyBiomInfo)
    {
        _currentBiomInfo = companyBiomInfo;
    }

    private void FixedUpdate()
    {
        if (_currentBiomInfo != null)
        {
            UpdateBiomPoints(_currentBiomInfo);
        }
    }

    private void UpdateBiomPoints(ICompanyBiomInfoReadOnly companyBiomInfo)
    {
        BiomReward[] rewards = companyBiomInfo.Rewards.ToArray();

        if (rewards == null || rewards.Length == 0)
        {
            return;
        }

        ClearBiomPoints();

        for (int i = 0; i < rewards.Length; i++)
        {
            if (rewards[i] != null)
            {
                SpawnBiomPoint(rewards[i], i, rewards.Length);
            }
        }

        _biomName.text = companyBiomInfo.BiomName;
        _coinsRewardPerStar.text = companyBiomInfo.RewardsPerStar.Amount.ToString();
        _progressSlider.fillAmount = companyBiomInfo.CurrentStars / (float)rewards.Length;
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

    private void SpawnBiomPoint(BiomReward biomReward, int progress, int maxProgress)
    {
        BiomProgressPoint pointTransform = Instantiate(_biomPointPrefab, _biomParent);
        pointTransform.transform.position = GetPointPosition(progress, maxProgress);
        pointTransform.Initialize(progress + 1, biomReward, progress == maxProgress - 1);
    }

    private Vector3 GetPointPosition(int progress, int maxProgress)
    {
        float leftPosition = _progressBar.rect.xMin * transform.lossyScale.x + _progressBar.position.x;
        float rightPosition = _progressBar.rect.xMax * transform.lossyScale.x + _progressBar.position.x;

        return new Vector3(
            Mathf.Lerp(leftPosition, rightPosition, (progress + 1) / (float)maxProgress),
            _biomParent.position.y,
            _biomParent.position.z
        );
    }
}
