﻿
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfTest.DTOs;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for ViewProduct.xaml
    /// </summary>
    public partial class ViewProduct : Window
    {
        private int num = 0;
        private List<int> ids = new List<int>();
        private string no;
        //private List<int> ids = new List<int>();
        
        public ViewProduct()
        {
           InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            ProductDTO productDTO = new ProductDTO();
            productDTO.id = Convert.ToInt32(no);
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
            new HttpClient().PostAsync("http://localhost:9388/updateproduct", new StringContent(convertedModel, Encoding.UTF8, "application/json"));


        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            no = cmb.SelectedItem.ToString();
            HttpClient httpClient = new HttpClient();
            string response = httpClient.GetAsync("http://localhost:9388/getproducts/" + no).Result.Content.ReadAsStringAsync().Result;
            var productRoot = JsonConvert.DeserializeObject<ProductsRootObject>(response);
            var product = productRoot.Products;
            foreach (var prod in product)
            {
                ImeProizvoda.Text = prod.name;
                KratakOpis.Text = prod.full_description;
                Sku.Text = prod.sku;
                Kolicina.Text = prod.stock_quantity;
                Cena.Text = prod.price;
                StaraCena.Text = prod.old_price;
                Tezina.Text = prod.weight;
                Duzina.Text = prod.length;
                Sirina.Text = prod.width;
                Visina.Text = prod.height;
            }

        }

        private void Product_Loaded(object sender, RoutedEventArgs e)
        {
             Intialize();
        }
        private void Intialize()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.GetAsync("http://localhost:9388/getproducts").Result;
            var res = response.Content.ReadAsStringAsync().Result;

            var lista = JsonConvert.DeserializeObject<ProductsRootObject>(res);
            num = lista.Products.ToArray().Length;
            foreach (ProductDTO productDTO in lista.Products)
            {
                ids.Add(productDTO.id);
                comboProducts.Items.Add(productDTO.id);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            HttpClient httpClient = new HttpClient();
            string response = httpClient.DeleteAsync("http://localhost:9388/deleteproducts/" + no).Result.StatusCode.ToString();
            if(response.Equals("OK"))
            MessageBox.Show("Obrisano !", "My App");
        }


    }
}
