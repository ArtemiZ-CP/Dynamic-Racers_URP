using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement), typeof(CharacterGadgets))]
public class CharacterAnimation : MonoBehaviour
{
	private const float _goodStartAnimationDelay = 0.5f;
	private const float _badStartAnimationDelay = 0.5f;

	private readonly int End = Animator.StringToHash(nameof(End));
	private readonly int SpeedMultiplier = Animator.StringToHash(nameof(SpeedMultiplier));
	private readonly int LaunchSpeed = Animator.StringToHash(nameof(LaunchSpeed));
	private readonly int Defeat = Animator.StringToHash(nameof(Defeat));
	private readonly int Victory = Animator.StringToHash(nameof(Victory));
	private readonly int Launch = Animator.StringToHash(nameof(Launch));
	private readonly int BadLaunch = Animator.StringToHash(nameof(BadLaunch));
	private readonly int StartLaunch = Animator.StringToHash(nameof(StartLaunch));

	[SerializeField] private MeshSpawner _meshSpawner;
	[SerializeField] private Animator _slingAnimator;
	[SerializeField] private float _launchAnimationDuration;
	[SerializeField] private GameObject _characterPoint;

	private GlobalSettings _globalSettings;
	private RunStagesBase _runStagesBase;
	private CharacterGadgets _characterGadgets;
	private CharacterMovement _characterMovement;
	private Animator _animator;
	private Animator _gadgetAnimator;
	private GameObject _activeGadget;
	private Coroutine _launchAnimationCoroutine;
	private int _place = 0;
	private bool _isAbleToAnimate = false;
	private GadgetChunkInfo _activeGadgetChunkInfo;
	private bool _isGadgetActive = false;

	private void Awake()
	{
		_globalSettings = GlobalSettings.Instance;
		_animator = _meshSpawner.Initialize();
		_characterMovement = GetComponent<CharacterMovement>();
		_characterGadgets = GetComponent<CharacterGadgets>();
		_runStagesBase = FindObjectOfType<RunStagesBase>();
		if (_characterPoint != null) _characterPoint.SetActive(false);
	}

	private void Start()
	{
		if (_characterGadgets.Gadget != null)
		{
			_activeGadget = Instantiate(_characterGadgets.Gadget.ScriptableObject.Prefab, _meshSpawner.transform);
			_gadgetAnimator = _activeGadget.GetComponent<Animator>();
			_activeGadget.SetActive(false);
		}

		_launchAnimationCoroutine = StartCoroutine(ControllLaunchAnimation());
	}

	private void OnEnable()
	{
		_characterGadgets.OnActiveAnimation += ActiveAnimationHandler;
		_characterGadgets.OnDisactiveAnimation += DisactiveAnimationHandler;
	}

	private void OnDisable()
	{
		_characterGadgets.OnActiveAnimation -= ActiveAnimationHandler;
		_characterGadgets.OnDisactiveAnimation -= DisactiveAnimationHandler;
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

	public void LaunchCharacter(bool goodStart, bool fullCharge)
	{
		if (_characterPoint != null) _characterPoint.SetActive(true);
		StopCoroutine(_launchAnimationCoroutine);

		if (fullCharge == false)
		{
			_slingAnimator.SetFloat(LaunchSpeed, -_globalSettings.BaseSpeed);
			StartCoroutine(StartAnimationDelay(0));
			print(1);
		}
		else
		{
			if (goodStart)
			{
				_slingAnimator.SetTrigger(Launch);
				_animator.SetTrigger(Launch);
				StartCoroutine(StartAnimationDelay(_goodStartAnimationDelay));
			}
			else
			{
				_slingAnimator.SetTrigger(BadLaunch);
				_animator.SetTrigger(BadLaunch);
				StartCoroutine(StartAnimationDelay(_badStartAnimationDelay));
			}
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

	private IEnumerator StartAnimationDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		_isAbleToAnimate = true;
		ActiveAnimationHandler(_activeGadgetChunkInfo, _isGadgetActive);
	}

	private IEnumerator ControllLaunchAnimation()
	{
		float lastCharactePosition = _characterMovement.CurrentOffset;
		float maxDistance = GlobalSettings.Instance.CharacterStartOffset;

		// while (lastCharactePosition == _characterMovement.CurrentOffset)
		// {
		// 	yield return null;
		// }

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

	private void ActiveAnimationHandler(GadgetChunkInfo gadgetChunkInfo, bool isGadgetActive)
	{
		if (gadgetChunkInfo.ChunkType == ChunkType.Finish)
		{
			if (_place == 1 || _place == 2 || _place == 3)
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
			if (_isAbleToAnimate == false)
			{
				_activeGadgetChunkInfo = gadgetChunkInfo;
				_isGadgetActive = isGadgetActive;
				return;
			}

			if (isGadgetActive)
			{
				_activeGadget.SetActive(true);
				_gadgetAnimator.SetTrigger(gadgetChunkInfo.AnimationTriggerName);
				_animator.SetTrigger(gadgetChunkInfo.AnimationTriggerName);
			}
			else
			{
				if (gadgetChunkInfo.ChunkType == ChunkType.Start)
				{
					_animator.SetTrigger(ChunkType.Ground.ToString());
				}
				else
				{
					_animator.SetTrigger(gadgetChunkInfo.ChunkType.ToString());
				}
			}
		}
	}

	private void DisactiveAnimationHandler(ChunkType chunkType)
	{
		_activeGadget?.SetActive(false);

		if (chunkType != ChunkType.Finish)
		{
			_animator.SetTrigger(chunkType.ToString());
		}
	}
}
