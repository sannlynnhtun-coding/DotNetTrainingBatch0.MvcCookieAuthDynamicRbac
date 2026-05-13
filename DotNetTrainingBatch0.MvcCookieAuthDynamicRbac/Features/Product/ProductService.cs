namespace DotNetTrainingBatch0.MvcCookieAuthDynamicRbac.Features.Product;

public class ProductService
{
    private static List<ProductDto> _products = new()
    {
        new ProductDto { Id = 1, Name = "Laptop", Price = 1200.50m, Description = "High-end laptop" },
        new ProductDto { Id = 2, Name = "Mouse", Price = 25.00m, Description = "Wireless mouse" }
    };

    public ProductListResponse GetProducts(ProductListRequest request)
    {
        var products = _products
            .Where(x => string.IsNullOrEmpty(request.SearchTerm) || 
                        x.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new ProductListResponse
        {
            Products = products,
            TotalCount = _products.Count
        };
    }

    public void CreateProduct(ProductCreateRequest request)
    {
        var newProduct = new ProductDto
        {
            Id = _products.Any() ? _products.Max(x => x.Id) + 1 : 1,
            Name = request.Name,
            Price = request.Price,
            Description = request.Description
        };
        _products.Add(newProduct);
    }

    public void UpdateProduct(int id, ProductUpdateRequest request)
    {
        var product = _products.FirstOrDefault(x => x.Id == id);
        if (product != null)
        {
            product.Name = request.Name;
            product.Price = request.Price;
            product.Description = request.Description;
        }
    }

    public void DeleteProduct(int id)
    {
        var product = _products.FirstOrDefault(x => x.Id == id);
        if (product != null)
        {
            _products.Remove(product);
        }
    }
}
