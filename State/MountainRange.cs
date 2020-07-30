using System.Collections.Generic;

namespace Imperit.State
{
    public class MountainRange : IAppearance
    {
        readonly Point[] line;
        readonly Settings settings;
        public MountainRange(Point[] ln, Settings set)
        {
            line = ln;
            settings = set;
        }
        public IEnumerator<Point> GetEnumerator() => (line as IEnumerable<Point>).GetEnumerator();
        public Color Stroke => settings.MountainsColor;
    }
}