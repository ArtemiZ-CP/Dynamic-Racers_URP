using UnityEngine;

[RequireComponent(typeof(CharacterMovement), typeof(CharacterGadgets))]
public class CharacterAnimation : MonoBehaviour
{
	private readonly int SpeedMultiplier = Animator.StringToHash(nameof(SpeedMultiplier));

	[SerializeField] private Transform _mesh;
	
	private CharacterGadgets _characterGadgets;
	private CharacterMovement _characterMovement;
	private Animator _animator;
	private Animator _gadgetAnimator;
	private GameObject _activeGadget;

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
		_animator = _mesh.GetComponentInChildren<Animator>();
		_characterMovement = GetComponent<CharacterMovement>();
		_characterGadgets = GetComponent<CharacterGadgets>();
	}

	private void Update()
	{
		SetAnimationSpeed();
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
		_animator.SetTrigger(gadgetAnimationInfo.AnimationTrigger);

		if (_activeGadget != null)
		{
			Destroy(_activeGadget);
		}

		if (gadgetAnimationInfo.Prefab != null)
		{
			_activeGadget = Instantiate(gadgetAnimationInfo.Prefab, _mesh);
			_gadgetAnimator = _activeGadget.GetComponent<Animator>();
		}
	}
}
