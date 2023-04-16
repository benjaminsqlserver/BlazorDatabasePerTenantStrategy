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
    public partial class EditOrderItem
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

        [Parameter]
        public int order_id { get; set; }

        [Parameter]
        public int item_id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            orderItem = await ConDataService.GetOrderItemByOrderIdAndItemId(order_id, item_id);

            ordersFororderId = await ConDataService.GetOrders();

            productsForproductId = await ConDataService.GetProducts();
        }
        protected bool errorVisible;
        protected BikeStores.Models.ConData.OrderItem orderItem;

        protected IEnumerable<BikeStores.Models.ConData.Order> ordersFororderId;

        protected IEnumerable<BikeStores.Models.ConData.Product> productsForproductId;

        protected async Task FormSubmit()
        {
            try
            {
                await ConDataService.UpdateOrderItem(order_id, item_id, orderItem);
                DialogService.Close(orderItem);
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


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
           ConDataService.Reset();
            hasChanges = false;
            canEdit = true;

            orderItem = await ConDataService.GetOrderItemByOrderIdAndItemId(order_id, item_id);
        }
    }
}