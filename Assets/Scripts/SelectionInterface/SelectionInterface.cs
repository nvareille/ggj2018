using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionInterface : MonoBehaviour
{
    [Header("Interface")]
    public Image[] Selected;

    [Header("Config")]
    public Sprite DefaultSprite;
    public TraitScriptableObject[] Traits;
    public CameraCharacterController Character;

    private List<TraitScriptableObject> SelectedTraits;
    private int Count;

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        Count = 0;
        foreach (Image s in Selected)
        {
            s.sprite = DefaultSprite;
        }
        SelectedTraits = new List<TraitScriptableObject>();
    }

    public void SelectTrait(int id)
    {
        Selected[Count].sprite = Traits[id].Sprite;
        SelectedTraits.Add(Traits[id]);
        ++Count;
    }

    public void Validate()
    {
        HeroStats stats = Character.GetComponent<HeroStats>();

        stats.WipeTraits();
        SelectedTraits.ForEach(stats.WinATrait);
        Character.Spawn();
        gameObject.SetActive(false);
    }
}
