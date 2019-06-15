using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using NopCommerce.Api.SampleApplication.Models;
using NopCommerce.Api.SampleApplication.Managers;
using NopCommerce.Api.SampleApplication.Parameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NopCommerce.Api.AdapterLibrary;
using NopCommerce.Api.SampleApplication.DTOs;
using System.Linq;
using WpfTest.DTOs;
using System.Collections.Generic;

namespace NopCommerce.Api.SampleApplication.Controllers
{
    public class AuthorizationController :Controller
    {


        [HttpPost("/submit")]
        [AllowAnonymous]
        //TODO: it is recommended to have an [Authorize] attribute set
        public ActionResult Submit([FromBody] UserAccessModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var nopAuthorizationManager = new AuthorizationManager(model.ClientId, model.ClientSecret, model.ServerUrl);

                    var redirectUrl = Url.RouteUrl("GetAccessToken", null, HttpContext.Request.Scheme); //  "http://localhost:9388/token";

                    if (redirectUrl != model.RedirectUrl)
                    {
                        return BadRequest();
                    }
                   
                    var convertedId = JsonConvert.SerializeObject(model.ClientId);
                    var convertedSecret = JsonConvert.SerializeObject(model.ClientSecret);
                    var convertedServerUrl = JsonConvert.SerializeObject(model.ServerUrl);
                    var convertedredirectUrl = JsonConvert.SerializeObject(redirectUrl);
                    //var convertedstate = JsonConvert.SerializeObject(state);

                    // For demo purposes this data is kept into the current Session, but in production environment you should keep it in your database
                    HttpContext.Session.SetString("clientId", model.ClientId);
                    HttpContext.Session.SetString("clientSecret", model.ClientSecret);
                    HttpContext.Session.SetString("serverUrl", model.ServerUrl);
                    HttpContext.Session.SetString("redirectUrl", model.RedirectUrl);

                    // This should not be saved anywhere.
                    var state = Guid.NewGuid();
                   // var convertedstate = JsonConvert.SerializeObject(state);
                    HttpContext.Session.SetString("state", state.ToString());

                    string authUrl = nopAuthorizationManager.BuildAuthUrl(redirectUrl, new string[] { }, state.ToString());

                    return Redirect(authUrl);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest();
        }

        [HttpGet("/token", Name = "GetAccessToken")]
        [AllowAnonymous]
        public ActionResult GetAccessToken(string code, string state)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(state))
            {
                if (state != HttpContext.Session.GetString("state"))
                {
                    return BadRequest();
                }

                var model = new AccessModel();

                try
                {
                    // TODO: Here you should get the authorization user data from the database instead from the current Session.
                    string clientId = HttpContext.Session.GetString("clientId");
                    string clientSecret = HttpContext.Session.GetString("clientSecret");
                    string serverUrl = HttpContext.Session.GetString("serverUrl");
                    string redirectUrl = HttpContext.Session.GetString("redirectUrl");

                    var authParameters = new AuthParameters()
                    {
                        ClientId = clientId,
                        ClientSecret = clientSecret,
                        ServerUrl = serverUrl,
                        RedirectUrl = redirectUrl,
                        GrantType = "authorization_code",
                        Code = code
                    };

                    var nopAuthorizationManager = new AuthorizationManager(authParameters.ClientId, authParameters.ClientSecret, authParameters.ServerUrl);

                    string responseJson = nopAuthorizationManager.GetAuthorizationData(authParameters);

                    AuthorizationModel authorizationModel = JsonConvert.DeserializeObject<AuthorizationModel>(responseJson);

                    model.AuthorizationModel = authorizationModel;
                    model.UserAccessModel = new UserAccessModel()
                    {
                        ClientId = clientId,
                        ClientSecret = clientSecret,
                        ServerUrl = serverUrl,
                        RedirectUrl = redirectUrl
                    };
                    var authorizationModelConverted = JsonConvert.SerializeObject(authorizationModel.AccessToken);
                    // TODO: Here you can save your access and refresh tokens in the database. For illustration purposes we will save them in the Session and show them in the view.
                    HttpContext.Session.SetString("accessToken", authorizationModel.AccessToken);
                    using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(@"auth.txt", true))
                    {
                       
                        file.WriteLine(authorizationModel.AccessToken);
                        file.WriteLine(authParameters.ServerUrl);
                    }

                    TempData["accessToken"] = authorizationModel.AccessToken;
                    TempData["serverUrl"] = serverUrl;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return View("~/Views/AccessToken.cshtml", model);
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult RefreshAccessToken(string refreshToken, string clientId, string clientSecret, string serverUrl)
        {
            string json = string.Empty;

            if (ModelState.IsValid &&
                !string.IsNullOrEmpty(refreshToken) &&
                !string.IsNullOrEmpty(clientId) &&
                 !string.IsNullOrEmpty(clientSecret) &&
                !string.IsNullOrEmpty(serverUrl))
            {
                var model = new AccessModel();

                try
                {
                    var authParameters = new AuthParameters()
                    {
                        ClientId = clientId,
                        ClientSecret = clientSecret,
                        ServerUrl = serverUrl,
                        RefreshToken = refreshToken,
                        GrantType = "refresh_token"
                    };

                    var nopAuthorizationManager = new AuthorizationManager(authParameters.ClientId,
                        authParameters.ClientSecret, authParameters.ServerUrl);

                    string responseJson = nopAuthorizationManager.RefreshAuthorizationData(authParameters);

                    AuthorizationModel authorizationModel =
                        JsonConvert.DeserializeObject<AuthorizationModel>(responseJson);

                    model.AuthorizationModel = authorizationModel;
                    model.UserAccessModel = new UserAccessModel()
                    {
                        ClientId = clientId,
                        ServerUrl = serverUrl
                    };

                    // Here we use the temp data because this method is called via ajax and here we can't hold a session.
                    // This is needed for the GetCustomers method in the CustomersController.
                    TempData["accessToken"] = authorizationModel.AccessToken;
                    TempData["serverUrl"] = serverUrl;
                }
                catch (Exception ex)
                {
                    json = string.Format("error: '{0}'", ex.Message);

                    return Ok(json);
                }

                json = JsonConvert.SerializeObject(model.AuthorizationModel);
            }
            else
            {
                json = "error: 'something went wrong'";
            }

            return Ok(json);
        }
        [HttpGet("getcustomers", Name = "GetCustomers")]
        [AllowAnonymous]
        public ActionResult GetCustomers()
        {
            // TODO: Here you should get the data from your database instead of the current Session.
            // Note: This should not be done in the action! This is only for illustration purposes.
            string [] lines = System.IO.File.ReadAllLines(@"auth.txt");
            var accessToken = lines[0];  //Settings.Default["accessToken"].ToString();//HttpContext.Session.GetString("accessToken");
            var serverUrl = lines[1];

            var nopApiClient = new ApiClient(accessToken, serverUrl);

            string jsonUrl = $"/api/customers?fields=id,first_name,last_name";
            object customersData = nopApiClient.Get(jsonUrl);

            var customersRootObject = JsonConvert.DeserializeObject<CustomersRootObject>(customersData.ToString());

            var customers = customersRootObject.Customers.Where(
                customer => !string.IsNullOrEmpty(customer.FirstName) || !string.IsNullOrEmpty(customer.LastName));

            return Ok(customersRootObject);
        }
        [HttpGet("getproducts/{id}", Name = "GetProduct")]
        [AllowAnonymous]
        public ActionResult GetProduct(int id)
        {
            // TODO: Here you should get the data from your database instead of the current Session.
            // Note: This should not be done in the action! This is only for illustration purposes.
            string[] lines = System.IO.File.ReadAllLines(@"auth.txt");
            var accessToken = lines[0];  //Settings.Default["accessToken"].ToString();//HttpContext.Session.GetString("accessToken");
            var serverUrl = lines[1];

            var nopApiClient = new ApiClient(accessToken, serverUrl);

            string jsonUrl = String.Format("/api/products/{0}",id);//$"/api/products?ids={}";
            object customersData = nopApiClient.Get(jsonUrl);

            var productsRaw = JsonConvert.DeserializeObject<ProductsRootObject>(customersData.ToString());

            //var customers = productsRaw.Products.Where(
            //    customer => !string.IsNullOrEmpty(customer.FirstName) || !string.IsNullOrEmpty(customer.LastName));

            return Ok(productsRaw);
        }
        [HttpGet("getproducts", Name = "GetProducts")]
        [AllowAnonymous]
        public ActionResult GetProducts()
        {
            // TODO: Here you should get the data from your database instead of the current Session.
            // Note: This should not be done in the action! This is only for illustration purposes.
            string[] lines = System.IO.File.ReadAllLines(@"auth.txt");
            var accessToken = lines[0];  //Settings.Default["accessToken"].ToString();//HttpContext.Session.GetString("accessToken");
            var serverUrl = lines[1];

            var nopApiClient = new ApiClient(accessToken, serverUrl);

            string jsonUrl = $"/api/products?fields=id";
            object productsData = nopApiClient.Get(jsonUrl);

            var productsRaw = JsonConvert.DeserializeObject<ProductsRootObject>(productsData.ToString());
           
            //List<int> niz = new List<int>();
            //foreach(ProductDTO prod in productsRaw.Products)
            //{
            //    niz.Add(prod.id);
            //}
                        
            return Ok(productsRaw);
        }
        [HttpPost("/addproduct")]
        [AllowAnonymous]
        //TODO: it is recommended to have an [Authorize] attribute set
        public ActionResult AddProduct([FromBody] ProductDTO product)
        {
            string[] lines = System.IO.File.ReadAllLines(@"auth.txt");
            var accessToken = lines[0];  //Settings.Default["accessToken"].ToString();//HttpContext.Session.GetString("accessToken");
            var serverUrl = lines[1];
            ProductSendDTO productSendDTO = new ProductSendDTO();
            productSendDTO.product = product;
            var nopApiClient = new ApiClient(accessToken, serverUrl);
            var convertedModel = JsonConvert.SerializeObject(productSendDTO,
     Formatting.None,
     new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            string jsonUrl = $"/api/products";
            object productsData = nopApiClient.Post(jsonUrl, convertedModel);
            return Ok();
        }
            private IActionResult BadRequestMsg(string message = "Bad Request")
        {
            return BadRequest(message);
        }
    }
}