using System.Collections;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] private ActiveMenu _activeMenu;
    [SerializeField] private GameObject _menuToActive;
    [SerializeField] private float _timeToActive;
    [SerializeField] private Animator _selectAnimation;
    [SerializeField] private string _selectTrigger;
    [SerializeField] private Animator[] _notSelectAnimation;
    [SerializeField] private string _notSelectTrigger;

    private bool _isPressed;

    private void OnEnable()
    {
        _isPressed = false;
    }

    public void ActiveMenu()
    {
        if (_isPressed)
        {
            return;
        }

        StartCoroutine(ActiveMenuCoroutine());
    }

    private IEnumerator ActiveMenuCoroutine()
    {
        _isPressed = true;

        _selectAnimation.SetTrigger(_selectTrigger);

        if (_notSelectAnimation.Length != 0)
        {
            foreach (var animator in _notSelectAnimation)
            {
                animator.SetTrigger(_notSelectTrigger);
            }
        }

        yield return new WaitForSeconds(_timeToActive);
        _activeMenu.SetActiveMenu(_menuToActive);
    }
}
