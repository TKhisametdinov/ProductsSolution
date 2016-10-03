namespace Products.Shared.Interfaces
{
    public interface IProductApiUrlProvider : IUrlProvider
    {
        string GetProductUrl(int id);
        string GetProductsUrl();
        string DeleteProductUrl(int id);
        string CurrentProductUrl(int id);
    }
}
