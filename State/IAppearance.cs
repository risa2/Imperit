using System.Collections.Generic;

namespace Imperit.State
{
	public interface IAppearance : IEnumerable<Point>
	{
		Color Fill => new Color();
		Color Stroke => new Color();
		int StrokeWidth => 0;
	}
}
