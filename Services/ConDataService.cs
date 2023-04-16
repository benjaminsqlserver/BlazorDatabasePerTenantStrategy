using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using BikeStores.Data;

namespace BikeStores
{
    public partial class ConDataService
    {
        ConDataContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly ConDataContext context;
        private readonly NavigationManager navigationManager;

        public ConDataService(ConDataContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);


        public async Task ExportBrandsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/brands/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/brands/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBrandsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/brands/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/brands/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBrandsRead(ref IQueryable<BikeStores.Models.ConData.Brand> items);

        public async Task<IQueryable<BikeStores.Models.ConData.Brand>> GetBrands(Query query = null)
        {
            var items = Context.Brands.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnBrandsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBrandGet(BikeStores.Models.ConData.Brand item);

        public async Task<BikeStores.Models.ConData.Brand> GetBrandByBrandId(int brandid)
        {
            var items = Context.Brands
                              .AsNoTracking()
                              .Where(i => i.brand_id == brandid);

  
            var itemToReturn = items.FirstOrDefault();

            OnBrandGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBrandCreated(BikeStores.Models.ConData.Brand item);
        partial void OnAfterBrandCreated(BikeStores.Models.ConData.Brand item);

        public async Task<BikeStores.Models.ConData.Brand> CreateBrand(BikeStores.Models.ConData.Brand brand)
        {
            OnBrandCreated(brand);

            var existingItem = Context.Brands
                              .Where(i => i.brand_id == brand.brand_id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Brands.Add(brand);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(brand).State = EntityState.Detached;
                throw;
            }

            OnAfterBrandCreated(brand);

            return brand;
        }

        public async Task<BikeStores.Models.ConData.Brand> CancelBrandChanges(BikeStores.Models.ConData.Brand item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBrandUpdated(BikeStores.Models.ConData.Brand item);
        partial void OnAfterBrandUpdated(BikeStores.Models.ConData.Brand item);

        public async Task<BikeStores.Models.ConData.Brand> UpdateBrand(int brandid, BikeStores.Models.ConData.Brand brand)
        {
            OnBrandUpdated(brand);

            var itemToUpdate = Context.Brands
                              .Where(i => i.brand_id == brand.brand_id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(brand);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterBrandUpdated(brand);

            return brand;
        }

        partial void OnBrandDeleted(BikeStores.Models.ConData.Brand item);
        partial void OnAfterBrandDeleted(BikeStores.Models.ConData.Brand item);

        public async Task<BikeStores.Models.ConData.Brand> DeleteBrand(int brandid)
        {
            var itemToDelete = Context.Brands
                              .Where(i => i.brand_id == brandid)
                              .Include(i => i.Products)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBrandDeleted(itemToDelete);


            Context.Brands.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBrandDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCategoriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCategoriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCategoriesRead(ref IQueryable<BikeStores.Models.ConData.Category> items);

        public async Task<IQueryable<BikeStores.Models.ConData.Category>> GetCategories(Query query = null)
        {
            var items = Context.Categories.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnCategoriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCategoryGet(BikeStores.Models.ConData.Category item);

        public async Task<BikeStores.Models.ConData.Category> GetCategoryByCategoryId(int categoryid)
        {
            var items = Context.Categories
                              .AsNoTracking()
                              .Where(i => i.category_id == categoryid);

  
            var itemToReturn = items.FirstOrDefault();

            OnCategoryGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCategoryCreated(BikeStores.Models.ConData.Category item);
        partial void OnAfterCategoryCreated(BikeStores.Models.ConData.Category item);

        public async Task<BikeStores.Models.ConData.Category> CreateCategory(BikeStores.Models.ConData.Category category)
        {
            OnCategoryCreated(category);

            var existingItem = Context.Categories
                              .Where(i => i.category_id == category.category_id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Categories.Add(category);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(category).State = EntityState.Detached;
                throw;
            }

            OnAfterCategoryCreated(category);

            return category;
        }

        public async Task<BikeStores.Models.ConData.Category> CancelCategoryChanges(BikeStores.Models.ConData.Category item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCategoryUpdated(BikeStores.Models.ConData.Category item);
        partial void OnAfterCategoryUpdated(BikeStores.Models.ConData.Category item);

        public async Task<BikeStores.Models.ConData.Category> UpdateCategory(int categoryid, BikeStores.Models.ConData.Category category)
        {
            OnCategoryUpdated(category);

            var itemToUpdate = Context.Categories
                              .Where(i => i.category_id == category.category_id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(category);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCategoryUpdated(category);

            return category;
        }

        partial void OnCategoryDeleted(BikeStores.Models.ConData.Category item);
        partial void OnAfterCategoryDeleted(BikeStores.Models.ConData.Category item);

        public async Task<BikeStores.Models.ConData.Category> DeleteCategory(int categoryid)
        {
            var itemToDelete = Context.Categories
                              .Where(i => i.category_id == categoryid)
                              .Include(i => i.Products)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCategoryDeleted(itemToDelete);


            Context.Categories.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCategoryDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportProductsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportProductsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnProductsRead(ref IQueryable<BikeStores.Models.ConData.Product> items);

        public async Task<IQueryable<BikeStores.Models.ConData.Product>> GetProducts(Query query = null)
        {
            var items = Context.Products.AsQueryable();

            items = items.Include(i => i.Brand);
            items = items.Include(i => i.Category);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnProductsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProductGet(BikeStores.Models.ConData.Product item);

        public async Task<BikeStores.Models.ConData.Product> GetProductByProductId(int productid)
        {
            var items = Context.Products
                              .AsNoTracking()
                              .Where(i => i.product_id == productid);

            items = items.Include(i => i.Brand);
            items = items.Include(i => i.Category);
  
            var itemToReturn = items.FirstOrDefault();

            OnProductGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnProductCreated(BikeStores.Models.ConData.Product item);
        partial void OnAfterProductCreated(BikeStores.Models.ConData.Product item);

        public async Task<BikeStores.Models.ConData.Product> CreateProduct(BikeStores.Models.ConData.Product product)
        {
            OnProductCreated(product);

            var existingItem = Context.Products
                              .Where(i => i.product_id == product.product_id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Products.Add(product);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(product).State = EntityState.Detached;
                throw;
            }

            OnAfterProductCreated(product);

            return product;
        }

        public async Task<BikeStores.Models.ConData.Product> CancelProductChanges(BikeStores.Models.ConData.Product item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnProductUpdated(BikeStores.Models.ConData.Product item);
        partial void OnAfterProductUpdated(BikeStores.Models.ConData.Product item);

        public async Task<BikeStores.Models.ConData.Product> UpdateProduct(int productid, BikeStores.Models.ConData.Product product)
        {
            OnProductUpdated(product);

            var itemToUpdate = Context.Products
                              .Where(i => i.product_id == product.product_id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(product);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterProductUpdated(product);

            return product;
        }

        partial void OnProductDeleted(BikeStores.Models.ConData.Product item);
        partial void OnAfterProductDeleted(BikeStores.Models.ConData.Product item);

        public async Task<BikeStores.Models.ConData.Product> DeleteProduct(int productid)
        {
            var itemToDelete = Context.Products
                              .Where(i => i.product_id == productid)
                              .Include(i => i.Stocks)
                              .Include(i => i.OrderItems)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnProductDeleted(itemToDelete);


            Context.Products.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterProductDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStocksToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/stocks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/stocks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStocksToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/stocks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/stocks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStocksRead(ref IQueryable<BikeStores.Models.ConData.Stock> items);

        public async Task<IQueryable<BikeStores.Models.ConData.Stock>> GetStocks(Query query = null)
        {
            var items = Context.Stocks.AsQueryable();

            items = items.Include(i => i.Product);
            items = items.Include(i => i.Store);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnStocksRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStockGet(BikeStores.Models.ConData.Stock item);

        public async Task<BikeStores.Models.ConData.Stock> GetStockByStoreIdAndProductId(int storeid, int productid)
        {
            var items = Context.Stocks
                              .AsNoTracking()
                              .Where(i => i.store_id == storeid && i.product_id == productid);

            items = items.Include(i => i.Product);
            items = items.Include(i => i.Store);
  
            var itemToReturn = items.FirstOrDefault();

            OnStockGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStockCreated(BikeStores.Models.ConData.Stock item);
        partial void OnAfterStockCreated(BikeStores.Models.ConData.Stock item);

        public async Task<BikeStores.Models.ConData.Stock> CreateStock(BikeStores.Models.ConData.Stock stock)
        {
            OnStockCreated(stock);

            var existingItem = Context.Stocks
                              .Where(i => i.store_id == stock.store_id && i.product_id == stock.product_id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Stocks.Add(stock);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stock).State = EntityState.Detached;
                throw;
            }

            OnAfterStockCreated(stock);

            return stock;
        }

        public async Task<BikeStores.Models.ConData.Stock> CancelStockChanges(BikeStores.Models.ConData.Stock item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStockUpdated(BikeStores.Models.ConData.Stock item);
        partial void OnAfterStockUpdated(BikeStores.Models.ConData.Stock item);

        public async Task<BikeStores.Models.ConData.Stock> UpdateStock(int storeid, int productid, BikeStores.Models.ConData.Stock stock)
        {
            OnStockUpdated(stock);

            var itemToUpdate = Context.Stocks
                              .Where(i => i.store_id == stock.store_id && i.product_id == stock.product_id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stock);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStockUpdated(stock);

            return stock;
        }

        partial void OnStockDeleted(BikeStores.Models.ConData.Stock item);
        partial void OnAfterStockDeleted(BikeStores.Models.ConData.Stock item);

        public async Task<BikeStores.Models.ConData.Stock> DeleteStock(int storeid, int productid)
        {
            var itemToDelete = Context.Stocks
                              .Where(i => i.store_id == storeid && i.product_id == productid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStockDeleted(itemToDelete);


            Context.Stocks.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStockDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCustomersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/customers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/customers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCustomersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/customers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/customers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCustomersRead(ref IQueryable<BikeStores.Models.ConData.Customer> items);

        public async Task<IQueryable<BikeStores.Models.ConData.Customer>> GetCustomers(Query query = null)
        {
            var items = Context.Customers.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnCustomersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCustomerGet(BikeStores.Models.ConData.Customer item);

        public async Task<BikeStores.Models.ConData.Customer> GetCustomerByCustomerId(int customerid)
        {
            var items = Context.Customers
                              .AsNoTracking()
                              .Where(i => i.customer_id == customerid);

  
            var itemToReturn = items.FirstOrDefault();

            OnCustomerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCustomerCreated(BikeStores.Models.ConData.Customer item);
        partial void OnAfterCustomerCreated(BikeStores.Models.ConData.Customer item);

        public async Task<BikeStores.Models.ConData.Customer> CreateCustomer(BikeStores.Models.ConData.Customer customer)
        {
            OnCustomerCreated(customer);

            var existingItem = Context.Customers
                              .Where(i => i.customer_id == customer.customer_id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Customers.Add(customer);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(customer).State = EntityState.Detached;
                throw;
            }

            OnAfterCustomerCreated(customer);

            return customer;
        }

        public async Task<BikeStores.Models.ConData.Customer> CancelCustomerChanges(BikeStores.Models.ConData.Customer item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCustomerUpdated(BikeStores.Models.ConData.Customer item);
        partial void OnAfterCustomerUpdated(BikeStores.Models.ConData.Customer item);

        public async Task<BikeStores.Models.ConData.Customer> UpdateCustomer(int customerid, BikeStores.Models.ConData.Customer customer)
        {
            OnCustomerUpdated(customer);

            var itemToUpdate = Context.Customers
                              .Where(i => i.customer_id == customer.customer_id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(customer);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCustomerUpdated(customer);

            return customer;
        }

        partial void OnCustomerDeleted(BikeStores.Models.ConData.Customer item);
        partial void OnAfterCustomerDeleted(BikeStores.Models.ConData.Customer item);

        public async Task<BikeStores.Models.ConData.Customer> DeleteCustomer(int customerid)
        {
            var itemToDelete = Context.Customers
                              .Where(i => i.customer_id == customerid)
                              .Include(i => i.Orders)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCustomerDeleted(itemToDelete);


            Context.Customers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCustomerDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportOrderItemsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/orderitems/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/orderitems/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportOrderItemsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/orderitems/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/orderitems/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnOrderItemsRead(ref IQueryable<BikeStores.Models.ConData.OrderItem> items);

        public async Task<IQueryable<BikeStores.Models.ConData.OrderItem>> GetOrderItems(Query query = null)
        {
            var items = Context.OrderItems.AsQueryable();

            items = items.Include(i => i.Order);
            items = items.Include(i => i.Product);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnOrderItemsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnOrderItemGet(BikeStores.Models.ConData.OrderItem item);

        public async Task<BikeStores.Models.ConData.OrderItem> GetOrderItemByOrderIdAndItemId(int orderid, int itemid)
        {
            var items = Context.OrderItems
                              .AsNoTracking()
                              .Where(i => i.order_id == orderid && i.item_id == itemid);

            items = items.Include(i => i.Order);
            items = items.Include(i => i.Product);
  
            var itemToReturn = items.FirstOrDefault();

            OnOrderItemGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnOrderItemCreated(BikeStores.Models.ConData.OrderItem item);
        partial void OnAfterOrderItemCreated(BikeStores.Models.ConData.OrderItem item);

        public async Task<BikeStores.Models.ConData.OrderItem> CreateOrderItem(BikeStores.Models.ConData.OrderItem orderitem)
        {
            OnOrderItemCreated(orderitem);

            var existingItem = Context.OrderItems
                              .Where(i => i.order_id == orderitem.order_id && i.item_id == orderitem.item_id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.OrderItems.Add(orderitem);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(orderitem).State = EntityState.Detached;
                throw;
            }

            OnAfterOrderItemCreated(orderitem);

            return orderitem;
        }

        public async Task<BikeStores.Models.ConData.OrderItem> CancelOrderItemChanges(BikeStores.Models.ConData.OrderItem item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnOrderItemUpdated(BikeStores.Models.ConData.OrderItem item);
        partial void OnAfterOrderItemUpdated(BikeStores.Models.ConData.OrderItem item);

        public async Task<BikeStores.Models.ConData.OrderItem> UpdateOrderItem(int orderid, int itemid, BikeStores.Models.ConData.OrderItem orderitem)
        {
            OnOrderItemUpdated(orderitem);

            var itemToUpdate = Context.OrderItems
                              .Where(i => i.order_id == orderitem.order_id && i.item_id == orderitem.item_id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(orderitem);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterOrderItemUpdated(orderitem);

            return orderitem;
        }

        partial void OnOrderItemDeleted(BikeStores.Models.ConData.OrderItem item);
        partial void OnAfterOrderItemDeleted(BikeStores.Models.ConData.OrderItem item);

        public async Task<BikeStores.Models.ConData.OrderItem> DeleteOrderItem(int orderid, int itemid)
        {
            var itemToDelete = Context.OrderItems
                              .Where(i => i.order_id == orderid && i.item_id == itemid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnOrderItemDeleted(itemToDelete);


            Context.OrderItems.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterOrderItemDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportOrdersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/orders/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/orders/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportOrdersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/orders/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/orders/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnOrdersRead(ref IQueryable<BikeStores.Models.ConData.Order> items);

        public async Task<IQueryable<BikeStores.Models.ConData.Order>> GetOrders(Query query = null)
        {
            var items = Context.Orders.AsQueryable();

            items = items.Include(i => i.Customer);
            items = items.Include(i => i.Staff);
            items = items.Include(i => i.Store);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnOrdersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnOrderGet(BikeStores.Models.ConData.Order item);

        public async Task<BikeStores.Models.ConData.Order> GetOrderByOrderId(int orderid)
        {
            var items = Context.Orders
                              .AsNoTracking()
                              .Where(i => i.order_id == orderid);

            items = items.Include(i => i.Customer);
            items = items.Include(i => i.Staff);
            items = items.Include(i => i.Store);
  
            var itemToReturn = items.FirstOrDefault();

            OnOrderGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnOrderCreated(BikeStores.Models.ConData.Order item);
        partial void OnAfterOrderCreated(BikeStores.Models.ConData.Order item);

        public async Task<BikeStores.Models.ConData.Order> CreateOrder(BikeStores.Models.ConData.Order order)
        {
            OnOrderCreated(order);

            var existingItem = Context.Orders
                              .Where(i => i.order_id == order.order_id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Orders.Add(order);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(order).State = EntityState.Detached;
                throw;
            }

            OnAfterOrderCreated(order);

            return order;
        }

        public async Task<BikeStores.Models.ConData.Order> CancelOrderChanges(BikeStores.Models.ConData.Order item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnOrderUpdated(BikeStores.Models.ConData.Order item);
        partial void OnAfterOrderUpdated(BikeStores.Models.ConData.Order item);

        public async Task<BikeStores.Models.ConData.Order> UpdateOrder(int orderid, BikeStores.Models.ConData.Order order)
        {
            OnOrderUpdated(order);

            var itemToUpdate = Context.Orders
                              .Where(i => i.order_id == order.order_id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(order);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterOrderUpdated(order);

            return order;
        }

        partial void OnOrderDeleted(BikeStores.Models.ConData.Order item);
        partial void OnAfterOrderDeleted(BikeStores.Models.ConData.Order item);

        public async Task<BikeStores.Models.ConData.Order> DeleteOrder(int orderid)
        {
            var itemToDelete = Context.Orders
                              .Where(i => i.order_id == orderid)
                              .Include(i => i.OrderItems)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnOrderDeleted(itemToDelete);


            Context.Orders.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterOrderDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStaffToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/staff/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/staff/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStaffToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/staff/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/staff/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStaffRead(ref IQueryable<BikeStores.Models.ConData.Staff> items);

        public async Task<IQueryable<BikeStores.Models.ConData.Staff>> GetStaff(Query query = null)
        {
            var items = Context.Staff.AsQueryable();

            items = items.Include(i => i.Staff1);
            items = items.Include(i => i.Store);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnStaffRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStaffGet(BikeStores.Models.ConData.Staff item);

        public async Task<BikeStores.Models.ConData.Staff> GetStaffByStaffId(int staffid)
        {
            var items = Context.Staff
                              .AsNoTracking()
                              .Where(i => i.staff_id == staffid);

            items = items.Include(i => i.Staff1);
            items = items.Include(i => i.Store);
  
            var itemToReturn = items.FirstOrDefault();

            OnStaffGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStaffCreated(BikeStores.Models.ConData.Staff item);
        partial void OnAfterStaffCreated(BikeStores.Models.ConData.Staff item);

        public async Task<BikeStores.Models.ConData.Staff> CreateStaff(BikeStores.Models.ConData.Staff staff)
        {
            OnStaffCreated(staff);

            var existingItem = Context.Staff
                              .Where(i => i.staff_id == staff.staff_id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Staff.Add(staff);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(staff).State = EntityState.Detached;
                throw;
            }

            OnAfterStaffCreated(staff);

            return staff;
        }

        public async Task<BikeStores.Models.ConData.Staff> CancelStaffChanges(BikeStores.Models.ConData.Staff item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStaffUpdated(BikeStores.Models.ConData.Staff item);
        partial void OnAfterStaffUpdated(BikeStores.Models.ConData.Staff item);

        public async Task<BikeStores.Models.ConData.Staff> UpdateStaff(int staffid, BikeStores.Models.ConData.Staff staff)
        {
            OnStaffUpdated(staff);

            var itemToUpdate = Context.Staff
                              .Where(i => i.staff_id == staff.staff_id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(staff);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStaffUpdated(staff);

            return staff;
        }

        partial void OnStaffDeleted(BikeStores.Models.ConData.Staff item);
        partial void OnAfterStaffDeleted(BikeStores.Models.ConData.Staff item);

        public async Task<BikeStores.Models.ConData.Staff> DeleteStaff(int staffid)
        {
            var itemToDelete = Context.Staff
                              .Where(i => i.staff_id == staffid)
                              .Include(i => i.Orders)
                              .Include(i => i.Staff2)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStaffDeleted(itemToDelete);


            Context.Staff.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStaffDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStoresToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/stores/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/stores/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStoresToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/stores/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/stores/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStoresRead(ref IQueryable<BikeStores.Models.ConData.Store> items);

        public async Task<IQueryable<BikeStores.Models.ConData.Store>> GetStores(Query query = null)
        {
            var items = Context.Stores.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnStoresRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStoreGet(BikeStores.Models.ConData.Store item);

        public async Task<BikeStores.Models.ConData.Store> GetStoreByStoreId(int storeid)
        {
            var items = Context.Stores
                              .AsNoTracking()
                              .Where(i => i.store_id == storeid);

  
            var itemToReturn = items.FirstOrDefault();

            OnStoreGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStoreCreated(BikeStores.Models.ConData.Store item);
        partial void OnAfterStoreCreated(BikeStores.Models.ConData.Store item);

        public async Task<BikeStores.Models.ConData.Store> CreateStore(BikeStores.Models.ConData.Store store)
        {
            OnStoreCreated(store);

            var existingItem = Context.Stores
                              .Where(i => i.store_id == store.store_id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Stores.Add(store);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(store).State = EntityState.Detached;
                throw;
            }

            OnAfterStoreCreated(store);

            return store;
        }

        public async Task<BikeStores.Models.ConData.Store> CancelStoreChanges(BikeStores.Models.ConData.Store item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStoreUpdated(BikeStores.Models.ConData.Store item);
        partial void OnAfterStoreUpdated(BikeStores.Models.ConData.Store item);

        public async Task<BikeStores.Models.ConData.Store> UpdateStore(int storeid, BikeStores.Models.ConData.Store store)
        {
            OnStoreUpdated(store);

            var itemToUpdate = Context.Stores
                              .Where(i => i.store_id == store.store_id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(store);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStoreUpdated(store);

            return store;
        }

        partial void OnStoreDeleted(BikeStores.Models.ConData.Store item);
        partial void OnAfterStoreDeleted(BikeStores.Models.ConData.Store item);

        public async Task<BikeStores.Models.ConData.Store> DeleteStore(int storeid)
        {
            var itemToDelete = Context.Stores
                              .Where(i => i.store_id == storeid)
                              .Include(i => i.Stocks)
                              .Include(i => i.Orders)
                              .Include(i => i.Staff)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStoreDeleted(itemToDelete);


            Context.Stores.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStoreDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}