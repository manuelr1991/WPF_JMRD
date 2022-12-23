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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_JMRD.Class;

namespace WPF_JMRD
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Content = "Espere";
            ((Button)sender).IsEnabled = false;
            await Task.Run(() => Consultar());
        }

        private async void Consultar()
        {
            string url = "https://api.github.com/users";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246");
                    using (var resultado = httpClient.GetAsync(url).Result)
                    {
                        string gitReult = resultado.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(gitReult);
                        List<User> resultadoJson = JsonConvert.DeserializeObject<List<User>>(gitReult);
                        for(int i = 1; i <= 10; i++)
                        {
                            tbResultado.Dispatcher.Invoke(() =>
                            {
                                tbResultado.Text += "\r\n" + resultadoJson[i].login + " | " + resultadoJson[i].id + " | " + resultadoJson[i].node_id;
                            });
                        }
                    }
                }

                btnOperacion.Dispatcher.Invoke(() =>
                {
                    btnOperacion.Content = "Ejecutar operación";
                    btnOperacion.IsEnabled = true;
                });
            }
            catch(Exception e)
            {
                btnOperacion.Dispatcher.Invoke(() =>
                {
                    btnOperacion.Content = "Error";
                });
            }
        }
    }
}
