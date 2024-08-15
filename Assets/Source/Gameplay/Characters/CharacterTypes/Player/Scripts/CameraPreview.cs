using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraPreview : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private AnimationCurve _speedCurve;
    [SerializeField] private CinemachineDollyCart _cinemachineDollyCart;

    private CinemachineVirtualCamera _virtualCamera;

    public CinemachineVirtualCamera VirtualCamera => _virtualCamera;

    public IEnumerator Run()
    {
        _cinemachineDollyCart.enabled = true;

        while (_cinemachineDollyCart.m_Position <= 0.999f)
        {
            _cinemachineDollyCart.m_Position += _speed * _speedCurve.Evaluate(_cinemachineDollyCart.m_Position) * Time.deltaTime;
            yield return null;
        }

        _virtualCamera.LookAt = null;
    }

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _virtualCamera.Priority = 0;
        _cinemachineDollyCart.m_PositionUnits = CinemachinePathBase.PositionUnits.Normalized;
        _cinemachineDollyCart.enabled = false;
        _cinemachineDollyCart.m_Speed = 0;
    }
}
