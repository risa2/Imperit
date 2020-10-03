using Imperit.State;

namespace Imperit.Load
{
	public class JsonDescription
	{
		public string Name { get; set; } = "";
		public string Symbol { get; set; } = "";
		public string Text { get; set; } = "";
		public Description Convert() => new Description(Name, Symbol, Text);
		public static JsonDescription From(Description d) => new JsonDescription() { Name = d.Name, Symbol = d.Symbol, Text = d.Text };
	}
}
