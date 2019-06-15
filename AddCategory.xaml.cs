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
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategory : Window
    {
        public AddCategory()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CategoryDTO categoryDTO = new CategoryDTO();
            categoryDTO.name = ImeKategorije.Text;
            categoryDTO.description = Opis.Text;
            var convertedModel = JsonConvert.SerializeObject(categoryDTO);
            //CreateProductAsync(userAccessModel);
            new HttpClient().PostAsync("http://localhost:9388/addcategory", new StringContent(convertedModel, Encoding.UTF8, "application/json"));
        }
    }
}
