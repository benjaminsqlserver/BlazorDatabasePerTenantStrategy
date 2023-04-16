using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace BikeStores.Pages
{
    public partial class Staff
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public ConDataService ConDataService { get; set; }

        protected IEnumerable<BikeStores.Models.ConData.Staff> staff;

        protected RadzenDataGrid<BikeStores.Models.ConData.Staff> grid0;

        protected string search = "";

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            staff = await ConDataService.GetStaff(new Query { Filter = $@"i => i.first_name.Contains(@0) || i.last_name.Contains(@0) || i.email.Contains(@0) || i.phone.Contains(@0)", FilterParameters = new object[] { search }, Expand = "Staff1,Store" });
        }
        protected override async Task OnInitializedAsync()
        {
            staff = await ConDataService.GetStaff(new Query { Filter = $@"i => i.first_name.Contains(@0) || i.last_name.Contains(@0) || i.email.Contains(@0) || i.phone.Contains(@0)", FilterParameters = new object[] { search }, Expand = "Staff1,Store" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddStaff>("Add Staff", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<BikeStores.Models.ConData.Staff> args)
        {
            await DialogService.OpenAsync<EditStaff>("Edit Staff", new Dictionary<string, object> { {"staff_id", args.Data.staff_id} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, BikeStores.Models.ConData.Staff staff)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ConDataService.DeleteStaff(staff.staff_id);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete Staff" 
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ConDataService.ExportStaffToCSV(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "Staff1,Store", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
}, "Staff");
            }

            if (args == null || args.Value == "xlsx")
            {
                await ConDataService.ExportStaffToExcel(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "Staff1,Store", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
}, "Staff");
            }
        }
    }
}