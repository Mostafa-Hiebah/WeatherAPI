using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeatherAPI
{
    public partial class FrmWeather : Form
    {
        public FrmWeather()
        {
            InitializeComponent();
        }

        Country countries;
        WeatherData weather;

        string countryName, cityName, fullName;
        List<string> listOfCountry_City = new List<string>();

       

        //get countries and cities from API
        public void Load_Countries_Cities()
        {
            try
            {
                HttpHelper helper = new HttpHelper();
                var data = helper.ConvertToJson("https://countriesnow.space/api/v0.1/countries");
                countries = Newtonsoft.Json.JsonConvert.DeserializeObject<Country>(data);

                for (int i = 0; i < countries.data.Length; i++)
                {
                    countryName = countries.data[i].country;

                    for (int j = 0; j < countries.data[i].cities.Length; j++)
                    {
                        cityName = countries.data[i].cities[j];
                        fullName = cityName + ", " + countryName;
                        listOfCountry_City.Add(fullName);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            
        }

        //get data from weather api and set it in form
        private void getWeatherData()
        {
            try
            {
                string url = "http://api.weatherstack.com/current?access_key=975bc4353c1b9a53bedd9d2b3a4d208b&query=" + Properties.Settings.Default.FullName;
                HttpHelper helper = new HttpHelper();
                var data = helper.ConvertToJson(url);
                weather = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherData>(data);

                lblCityName.Invoke((MethodInvoker)(() => lblCityName.Text = weather.request.query));
                pictureBox1.ImageLocation = string.Join("", weather.current.weather_icons);
                lblDescribtion.Invoke((MethodInvoker)(() => lblDescribtion.Text = string.Join("", weather.current.weather_descriptions)));
                lblTemprature.Invoke((MethodInvoker)(() => lblTemprature.Text = weather.current.temperature.ToString()));
                lblWind.Invoke((MethodInvoker)(() => lblWind.Text = weather.current.wind_speed.ToString()));
                lblHumidity.Invoke((MethodInvoker)(() => lblHumidity.Text = weather.current.humidity.ToString()));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await Task.Run(() => getWeatherData());
        }

        private async void FrmWeather_Load(object sender, EventArgs e)
        {
            await Task.Run(() => getWeatherData());
            await Task.Run(() => Load_Countries_Cities());
            comboBox1.DataSource = listOfCountry_City;
            comboBox1.Enabled = true;
            btnPreview.Enabled = true;
            comboBox1.Text = Properties.Settings.Default.FullName;

        }


        private async void btnPreview_Click(object sender, EventArgs e)
        {
            //save last city in Application setting to start with it in next time
            Properties.Settings.Default.FullName = comboBox1.Text;
            Properties.Settings.Default.Save();
            await Task.Run(() => getWeatherData());
        }
    }
}
