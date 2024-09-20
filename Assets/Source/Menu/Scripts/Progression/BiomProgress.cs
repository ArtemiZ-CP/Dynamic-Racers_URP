using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BiomProgress : MonoBehaviour
{
    [SerializeField] private TMP_Text _biomName;
    [SerializeField] private RectTransform _biomPointPrefab;
    [SerializeField] private RectTransform _biomEmptyPointPrefab;
    [SerializeField] private RectTransform _biomParent;
    [SerializeField] private RectTransform _progressBar;
    [SerializeField] private Image _progressSlider;
    [SerializeField] private TMP_Text _coinsRewardPerStar;

    public void SetBiomPoints(ICompanyBiomInfoReadOnly companyBiomInfo)
    {
        ChestReward.ChestType?[] rewards = companyBiomInfo.Rewards.ToArray();

        if (rewards == null || rewards.Length == 0)
        {
            return;
        }

        ClearBiomPoints();

        for (int i = 0; i < rewards.Length; i++)
        {
            if (rewards[i] is ChestReward.ChestType chestType)
            {
                SpawnBiomPoint(chestType, i, rewards.Length);
            }
            else
            {
                SpawnEmptyBiomPoint(i, rewards.Length);
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

    private void SpawnBiomPoint(ChestReward.ChestType chestType, int progress, int maxProgress)
    {
        if (progress == maxProgress - 1)
        {
            return;
        }

        RectTransform pointTransform = Instantiate(_biomPointPrefab, _biomParent);
        pointTransform.position = GetPointPosition(progress, maxProgress);
    }

    private void SpawnEmptyBiomPoint(int progress, int maxProgress)
    {
        if (progress == maxProgress - 1)
        {
            return;
        }

        RectTransform pointTransform = Instantiate(_biomEmptyPointPrefab, _biomParent);
        pointTransform.position = GetPointPosition(progress, maxProgress);
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
