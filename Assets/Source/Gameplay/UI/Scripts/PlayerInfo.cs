using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private PlayerPlacement _playerPlacement;

    public void SetPlace(int place)
    {
        _playerPlacement.SetPlace(place);
    }
}
