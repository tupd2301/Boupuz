using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static SkillController;

public class SkillView : MonoBehaviour
{
    [SerializeField] private List<Image> _skillGraphics;
    [SerializeField] private List<Image> _skillGraphicParents;
    [SerializeField] private List<Text> _skillNames;
    [SerializeField] private List<Text> _skillLevels;
    [SerializeField] private List<Text> _skillEffects;

    [SerializeField] private int _skillChosing;

    [SerializeField] private List<int> _idSlots = new List<int>();
    [SerializeField] private List<int> _levelSlots = new List<int>();

    public void Submit()
    {
        if (_skillChosing != 0)
        {
            SkillController.Instance.UpdateSkill(_idSlots[_skillChosing - 1],_levelSlots[_skillChosing-1]);
            _idSlots = new List<int>();
            _levelSlots = new List<int>();
        }
    }

    public void ChooseSkill(int slot)
    {
        _skillChosing = slot;
        for (int i = 1; i < _skillGraphicParents.Count; i++)
        {
            if(i == slot)
            {
                _skillGraphicParents[i].color = new Color32(255, 200, 0, 255);
            }
            else
            {
                _skillGraphicParents[i].color = new Color32((byte)_skillGraphicParents[i].color.r, (byte)_skillGraphicParents[i].color.g, (byte)_skillGraphicParents[i].color.b, 0);
            }
        }
    }

    public void SetUp(int slot, int id, Sprite sprite, string name, int level, string effect)
    {
        string textLevel = "Lv." + level;
        if (level == 5)
        {
            textLevel = "Lv.Max";
        }
        _skillGraphics[slot].sprite = sprite;
        _skillNames[slot].text = name;
        _skillLevels[slot].text = "Lv." + level;
        _skillEffects[slot].text = effect;
        _idSlots.Add(id);
        _levelSlots.Add(level);
    }
}
