using Microsoft.AspNetCore.Mvc.Filters;
using StocksApp.Controllers;
using StocksApp.Models;
using StocksApp.ServiceContracts.DTOs;

namespace StocksApp.Filters.ActionFilter
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {

        public CreateOrderActionFilter()
        {
            
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //TO DO: before logic
            /*
                => First checks if it has been applied TradeController.
                => If so, then it receives the action argument called "orderRequest".
                => It perform model level validations on that model object.
                => In case of no model errors, it doesn't nothing - just invokes the subsequent filter in the filter pipeline.
                => In case if it has one or more model errors, it has to create object of StockTrade model class with essential data, adds model errors to ViewBag.Errors and then reinvokes the "TradeController.Index" view.
             */

            if (context.Controller is TradeController tradeController)
            {
                var orderRequest = context.ActionArguments["orderRequest"] as IOrderRequest;

                if (orderRequest != null)
                {

                    //update date of order
                    orderRequest.DateAndTimeOfOrder = DateTime.Now;

                    //re-validate the model object after updating the date
                    tradeController.ModelState.Clear();
                    tradeController.TryValidateModel(orderRequest);


                    if (!tradeController.ModelState.IsValid)
                    {
                        tradeController.ViewBag.Errors = tradeController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                        StockTrade stockTrade = new StockTrade() { StockName = orderRequest.StockName, Quantity = orderRequest.Quantity, StockSymbol = orderRequest.StockSymbol };

                        context.Result = tradeController.View(nameof(TradeController.Index), stockTrade); //short-circuits or skips the subsequent action filters & action method
                    }
                    else
                    {
                        await next(); //invokes the subsequent filter or action method
                    }
                }
                else
                {
                    await next(); //invokes the subsequent filter or action method
                }
            }
            else
            {
                await next(); //calls the subsequent filter or action method
            }

            //TO DO: After logic
        }
    }
}
