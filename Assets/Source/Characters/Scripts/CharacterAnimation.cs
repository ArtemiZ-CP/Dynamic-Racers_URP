using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterAnimation : MonoBehaviour
{
	private readonly int SpeedMultiplier = Animator.StringToHash(nameof(SpeedMultiplier));

	[SerializeField] private Animator _animator;
	
	private CharacterMovement _characterMovement;

	private void OnEnable()
	{
		_characterMovement.OnChangeChunkType += OnChangeChunkHandler;
	}

	private void OnDisable()
	{
		_characterMovement.OnChangeChunkType -= OnChangeChunkHandler;
	}

	private void Awake()
	{
		_characterMovement = GetComponent<CharacterMovement>();
	}

	private void Update()
	{
		_animator.SetFloat(SpeedMultiplier, _characterMovement.Speed);
	}

	private void OnChangeChunkHandler(ChunkType chunkType)
	{
		if (chunkType == ChunkType.Start)
		{
			chunkType = ChunkType.Ground;
		}

		_animator.SetTrigger(chunkType.ToString());
	}
}
