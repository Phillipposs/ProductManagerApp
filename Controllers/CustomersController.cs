using System.Dynamic;
using System.Linq;
using Microsoft.AspNetCore.Http;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NopCommerce.Api.AdapterLibrary;
using NopCommerce.Api.SampleApplication.DTOs;
//using NopCommerce.Api.SampleApplication.DTOs;

namespace NopCommerce.Api.SampleApplication.Controllers
{
    public class CustomersController : Controller
    {
        

        public ActionResult UpdateCustomer(int customerId)
        {
            var accessToken = (HttpContext.Session.GetString("accessToken") ?? TempData["accessToken"]).ToString();
            var serverUrl = (HttpContext.Session.GetString("serverUrl") ?? TempData["serverUrl"]).ToString();

            var nopApiClient = new ApiClient(accessToken, serverUrl);

            string jsonUrl = $"/api/customers/{customerId}";

            // we use anonymous type as we want to update only the last_name of the customer
            // also the customer shoud be the cutomer property of a holder object as explained in the documentation
            // https://github.com/SevenSpikes/api-plugin-for-nopcommerce/blob/nopcommerce-3.80/Customers.md#update-details-for-a-customer
            // i.e: { customer : { last_name: "" } }
            var customerToUpdate = new { customer = new { last_name = "" } };
            string customerJson = JsonConvert.SerializeObject(customerToUpdate);

            nopApiClient.Put(jsonUrl, customerJson);

            return RedirectToAction("GetCustomers");
        }
    }
}