namespace Products.Shared.Interfaces
{
    public interface IServiceLocator
    {
        T Get<T>();
    }
}
