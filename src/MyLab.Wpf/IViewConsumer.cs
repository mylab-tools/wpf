namespace MyLab.Wpf
{
    public interface IViewProvider
    {
        I Provide<I>() where I :class;
    }
}
