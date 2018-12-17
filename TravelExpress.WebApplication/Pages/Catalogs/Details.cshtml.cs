namespace TravelExpress.WebApplication.Pages.Catalogs
{
    using EnaBricks.Generics;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Products;
    using TravelExpress.Entities.Products;
    using TravelExpress.Queries.Products;

    public class DetailsModel : BasePageModel
    {
        private readonly ProductQueryComponent _productQueryComponent;

        public Product Product { get; set; }

        [BindProperty]
        public Guid Id { get; set; }

        [BindProperty]
        public AddAditionalServiceRequest AditionalService { get; set; }

        [BindProperty]
        public AddPriceDetailRequest AddPriceDetailRequest { get; set; }

        [BindProperty]
        public AddUpdatePriceCategoryRequest PriceCategoryRequest { get; set; }

        [BindProperty]
        public AddUpdateProductRequest AddUpdateProductRequest { get; set; }

        [BindProperty]
        public AddExcursionRequest AddExcursionRequest { get; set; }

        public DetailsModel(ProductQueryComponent productQueryComponent)
        {
            _productQueryComponent = productQueryComponent;
        }


        public async Task OnGetAsync(Guid id)
        {
            Id = id;
            Product = await _productQueryComponent.ProductAsync(Id);
        }

        public async Task<ActionResult> OnPostUpdateProductAsync([FromServices]IProductComponentFactory productComponentFactory)
        {
            try
            {
                ModelState.Clear();
                if (!TryValidateModel(AddUpdateProductRequest))
                    return Page();
                Option<IProductComponent> productResult = await productComponentFactory.CurrentAsync(Id);

                if (!productResult.Any())
                    ModelState.TryAddModelError("", "Error, registro no existe en bd");

                IProductComponent product = productResult.Single();

                WorkflowResult result = await product.UpdateDescriptionAsync(AddUpdateProductRequest.Description);

                if (!result.Success)
                {
                    for (int i = 0; i < result.Errors.Length; i++)
                    {
                        ModelState.TryAddModelError("", result.Errors[i]);
                    }
                }

                return Page();
            }
            finally
            {
                AditionalService = new AddAditionalServiceRequest();
                Product = await _productQueryComponent.ProductAsync(Id);
            }
        }

        public async Task<ActionResult> OnPostAditionalServiceAsync([FromServices]IProductComponentFactory productComponentFactory)
        {
            try
            {
                ModelState.Clear();
                if (!TryValidateModel(AditionalService))
                    return Page();
                Option<IProductComponent> productResult = await productComponentFactory.CurrentAsync(Id);

                if (!productResult.Any())
                    ModelState.TryAddModelError("", "Error, registro no existe en bd");

                IProductComponent product = productResult.Single();

                WorkflowResult result = await product.AddAditionalServiceAsync(AditionalService.Description, AditionalService.UnitPrice);

                if (!result.Success)
                {
                    for (int i = 0; i < result.Errors.Length; i++)
                    {
                        ModelState.TryAddModelError("", result.Errors[i]);
                    }
                }

                return Page();
            }
            finally
            {
                AditionalService = new AddAditionalServiceRequest();
                Product = await _productQueryComponent.ProductAsync(Id);
            }
        }

        public async Task<ActionResult> OnPostDeleteAditionalServiceAsync(
            [FromServices]IProductComponentFactory productComponentFactory,
            int aditionalServiceId
            )
        {
            try
            {
                ModelState.Clear();
                if (aditionalServiceId <= 0)
                    return Page();
                Option<IProductComponent> productResult = await productComponentFactory.CurrentAsync(Id);

                if (!productResult.Any())
                    ModelState.TryAddModelError("", "Error, registro no existe en bd");

                IProductComponent product = productResult.Single();

                WorkflowResult result = await product.DeleteAditionalServiceAsync(aditionalServiceId);

                if (!result.Success)
                {
                    for (int i = 0; i < result.Errors.Length; i++)
                    {
                        ModelState.TryAddModelError("", result.Errors[i]);
                    }
                }

                return Page();
            }
            finally
            {
                AditionalService = new AddAditionalServiceRequest();
                Product = await _productQueryComponent.ProductAsync(Id);
            }
        }

        public async Task<ActionResult> OnPostPriceCategoryAsync([FromServices]IProductComponentFactory productComponentFactory)
        {
            try
            {
                ModelState.Clear();
                if (!TryValidateModel(PriceCategoryRequest))
                    return Page();
                Option<IProductComponent> productResult = await productComponentFactory.CurrentAsync(Id);

                if (!productResult.Any())
                    ModelState.TryAddModelError("", "Error, registro no existe en bd");

                IProductComponent product = productResult.Single();

                WorkflowResult result = await product.AddPriceCategoryAsync(PriceCategoryRequest.Description);

                if (!result.Success)
                {
                    for (int i = 0; i < result.Errors.Length; i++)
                    {
                        ModelState.TryAddModelError("", result.Errors[i]);
                    }
                }

                return Page();
            }
            finally
            {
                AditionalService = new AddAditionalServiceRequest();
                Product = await _productQueryComponent.ProductAsync(Id);
            }
        }

        public async Task<ActionResult> OnPostDeletePriceCategoryAsync(
            [FromServices]IProductComponentFactory productComponentFactory,
            int priceCategoryId)
        {
            try
            {
                ModelState.Clear();
                if (priceCategoryId <= 0)
                {
                    ModelState.TryAddModelError("", "Acción Invalida");
                    return Page();
                }
                Option<IProductComponent> productResult = await productComponentFactory.CurrentAsync(Id);

                if (!productResult.Any())
                    ModelState.TryAddModelError("", "Error, registro no existe en bd");

                IProductComponent product = productResult.Single();

                WorkflowResult result = await product.DeletePriceCategoryAsync(priceCategoryId);

                if (!result.Success)
                {
                    for (int i = 0; i < result.Errors.Length; i++)
                    {
                        ModelState.TryAddModelError("", result.Errors[i]);
                    }
                }

                return Page();
            }
            finally
            {
                AditionalService = new AddAditionalServiceRequest();
                Product = await _productQueryComponent.ProductAsync(Id);
            }
        }

        public async Task<ActionResult> OnPostAddPriceDetailAsync([FromServices]IProductComponentFactory productComponentFactory)
        {
            try
            {
                ModelState.Clear();
                if (!TryValidateModel(AddPriceDetailRequest))
                    return Page();
                Option<IProductComponent> productResult = await productComponentFactory.CurrentAsync(Id);

                if (!productResult.Any())
                    ModelState.TryAddModelError("", "Error, registro no existe en bd");

                IProductComponent product = productResult.Single();

                WorkflowResult result = await product.AddPriceDetailAsync(AddPriceDetailRequest.PriceCategoryId, AddPriceDetailRequest.Description, AddPriceDetailRequest.UnitPrice);

                if (!result.Success)
                {
                    for (int i = 0; i < result.Errors.Length; i++)
                    {
                        ModelState.TryAddModelError("", result.Errors[i]);
                    }
                }

                return Page();
            }
            finally
            {
                AditionalService = new AddAditionalServiceRequest();
                Product = await _productQueryComponent.ProductAsync(Id);
            }
        }

        public async Task<ActionResult> OnPostDeletePriceDetailAsync(
            [FromServices]IProductComponentFactory productComponentFactory,
            int priceDetailId)
        {
            try
            {
                ModelState.Clear();
                if (priceDetailId <= 0)
                {
                    ModelState.TryAddModelError("", "Acción Invalida");
                    return Page();
                }
                Option<IProductComponent> productResult = await productComponentFactory.CurrentAsync(Id);

                if (!productResult.Any())
                    ModelState.TryAddModelError("", "Error, registro no existe en bd");

                IProductComponent product = productResult.Single();

                WorkflowResult result = await product.DeletePriceDetailAsync(priceDetailId);

                if (!result.Success)
                {
                    for (int i = 0; i < result.Errors.Length; i++)
                    {
                        ModelState.TryAddModelError("", result.Errors[i]);
                    }
                }

                return Page();
            }
            finally
            {
                AditionalService = new AddAditionalServiceRequest();
                Product = await _productQueryComponent.ProductAsync(Id);
            }
        }

        public async Task<ActionResult> OnPostAddExcursionAsync([FromServices]IProductComponentFactory productComponentFactory)
        {
            try
            {
                ModelState.Clear();
                if (!TryValidateModel(AddExcursionRequest))
                    return Page();
                Option<IProductComponent> productResult = await productComponentFactory.CurrentAsync(Id);

                if (!productResult.Any())
                    ModelState.TryAddModelError("", "Error, registro no existe en bd");

                IProductComponent product = productResult.Single();

                WorkflowResult result = (await product.AddExcursionAsync(AddExcursionRequest.BeginDate, AddExcursionRequest.EndDate, UserId())).Key;

                if (!result.Success)
                {
                    for (int i = 0; i < result.Errors.Length; i++)
                    {
                        ModelState.TryAddModelError("", result.Errors[i]);
                    }
                }

                return Page();
            }
            finally
            {
                AditionalService = new AddAditionalServiceRequest();
                Product = await _productQueryComponent.ProductAsync(Id);
            }
        }
    }
}