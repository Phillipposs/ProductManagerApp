using Newtonsoft.Json;
using NopCommerce.Api.SampleApplication.DTOs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfTest.DTOs;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        private string role;

        public AddCustomer()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            role = cmb.SelectedItem.ToString().Split(':')[1];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            CustomerCreateDTO customerDTO = new CustomerCreateDTO();
            customerDTO.FirstName = ImeKorisnika.Text;
            customerDTO.LastName = PrezimeKorisnika.Text;
            customerDTO.Email = Email.Text;

            customerDTO.CustomerRoles.Add(3);
          
          
            var convertedModel = JsonConvert.SerializeObject(customerDTO);
            //CreateProductAsync(userAccessModel);
            new HttpClient().PostAsync("http://localhost:9388/addcustomer", new StringContent(convertedModel, Encoding.UTF8, "application/json"));
        }
    }
}
