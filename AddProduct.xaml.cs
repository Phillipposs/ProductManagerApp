using Newtonsoft.Json;
using NopCommerce.Api.SampleApplication.Controllers;
using NopCommerce.Api.SampleApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfTest.DTOs;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private MainController _mainController;
        static HttpClient client = new HttpClient();
        public AddProduct(MainController mainController)
        {
            _mainController = mainController;
            InitializeComponent();
         
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProductDTO productDTO = new ProductDTO();
            productDTO.name = ImeProizvoda.Text;
            productDTO.short_description = KratakOpis.Text;
            productDTO.full_description = KompletanOpis.Text;
            productDTO.sku = Sku.Text;
            productDTO.stock_quantity = Kolicina.Text;
            productDTO.price = Cena.Text;
            productDTO.old_price = StaraCena.Text;
            productDTO.weight = Tezina.Text;
            productDTO.length = Duzina.Text;
            productDTO.width = Sirina.Text;
            productDTO.height = Visina.Text;
            var convertedModel = JsonConvert.SerializeObject(productDTO);
            //CreateProductAsync(userAccessModel);
            new HttpClient().PostAsync("http://localhost:9388/addproduct", new StringContent(convertedModel, Encoding.UTF8, "application/json"));
            // new HttpClient().GetAsync("http://localhost:9388/getcustomers");
            // _authorizationController.Submit(userAccessModel);
        }


        static async Task<Uri> CreateProductAsync(UserAccessModel userAccessModel)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "http://localhost:9388/submit", userAccessModel);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }
    }
}

//http://localhost:15536/oauth/authorize?client_id=b20033f1-af49-421f-81b9-5e32bc7f5149
//    &redirect_uri=http%3a%2f%2flocalhost%3a49676%2ftoken
//    &response_type=code&state=e4f2c78d-aac8-40ba-857a-a7f03eefb084


//http://localhost:15536/oauth/authorize?client_id=b20033f1-af49-421f-81b9-5e32bc7f5149
//&redirect_uri=http%3A%2F%2Flocalhost%3A9388%2Ftoken
//&response_type=code&state=6545e277-5bd8-41d8-bf3c-6d417976c257"
