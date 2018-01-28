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
    public Image[] TraitsImage;
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

        int count = 0;
        foreach (TraitScriptableObject o in Traits)
        {
            DisplayObjects(o, count++);
        }

        SelectedTraits = new List<TraitScriptableObject>();
    }

    public void DisplayObjects(TraitScriptableObject o, int id)
    {
        HeroStats stats = Character.GetComponent<HeroStats>();

        TraitsImage[id].sprite = stats.UnlockedTraits.Contains(o) ? o.Sprite : DefaultSprite;
    }

    public void SelectTrait(int id)
    {
        HeroStats stats = Character.GetComponent<HeroStats>();

        Debug.Log(stats.UnlockedTraits.Count);

        if (stats.UnlockedTraits.Contains(Traits[id]))
        {
            Selected[Count].sprite = Traits[id].Sprite;
            SelectedTraits.Add(Traits[id]);
            ++Count;
        }
    }

    public void Validate()
    {
        HeroStats stats = Character.GetComponent<HeroStats>();

        stats.WipeTraits();
        SelectedTraits.ForEach(stats.AddTrait);
        Character.Spawn();
        gameObject.SetActive(false);
    }
}
