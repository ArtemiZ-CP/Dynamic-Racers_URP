using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement), typeof(CharacterGadgets))]
public class CharacterAnimation : MonoBehaviour
{
	private readonly int End = Animator.StringToHash(nameof(End));
	private readonly int SpeedMultiplier = Animator.StringToHash(nameof(SpeedMultiplier));
	private readonly int LaunchSpeed = Animator.StringToHash(nameof(LaunchSpeed));
	private readonly int Defeat = Animator.StringToHash(nameof(Defeat));
	private readonly int Victory = Animator.StringToHash(nameof(Victory));
	private readonly int Launch = Animator.StringToHash(nameof(Launch));
	private readonly int StartLaunch = Animator.StringToHash(nameof(StartLaunch));

	[SerializeField] private MeshSpawner _meshSpawner;
	[SerializeField] private Animator _slingAnimator;
	[SerializeField] private float _launchAnimationDuration;

	private GlobalSettings _globalSettings;
	private RunStagesBase _runStagesBase;
	private SpeedGameBase _speedGameBase;
	private CharacterGadgets _characterGadgets;
	private CharacterMovement _characterMovement;
	private Animator _animator;
	private Animator _gadgetAnimator;
	private GameObject _activeGadget;
	private Coroutine _launchAnimationCoroutine;
	private int _place = 0;

	private void Awake()
	{
		_globalSettings = GlobalSettings.Instance;
		_animator = _meshSpawner.Initialize();
		_characterMovement = GetComponent<CharacterMovement>();
		_characterGadgets = GetComponent<CharacterGadgets>();
		_runStagesBase = FindObjectOfType<RunStagesBase>();
		_speedGameBase = _runStagesBase.SpeedGame;
	}

	private void Start()
	{
		if (_characterGadgets.Gadget != null)
		{
			_activeGadget = Instantiate(_characterGadgets.Gadget.Prefab, _meshSpawner.transform);
			_gadgetAnimator = _activeGadget.GetComponent<Animator>();
			_activeGadget.SetActive(false);
		}

		_launchAnimationCoroutine = StartCoroutine(ControllLaunchAnimation());
	}

	private void OnEnable()
	{
		_characterGadgets.OnActiveAnimation += ActiveAnimationHandler;
		_characterGadgets.OnDisactiveAnimation += DisactiveAnimationHandler;
		_speedGameBase.EndedSpeedGame += LaunchCharacter;
	}

	private void OnDisable()
	{
		_characterGadgets.OnActiveAnimation -= ActiveAnimationHandler;
		_characterGadgets.OnDisactiveAnimation -= DisactiveAnimationHandler;
		_speedGameBase.EndedSpeedGame -= LaunchCharacter;
	}

	private void Update()
	{
		SetAnimationSpeed();
	}

	private void LateUpdate()
	{
		if (_runStagesBase.IsRunning)
		{
			_place = _runStagesBase.GetPlacement(_characterMovement);
		}
	}

	private void SetAnimationSpeed()
	{
		float speedMultiplier = _characterMovement.Speed;

		_animator.SetFloat(SpeedMultiplier, speedMultiplier);

		if (_activeGadget != null)
		{
			_gadgetAnimator.SetFloat(SpeedMultiplier, speedMultiplier);
		}
	}

	private IEnumerator ControllLaunchAnimation()
	{
		float lastCharactePosition = _characterMovement.CurrentOffset;
		float maxDistance = GlobalSettings.Instance.CharacterStartOffset;

		while (lastCharactePosition == transform.position.z)
		{
			yield return null;
		}

		_animator.SetTrigger(StartLaunch);
		_slingAnimator.SetTrigger(StartLaunch);

		while (true)
		{
			float currentDistance = _characterMovement.CurrentOffset - lastCharactePosition;
			float animationSpeed = currentDistance / maxDistance * _launchAnimationDuration / Time.deltaTime;

			_slingAnimator.SetFloat(LaunchSpeed, animationSpeed);
			_animator.SetFloat(LaunchSpeed, animationSpeed);
			lastCharactePosition = _characterMovement.CurrentOffset;

			yield return null;
		}
	}

	private void LaunchCharacter(float speedMultiplier)
	{
		StopCoroutine(_launchAnimationCoroutine);

		if (_characterMovement.CurrentOffset < _globalSettings.CharacterStartOffset)
		{
			_slingAnimator.SetFloat(LaunchSpeed, -_globalSettings.BaseSpeed);
		}
		else
		{
			_slingAnimator.SetTrigger(Launch);
			_animator.SetTrigger(Launch);
		}
	}

	private void ActiveAnimationHandler(GadgetChunkInfo gadgetChunkInfo, bool isGadgetActive)
	{
		if (gadgetChunkInfo.ChunkType == ChunkType.Finish)
		{
			if (_place == 1)
			{
				_animator.SetTrigger(Victory);
			}
			else
			{
				_animator.SetTrigger(Defeat);
			}
		}
		else
		{
			if (isGadgetActive)
			{
				_activeGadget.SetActive(true);
				_gadgetAnimator.SetTrigger(gadgetChunkInfo.AnimationTriggerName);
				_animator.SetTrigger(gadgetChunkInfo.AnimationTriggerName);
				// _animator.SetTrigger(gadgetChunkInfo.ChunkType.ToString());
				// Debug.LogWarning("Temporarily solution");
			}
			else
			{
				_animator.SetTrigger(gadgetChunkInfo.ChunkType.ToString());
			}
		}
	}

	private void DisactiveAnimationHandler(ChunkType chunkType)
	{
		_activeGadget.SetActive(false);
		_animator.SetTrigger(chunkType.ToString());
	}
}
