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
    public partial class AddOrder
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

        protected override async Task OnInitializedAsync()
        {
            order = new BikeStores.Models.ConData.Order();

            customersForcustomerId = await ConDataService.GetCustomers();

            staffForstaffId = await ConDataService.GetStaff();

            storesForstoreId = await ConDataService.GetStores();
        }
        protected bool errorVisible;
        protected BikeStores.Models.ConData.Order order;

        protected IEnumerable<BikeStores.Models.ConData.Customer> customersForcustomerId;

        protected IEnumerable<BikeStores.Models.ConData.Staff> staffForstaffId;

        protected IEnumerable<BikeStores.Models.ConData.Store> storesForstoreId;

        protected async Task FormSubmit()
        {
            try
            {
                await ConDataService.CreateOrder(order);
                DialogService.Close(order);
            }
            catch (Exception ex)
            {
                hasChanges = ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException;
                canEdit = !(ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException);
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }
    }
}