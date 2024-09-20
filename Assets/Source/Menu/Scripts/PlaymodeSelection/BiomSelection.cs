using UnityEngine.UI;
using System.Linq;
using UnityEngine;

public class BiomSelection : MonoBehaviour
{
    [SerializeField] private ModeSelectionScreen _modeSelectionScreen;
    [SerializeField] private BiomProgress _biomProgress;
    [SerializeField] private Image _biomIcon;
    [SerializeField] private GameObject _previewBiom;
    [SerializeField] private GameObject _nextBiom;

    private ICompanyBiomInfoReadOnly[] _bioms;
    private int _currentBiomIndex = 0;

    private void OnEnable()
    {
        _bioms = PlayerData.CompanyBiomInfos.ToArray();
        SetFirstNotComletedBiom();
        SelectBiom();
    }

    public void OpenPreviewBiom()
    {
        _currentBiomIndex--;
        SelectBiom();
    }

    public void OpenNextBiom()
    {
        _currentBiomIndex++;
        SelectBiom();
    }

    private void SetActiveBiomSwitchButtons()
    {
        if (_currentBiomIndex == 0)
        {
            _previewBiom.SetActive(false);
        }
        else
        {
            _previewBiom.SetActive(true);
        }

        if (_currentBiomIndex == _bioms.Length - 1)
        {
            _nextBiom.SetActive(false);
        }
        else
        {
            _nextBiom.SetActive(true);
        }
    }

    private void SetFirstNotComletedBiom()
    {
        for (int i = 0; i < _bioms.Length; i++)
        {
            if (_bioms[i].CurrentStars < _bioms[i].Rewards.Count)
            {
                _currentBiomIndex = i;
                return;
            }
        }

        _currentBiomIndex = _bioms.Length - 1;
    }

    private void SelectBiom()
    {
        SetActiveBiomSwitchButtons();
        _biomIcon.sprite = _bioms[_currentBiomIndex].BiomSprite;
        _modeSelectionScreen.ChooseCompanyMapPreset(_bioms[_currentBiomIndex]);
        _biomProgress.SetBiomPoints(_bioms[_currentBiomIndex]);
    }
}
