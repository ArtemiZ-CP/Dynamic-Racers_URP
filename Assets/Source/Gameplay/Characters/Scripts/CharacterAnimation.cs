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
	private GameObject _activeGadget;
	private Animator _gadgetAnimator;
	private int _place = 0;

	private void Awake()
	{
		_animator = _meshSpawner.Initialize();
		_characterMovement = GetComponent<CharacterMovement>();
		_characterGadgets = GetComponent<CharacterGadgets>();
		_runStagesBase = FindObjectOfType<RunStagesBase>();
	}

	private void Start()
	{
		if (_characterGadgets.Gadget != null)
		{
			_activeGadget = Instantiate(_characterGadgets.Gadget.Prefab, _meshSpawner.transform);
			_gadgetAnimator = _activeGadget.GetComponent<Animator>();
			_activeGadget.SetActive(false);
		}
	}

	private void OnEnable()
	{
		_characterGadgets.OnActiveAnimation += OnActiveAnimationHandler;
		_characterGadgets.OnDisactiveAnimation += OnDisactiveAnimationHandler;
	}

	private void OnDisable()
	{
		_characterGadgets.OnActiveAnimation -= OnActiveAnimationHandler;
		_characterGadgets.OnDisactiveAnimation -= OnDisactiveAnimationHandler;
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

	private void OnActiveAnimationHandler(GadgetChunkInfo gadgetChunkInfo, bool isGadgetActive)
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
				// _activeGadget.SetActive(true);
				// _gadgetAnimator.SetTrigger(gadgetChunkInfo.ChunkType.ToString());
				// _animator.SetTrigger(gadgetChunkInfo.AnimationTriggerName);
				_animator.SetTrigger(gadgetChunkInfo.ChunkType.ToString());
				Debug.LogWarning("Temporarily solution");
			}
			else
			{
				_animator.SetTrigger(gadgetChunkInfo.ChunkType.ToString());
			}
		}
	}

	private void OnDisactiveAnimationHandler(ChunkType chunkType)
	{
		_activeGadget.SetActive(false);
		_animator.SetTrigger(chunkType.ToString());
	}
}
