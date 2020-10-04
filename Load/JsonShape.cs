using Imperit.State;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
	public class JsonShape : IConvertibleToWith<Shape, bool>
	{
		public IEnumerable<JsonPoint>? Border { get; set; }
		public JsonPoint? Center { get; set; }
		public Shape Convert(int _i, bool _b)
		{
			return new Shape(Border!.Select(pt => pt.Convert()).ToArray(), Center!.Convert());
		}
	}
}