using Marten.Schema;

namespace Catalog.API.Data
{
    public class DatalogInitialData : IInitialData

    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<Product>().AnyAsync(cancellation))
            {
                return;
            }

            session.Store<Product>(GetPreConfiguredProduct());

            await session.SaveChangesAsync();
        }

        private static IEnumerable<Product> GetPreConfiguredProduct() => new List<Product>
            {
                new Product
                {
                    Id = new  Guid("019cc3db-ff45-48a1-82e2-bd23c02d4cc2"),
                    Name = "IPhone 12",
                    Description = "IPhone 12 with 64GB Memory",
                    ImageFile = "product-1.png",
                    Price = 799,
                    Categories = new List<string> { "Smart Phone", "Electronics" }
                },
                new Product
                {
                    Id = new Guid("019cc3db-ff45-48a1-82e2-bd23c02d4cc3"),
                    Name = "Samsung Galaxy S21",
                    Description = "Samsung Galaxy S21 with 128GB Memory",
                    ImageFile = "product-2.png",
                    Price = 999,
                    Categories = new List<string> { "Smart Phone", "Electronics" }
                },
                new Product
                {
                    Id = new Guid("019cc3db-ff45-48a1-82e2-bd23c02d4cc4"),
                    Name = "MacBook Pro",
                    Description = "MacBook Pro with M1 Chip",
                    ImageFile = "product-3.png",
                    Price = 1299,
                    Categories = new List<string> { "Laptop", "Electronics" }
                },
                 new Product
                 {
                     Id = new Guid("019cc3db-ff45-48a1-82e2-bd23c02d4cc5"),
                     Name = "Dell XPS 13",
                     Description = "Dell XPS 13 with Intel Core i7",
                     ImageFile = "product-4.png",
                     Price = 1099,
                     Categories = new List<string> { "Laptop", "Electronics" }
                 },
        };
    }


}
