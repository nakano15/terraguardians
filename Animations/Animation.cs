using System.Collections.Generic;

namespace terraguardians
{
    public class Animation
    {
        public List<Frame> GetFrames{ get { return Frames; } }
        private List<Frame> Frames = new List<Frame>();
        private float TotalFrameDuration = 0;
        public float GetTotalFrameDuration { get { return TotalFrameDuration; } }
        public bool HasFrames { get { return Frames.Count > 0; } }

        public Animation()
        {

        }

        public int GetFrame(int FrameID)
        {
            if(FrameID < 0 || FrameID >= Frames.Count)
                return -1;
            return Frames[FrameID].ID;
        }

        public int UpdateFrameAndGetTime(float AnimationIncrement, ref float AnimationTime)
        {
            AnimationTime += AnimationIncrement;
            if(AnimationTime < 0)
                AnimationTime += GetTotalFrameDuration;
            if(AnimationTime >= GetTotalFrameDuration)
                AnimationTime -= GetTotalFrameDuration;
            float Sum = 0;
            for(int i = 0; i < Frames.Count; i++)
            {
                if(AnimationTime >= Sum && AnimationTime < Sum + Frames[i].Duration)
                {
                    return Frames[i].ID;
                }
                Sum += Frames[i].Duration;
            }
            return 0;
        }

        public Animation(short FirstFrameID, float FirstFrameDuration = 1)
        {
            AddFrame(FirstFrameID, FirstFrameDuration);
        }

        public Animation AddFrame(short ID, float Duration)
        {
            TotalFrameDuration = 0;
            foreach(Frame f in Frames) TotalFrameDuration += f.Duration;
            TotalFrameDuration += Duration;
            Frames.Add(new Frame(){ ID = ID , Duration = Duration });
            return this;
        }
    }
}