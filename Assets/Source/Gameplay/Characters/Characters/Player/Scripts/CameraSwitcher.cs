using System.Collections;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CameraPreview _cameraPreview;
    [SerializeField] private CameraFollow _cameraFollow;

    public IEnumerator SwitchToPreviewCamera()
    {
        _cameraPreview.VirtualCamera.Priority = 1;
        _cameraFollow.VirtualCamera.Priority = 0;
        _cameraFollow.enabled = false;

        yield return StartCoroutine(_cameraPreview.Run());
    }

    public void SwitchToGameplayCamera()
    {
        _cameraPreview.VirtualCamera.Priority = 0;
        _cameraFollow.VirtualCamera.Priority = 1;
        _cameraFollow.enabled = true;
    }
}
