using System.Collections.Generic;

namespace Imperit.State
{
	public class Shape : IEnumerableImpl<Point>
	{
		readonly Point[] border;
		public readonly Point Center;
		public Shape(Point[] points, Point center)
		{
			border = points;
			Center = center;
		}
		public IEnumerator<Point> GetEnumerator() => (border as IEnumerable<Point>).GetEnumerator();
	}
}