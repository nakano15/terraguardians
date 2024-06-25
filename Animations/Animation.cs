using System.Collections.Generic;

namespace terraguardians
{
    public class Animation
    {
        public List<Frame> GetFrames{ get { return Frames; } }
        private List<Frame> Frames = new List<Frame>();
        private float TotalAnimationDuration = 0;
        public float GetTotalAnimationDuration { get { return TotalAnimationDuration; } }
        public bool HasFrames { get { return Frames.Count > 0; } }
        public int GetFrameCount { get { return Frames.Count; } }

        public Animation()
        {
            
        }

        public short GetFrame(short FrameID)
        {
            if(FrameID < 0 || FrameID >= Frames.Count)
                return -1;
            return Frames[FrameID].ID;
        }

        public short UpdateTimeAndGetFrame(float AnimationIncrement, ref float AnimationTime)
        {
            AnimationTime += AnimationIncrement;
            if(AnimationTime < 0)
                AnimationTime += GetTotalAnimationDuration;
            if(AnimationTime >= GetTotalAnimationDuration)
                AnimationTime -= GetTotalAnimationDuration;
            return GetFrameFromTime(AnimationTime);
        }

        public short GetFrameFromTime(float AnimationTime)
        {
            if(!HasFrames) return 0;
            float Sum = 0;
            for(int i = 0; i < Frames.Count; i++)
            {
                if(AnimationTime >= Sum && AnimationTime < Sum + Frames[i].Duration)
                {
                    return Frames[i].ID;
                }
                Sum += Frames[i].Duration;
            }
            return AnimationTime >= GetTotalAnimationDuration ? Frames[Frames.Count - 1].ID : Frames[0].ID;
        }

        public short GetFrameFromPercentage(float Percentage)
        {
            float AimedTime = TotalAnimationDuration * Percentage;
            if(!HasFrames) return 0;
            return GetFrameFromTime(AimedTime);
        }

        public Animation(short FirstFrameID, float FirstFrameDuration = 1)
        {
            AddFrame(FirstFrameID, FirstFrameDuration);
        }

        public Animation AddFrame(short ID, float Duration = 1)
        {
            TotalAnimationDuration = 0;
            foreach(Frame f in Frames) TotalAnimationDuration += f.Duration;
            TotalAnimationDuration += Duration;
            Frames.Add(new Frame(){ ID = ID , Duration = Duration });
            return this;
        }
    }
}