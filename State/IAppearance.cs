namespace Imperit.State
{
    public interface IAppearance : IEnumerableImpl<Point>
    {
        Color Fill => new Color();
        Color Stroke => new Color();
    }
}
