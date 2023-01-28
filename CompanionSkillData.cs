using Terraria;
using System;
using System.IO;

namespace terraguardians
{
    public class CompanionSkillData
    {
        private CompanionSkillBase _Base;
        private uint SkillID = 0;
        private string SkillModID = "";
        public uint GetID { get { return SkillID; }}
        public string GetModID { get { return SkillModID; } }
        public int Level = 0;
        public float Progress = 0, MaxProgress = 0;
        private float Power = 0;
        public bool IsValidSkill{ get { return _Base.IsValidSkill; } }

        public CompanionSkillData()
        {

        }

        public CompanionSkillData(uint ID, string ModID)
        {
            this.SkillID = ID;
            this.SkillModID = ModID;
            _Base = CompanionSkillContainer.GetSkillBase(ID, ModID);
            UpdateMaxProgress();
            UpdatePower();
        }

        public string GetSkillInfo
        {
            get
            {
                string Text = _Base.Name + " Lv: " + Level + " (" + System.Math.Round(Progress * 100 / MaxProgress) + "%)";
                return Text;
            }
        }

        public string GetDescription
        {
            get
            {
                return _Base.Description;
            }
        }

        public void AddProgress(float ExtraProgress)
        {
            Progress += ExtraProgress;
            while (Progress >= MaxProgress)
            {
                Level ++;
                Progress -= MaxProgress;
                UpdateMaxProgress();
                UpdatePower();
            }
        }

        public void UpdateStatus(Companion companion)
        {
            _Base.UpdateStatus(companion, Level, Power);
        }

        public void UpdatePower()
        {
            Power = (float)Math.Log(Math.Max(0, Level) + 1);
        }

        public void UpdateMaxProgress()
        {
            MaxProgress = _Base.GetMaxSkillProgressForLevel(Level);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(SkillID);
            writer.Write(SkillModID);
            writer.Write(Level);
            writer.Write(Progress);
        }

        public void Load(BinaryReader reader, uint LastVersion)
        {
            SkillID = reader.ReadUInt32();
            SkillModID = reader.ReadString();
            _Base = CompanionSkillContainer.GetSkillBase(SkillID, SkillModID);
            Level = reader.ReadInt32();
            Progress = reader.ReadSingle();
            UpdatePower();
            UpdateMaxProgress();
        }
    }
}