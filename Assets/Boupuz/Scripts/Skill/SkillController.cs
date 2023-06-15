using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SkillController : MonoBehaviour
{
    public static SkillController Instance;
    [SerializeField] private List<Skill> _listSkill;
    [SerializeField] private List<SkillData> _skills = new List<SkillData>();
    [SerializeField] private SkillView _view;

    [Serializable]
    public class SkillData
    {
        [SerializeField] private int _id;
        [SerializeField] private int _level;

        public int Level { get => _level; set => _level = value; }
        public int Id { get => _id; set => _id = value; }
    }

    public List<Skill> ListSkill { get => _listSkill; set => _listSkill = value; }
    public List<SkillData> Skills { get => _skills; set => _skills = value; }

    private void Awake()
    {
        SkillController.Instance = this;
    }

    public List<Skill> RandomSkill(List<Skill> skills)
    {
        List<int> listSkill = new List<int>();
        int idSkill = 0;
        for (int i = 0; i < _listSkill.Count; i++)
        {
            listSkill.Add(i);
        }
        idSkill = UnityEngine.Random.Range(0, listSkill.Count - 1);
        skills.Add(_listSkill[listSkill[idSkill]]);
        listSkill.RemoveAt(idSkill);
        idSkill = UnityEngine.Random.Range(0, listSkill.Count - 1);
        skills.Add(_listSkill[listSkill[idSkill]]);
        listSkill.RemoveAt(idSkill);
        idSkill = UnityEngine.Random.Range(0, listSkill.Count - 1);
        skills.Add(_listSkill[listSkill[idSkill]]);
        return skills;
    }

    public void ShowUISkill()
    {
        _view.gameObject.SetActive(true);
        List<Skill> skills = new List<Skill>();
        skills = RandomSkill(skills);
        Skill skill1 = skills[0];
        Skill skill2 = skills[1];
        Skill skill3 = skills[2];

        bool check1 = false;
        bool check2 = false;
        bool check3 = false;
        for (int i = 0; i < _skills.Count; i++)
        {
            if(_skills[i].Id == skill1.Id)
            {
                check1 = true;
                _view.SetUp(1, skill1.Id, skill1.Image, skill1.Name, _skills[i].Level + 1, _skills[i].Level < 5 ? skill1.Level[_skills[i].Level + 1] : "");
            }
            if (_skills[i].Id == skill2.Id)
            {
                check2 = true;
                _view.SetUp(2, skill2.Id, skill2.Image, skill2.Name, _skills[i].Level + 1, _skills[i].Level < 5 ? skill2.Level[_skills[i].Level + 1] : "");
            }
            if (_skills[i].Id == skill3.Id)
            {
                check3 = true;
                _view.SetUp(3, skill3.Id, skill3.Image, skill3.Name, _skills[i].Level + 1, _skills[i].Level < 5 ? skill3.Level[_skills[i].Level + 1] : "");
            }
        }
        if (!check1)
        {
            _view.SetUp(1, skill1.Id, skill1.Image, skill1.Name, 1, skill1.Level[1]);
        }
        if (!check2)
        {
            _view.SetUp(2, skill2.Id, skill2.Image, skill2.Name, 1, skill2.Level[1]);
        }
        if (!check3)
        {
            _view.SetUp(3, skill3.Id, skill3.Image, skill3.Name, 1, skill3.Level[1]);
        }
    }

    public void UpdateSkill(int id, int level)
    {
        Debug.Log("Skill:" + id + ":" + level);
        SkillData skillData = new SkillData();
        skillData.Id = id;
        skillData.Level = level;
        switch (id)
        {
            case 0: //+Bou
                BallController.Instance.AddNewBall(Int32.Parse(_listSkill[0].Level[level]));
                BallController.Instance.AddBallBySkill += Int32.Parse(_listSkill[0].Level[level]);
                break;
            case 1: //+Atk
                BallController.Instance.AddDamageBySkill = Int32.Parse(_listSkill[1].Level[level]);
                break;
            case 2: //%Freeze
                BallController.Instance.AddFreezeBySkill = Int32.Parse(_listSkill[2].Level[level]);
                break;
            default:
                break;
        }
        for (int i = 0; i < _skills.Count; i++)
        {
            if(_skills[i].Id == id)
            {
                _skills.RemoveAt(i);
            }
        }
        _skills.Add(skillData);
        _view.gameObject.SetActive(false);
        GameFlow.Instance.canShoot = true;
    }
}
