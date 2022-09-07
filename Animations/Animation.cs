using System.Collections.Generic;

namespace terraguardians
{
    public class Animation
    {
        public List<Frame> GetFrames{ get { return Frames; } }
        private List<Frame> Frames = new List<Frame>();
        private float TotalFrameDuration = 0;
        public float GetTotalFrameDuration { get { return TotalFrameDuration; } }
        public bool HasFrames { get { return Frames.Count == 0; } }

        public Animation()
        {

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