using Newtonsoft.Json;
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
    /// Interaction logic for ViewCategory.xaml
    /// </summary>
    public partial class ViewCategory : Window
    {
        private int num;
        private List<int> ids = new List<int>();
        private string no;

        public ViewCategory()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CategoryUpdateDTO categoryDTO = new CategoryUpdateDTO();
            categoryDTO.id = Convert.ToInt32(no);
            categoryDTO.name = NazivKategorije.Text;
            categoryDTO.description = OpisKategorije.Text;
            var convertedModel = JsonConvert.SerializeObject(categoryDTO);
            //CreateProductAsync(userAccessModel);
            new HttpClient().PostAsync("http://localhost:9388/updatecategory", new StringContent(convertedModel, Encoding.UTF8, "application/json"));
        }
        private void Category_Loaded(object sender, RoutedEventArgs e)
        {
            Intialize();
        }
        private void Intialize()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response =  httpClient.GetAsync("http://localhost:9388/getcategories").Result;
            var res = response.Content.ReadAsStringAsync().Result;

            var lista = JsonConvert.DeserializeObject<CategoriesRootObject>(res);
            num = lista.Categories.ToArray().Length;
            foreach (CategoryFullDTO categoryDTO in lista.Categories)
            {
                ids.Add(categoryDTO.id);
                categoriesCombo.Items.Add(categoryDTO.id);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            no = cmb.SelectedItem.ToString();
            HttpClient httpClient = new HttpClient();
            string response = httpClient.GetAsync("http://localhost:9388/getcategories/" + no).Result.Content.ReadAsStringAsync().Result;
            var categoriesRoot = JsonConvert.DeserializeObject<CategoriesRootObject>(response);
            var categories = categoriesRoot.Categories;
            foreach (var category in categories)
            {
                NazivKategorije.Text = category.name;
                OpisKategorije.Text = category.description;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.DeleteAsync("http://localhost:9388/deletecategories/" + no).Result;
            var res = response.Content.ReadAsStringAsync().Result;
        }
    }
}
