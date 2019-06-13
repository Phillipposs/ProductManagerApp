
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private string no;
        //private List<int> ids = new List<int>();
        
        public ViewProduct()
        {
           InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            HttpClient httpClient = new HttpClient();
            string response = httpClient.GetAsync("http://localhost:9388/getproducts/"+no).Result.Content.ReadAsStringAsync().Result;
           
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            no = cmb.SelectedItem.ToString().Split(':')[1].Split(' ')[1];


        }

        private void Product_Loaded(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            string response = httpClient.GetAsync("http://localhost:9388/getproducts").Result.Content.ReadAsStringAsync().Result;
            int[] niz = new int[response.Trim('[', ',', ']').Length];
            string binary = Convert.ToString(new char(), 2);
            niz = binary.Select(n => Convert.ToInt32(n)).ToArray();
             var lista = JsonConvert.DeserializeObject<ProductsRootObject>(response);
            //foreach(int id in lista)
            //{
            //    ids.Add(Convert.ToInt32(response.ElementAt<char>(i)));
            //}
            int x = 0;
        }
    }
}
