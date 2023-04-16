using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using BikeStores.Data;

namespace BikeStores.Controllers
{
    public partial class ExportConDataController : ExportController
    {
        private readonly ConDataContext context;
        private readonly ConDataService service;

        public ExportConDataController(ConDataContext context, ConDataService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/ConData/brands/csv")]
        [HttpGet("/export/ConData/brands/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBrandsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBrands(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/brands/excel")]
        [HttpGet("/export/ConData/brands/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBrandsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBrands(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/categories/csv")]
        [HttpGet("/export/ConData/categories/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCategoriesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCategories(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/categories/excel")]
        [HttpGet("/export/ConData/categories/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCategoriesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCategories(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/products/csv")]
        [HttpGet("/export/ConData/products/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProductsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetProducts(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/products/excel")]
        [HttpGet("/export/ConData/products/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProductsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetProducts(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/stocks/csv")]
        [HttpGet("/export/ConData/stocks/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStocksToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStocks(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/stocks/excel")]
        [HttpGet("/export/ConData/stocks/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStocksToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStocks(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/customers/csv")]
        [HttpGet("/export/ConData/customers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCustomersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCustomers(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/customers/excel")]
        [HttpGet("/export/ConData/customers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCustomersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCustomers(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/orderitems/csv")]
        [HttpGet("/export/ConData/orderitems/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportOrderItemsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetOrderItems(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/orderitems/excel")]
        [HttpGet("/export/ConData/orderitems/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportOrderItemsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetOrderItems(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/orders/csv")]
        [HttpGet("/export/ConData/orders/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportOrdersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetOrders(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/orders/excel")]
        [HttpGet("/export/ConData/orders/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportOrdersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetOrders(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/staff/csv")]
        [HttpGet("/export/ConData/staff/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStaffToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStaff(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/staff/excel")]
        [HttpGet("/export/ConData/staff/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStaffToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStaff(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/stores/csv")]
        [HttpGet("/export/ConData/stores/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStoresToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStores(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/stores/excel")]
        [HttpGet("/export/ConData/stores/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStoresToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStores(), Request.Query), fileName);
        }
    }
}
