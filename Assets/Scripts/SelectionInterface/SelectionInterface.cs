using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    private EventSystem EventSystem;
    private List<TraitScriptableObject> SelectedTraits;
    private int Count;

    public bool FirstTime = true;

    public void Awake()
    {
        EventSystem = FindObjectOfType<EventSystem>();
        Init(true);
    }

    public void Update()
    {
        if (FirstTime)
        {
            FirstTime = false;
            Validate();
        }
    }

    public void Init(bool show = false)
    {
        Count = 0;
        foreach (Image s in Selected)
        {
            s.sprite = DefaultSprite;
        }

        int count = 0;
        if (!FirstTime)
        {
            foreach (TraitScriptableObject o in Traits)
            {
                DisplayObjects(o, count++);
            }
            TraitsImage[0].GetComponent<Button>().Select();
        }
        
        gameObject.SetActive(show);
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
        Character.MayMove = true;
        gameObject.SetActive(false);
    }
}
