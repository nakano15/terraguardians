using Terraria.Cinematics;

namespace terraguardians
{
    public class FilmExpanded : Film
    {
        public virtual bool HideInterfaces => true;
        public virtual bool DrawInFrontOfInterfaces => true;

        public virtual void DrawOnScreen()
        {

        }
    }
}