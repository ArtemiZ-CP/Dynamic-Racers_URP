using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private readonly int ChangePlace = Animator.StringToHash(nameof(ChangePlace));

    [SerializeField] private PlayerPlacement _playerPlacement;
    [SerializeField] private Animator _animator;

    private int _place;

    public void SetPlace(int place)
    {
        if (_place != place)
        {
            _playerPlacement.SetPlace(place);
            _animator.SetTrigger(ChangePlace);
            _place = place;
        }
    }
}
