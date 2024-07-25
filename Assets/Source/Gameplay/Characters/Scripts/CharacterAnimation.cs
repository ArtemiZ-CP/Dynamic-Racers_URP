using UnityEngine;

[RequireComponent(typeof(CharacterMovement), typeof(CharacterGadgets))]
public class CharacterAnimation : MonoBehaviour
{
	private readonly int SpeedMultiplier = Animator.StringToHash(nameof(SpeedMultiplier));
	private readonly int Defeat = Animator.StringToHash(nameof(Defeat));
	private readonly int Victory = Animator.StringToHash(nameof(Victory));

	[SerializeField] private MeshSpawner _meshSpawner;
	
	private RunStagesBase _runStagesBase;
	private CharacterGadgets _characterGadgets;
	private CharacterMovement _characterMovement;
	private Animator _animator;
	private Animator _gadgetAnimator;
	private GameObject _activeGadget;
	private int _place = 0;

	private void OnEnable()
	{
		_characterGadgets.OnActiveAnimation += OnActiveAnimationHandler;
	}

	private void OnDisable()
	{
		_characterGadgets.OnActiveAnimation -= OnActiveAnimationHandler;
	}

	private void Awake()
	{
		_animator = _meshSpawner.Initialize();
		_characterMovement = GetComponent<CharacterMovement>();
		_characterGadgets = GetComponent<CharacterGadgets>();
		_runStagesBase = FindObjectOfType<RunStagesBase>();
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

	private void OnActiveAnimationHandler(GadgetAnimationInfo gadgetAnimationInfo)
	{
		if (gadgetAnimationInfo.ChunkType == ChunkType.Finish)
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
			_animator.SetTrigger(gadgetAnimationInfo.AnimationTrigger);
		}

		if (_activeGadget != null)
		{
			Destroy(_activeGadget);
		}

		if (gadgetAnimationInfo.Prefab != null)
		{
			_activeGadget = Instantiate(gadgetAnimationInfo.Prefab, _meshSpawner.transform);
			_gadgetAnimator = _activeGadget.GetComponent<Animator>();
		}
	}
}
