using Newtonsoft.Json;
using NopCommerce.Api.SampleApplication.DTOs;
using NopCommerce.Api.SampleApplication.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
          
            
            InitializeComponent();
            Loaded += MyWindow_Loaded;
        }
        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetToken();
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await HostBuilder.Start();
        }
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProduct addProduct = new AddProduct(new NopCommerce.Api.SampleApplication.Controllers.AuthorizationController());
            addProduct.Show();
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategory addCategory = new AddCategory();
            addCategory.Show();
        }

        private void ViewCategory_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            ViewCategory viewCategory = new ViewCategory();
            viewCategory.Show();
            string response = httpClient.GetAsync("http://localhost:9388/getcustomers").Result.Content.ReadAsStringAsync().Result;
            // CustomersRootObject customers = new CustomersRootObject();
            CustomersRootObject customers = JsonConvert.DeserializeObject<CustomersRootObject>(response);

        }
        private void GetToken()
        {
            UserAccessModel userAccessModel = new UserAccessModel();
            userAccessModel.ClientId = "b20033f1-af49-421f-81b9-5e32bc7f5149";
            userAccessModel.ClientSecret = "c993b4bf-a362-4469-b7d7-d1a39819021b";
            userAccessModel.RedirectUrl = "http://localhost:9388/token";
            userAccessModel.ServerUrl = "http://localhost:15536";
            var convertedModel = JsonConvert.SerializeObject(userAccessModel);
            //CreateProductAsync(userAccessModel);
            new HttpClient().PostAsync("http://localhost:9388/Submit", new StringContent(convertedModel, Encoding.UTF8, "application/json"));
        }
        private Task<HttpResponseMessage> GetCustomersAsync()
        {
            return new HttpClient().GetAsync("http://localhost:9388/getcustomers");
        }
        private void ViewProduct_Click(object sender, RoutedEventArgs e)
        {
            

            ViewProduct viewProduct = new ViewProduct();
            viewProduct.Show();
        }
    }
}
