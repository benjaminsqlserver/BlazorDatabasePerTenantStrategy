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
    public partial class EditProduct
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
        public int product_id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            product = await ConDataService.GetProductByProductId(product_id);

            brandsForbrandId = await ConDataService.GetBrands();

            categoriesForcategoryId = await ConDataService.GetCategories();
        }
        protected bool errorVisible;
        protected BikeStores.Models.ConData.Product product;

        protected IEnumerable<BikeStores.Models.ConData.Brand> brandsForbrandId;

        protected IEnumerable<BikeStores.Models.ConData.Category> categoriesForcategoryId;

        protected async Task FormSubmit()
        {
            try
            {
                await ConDataService.UpdateProduct(product_id, product);
                DialogService.Close(product);
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

            product = await ConDataService.GetProductByProductId(product_id);
        }
    }
}