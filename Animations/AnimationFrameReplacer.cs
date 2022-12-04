using System.Collections.Generic;

namespace terraguardians
{
    public class AnimationFrameReplacer
    {
        private short DefaultFrame = -1;
        private List<FrameReplacer> Frames = new List<FrameReplacer>();

        public AnimationFrameReplacer(short DefaultFrame = -1)
        {
            this.DefaultFrame = DefaultFrame;
        }

        public void AddFrameToReplace(short FrameID, short NewFrameID)
        {
            Frames.Add(new FrameReplacer(){ FrameID = FrameID, NewFrameID = NewFrameID });
        }

        public short GetFrameID(short FrameID)
        {
            foreach(FrameReplacer f in Frames)
            {
                if (f.FrameID == FrameID) return f.NewFrameID;
            }
            return DefaultFrame;
        }
    }
}