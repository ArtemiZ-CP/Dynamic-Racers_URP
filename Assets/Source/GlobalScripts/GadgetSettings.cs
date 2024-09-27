using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GadgetSettings", menuName = "GlobalSettings/GadgetSettings", order = 1)]
public class GadgetSettings : ScriptableObject
{
	private static GadgetSettings _instance;
    
	[Serializable]
	private class GadgetsLevelProgression
	{
		[SerializeField] private int _gadgetsToLevelUp;
		[SerializeField] private int _coinsCost;

		public int GadgetsToLevelUp => _gadgetsToLevelUp;
		public int CoinsCost => _coinsCost;
	}

	[SerializeField] private GadgetsLevelProgression[] _gadgetsLevelProgression;
	[SerializeField] private int[] _gadgetsLevelProgressionCostMultiplier;
	[SerializeField] private int[] _gadgetsLevelProgressionStartLevel;
	[SerializeField] private GadgetScriptableObject[] _allGadgets;
	[SerializeField] private Color[] _gadgetRareColor;
	[SerializeField] private Sprite[] _gadgetRareBackgrounds;
	[SerializeField] private Sprite[] _gadgetMisteryRareBackgrounds;

	public static GadgetSettings Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Resources.Load<GadgetSettings>(nameof(GadgetSettings));
			}

			return _instance;
		}
	}

	public GadgetScriptableObject GetGadgetByName(string name)
	{
		return _allGadgets.FirstOrDefault(gadget => gadget.Name == name);
	}

	public List<Gadget> GetAllGadgets()
	{
		return _allGadgets.Select(gadget => new Gadget(gadget)).ToList();
	}

	public List<Gadget> GetNotFoundGadgets()
	{
		List<Gadget> foundGadgets = PlayerData.PlayerGadgets.ToList();
		List<Gadget> notFoundGadgets = _allGadgets.Where(gadget =>
			!foundGadgets.Any(foundGadget => foundGadget.ScriptableObject == gadget)).
			Select(gadget => new Gadget(gadget)).ToList();

		return notFoundGadgets;
	}

	public Sprite GetGadgetRareBackground(Rare rare)
	{
		return _gadgetRareBackgrounds[(int)rare - (int)Rare.Common];
	}

	public Sprite GetGadgetMisteryRareBackground(Rare rare)
	{
		return _gadgetMisteryRareBackgrounds[(int)rare - (int)Rare.Common];
	}

	public Color GetGadgetRareColor(Rare rare)
	{
		return _gadgetRareColor[(int)rare - (int)Rare.Common];
	}

	public GadgetScriptableObject GetRandomGadget()
	{
		return _allGadgets[UnityEngine.Random.Range(0, _allGadgets.Length)];
	}

	public GadgetScriptableObject GetRandomGadget(Rare rare)
	{
		List<GadgetScriptableObject> gadgets = _allGadgets.Where(g =>
			g.Rare == rare).ToList();

		if (gadgets.Count == 0)
		{
			return null;
		}

		return gadgets[UnityEngine.Random.Range(0, gadgets.Count)];
	}

	public GadgetScriptableObject GetRandomGadget(Rare[] rares)
	{
		List<GadgetScriptableObject> gadgets = _allGadgets.Where(g =>
			rares.Any(r => r == g.Rare)).ToList();

		if (gadgets.Count == 0)
		{
			return null;
		}

		return gadgets[UnityEngine.Random.Range(0, gadgets.Count)];
	}

	public GadgetScriptableObject GetRandomGadget(Rare rare, System.Random random)
	{
		List<GadgetScriptableObject> gadgets = _allGadgets.Where(g =>
			g.Rare == rare).ToList();

		if (gadgets.Count == 0)
		{
			return null;
		}

		return gadgets[random.Next(gadgets.Count)];
	}

	public int GetStartGadgetLevel(Gadget gadget)
	{
		return _gadgetsLevelProgressionStartLevel[(int)gadget.ScriptableObject.Rare - (int)Rare.Common] - 1;
	}

	public bool TryGetGadgetsLevelProgression(Gadget gadget, out int gadgetsToLevelUp, out int coinsCost)
	{
		gadgetsToLevelUp = 0;
		coinsCost = 0;
		
        if (PlayerData.PlayerGadgets.Contains(gadget) == false)
        {
            return false;
        }

		if (gadget.Level < _gadgetsLevelProgression.Length)
		{
			int startLevel = GetStartGadgetLevel(gadget);
			int rareGadgetCostMultiplier = _gadgetsLevelProgressionCostMultiplier[(int)gadget.ScriptableObject.Rare - (int)Rare.Common];

			gadgetsToLevelUp = _gadgetsLevelProgression[gadget.Level - startLevel].GadgetsToLevelUp;
			coinsCost = _gadgetsLevelProgression[gadget.Level - startLevel].CoinsCost * rareGadgetCostMultiplier;

			return true;
		}


		return false;
	}
}
