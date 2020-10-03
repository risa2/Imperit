using Imperit.State;

namespace Imperit.Load
{
	public class JsonPoint
	{
		public double X { get; set; }
		public double Y { get; set; }
		public Point Convert() => new Point(X, Y);
	}
}