using System;
using System.Net.Http;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace weather_app
{
    class MainWindow : Window
    {
        [UI] private Label Title = null;
		[UI] private Entry CityEntry = null;
		[UI] private Button ConfirmButton1 = null;
		[UI] private Image image = null;
        [UI] private Entry entry = null;
        [UI] private Label _label1 = null;
        [UI] private Label _label2 = null;
        [UI] private Label _label3 = null;
        [UI] private Label _label4 = null;
        [UI] private Label _label5 = null;
        [UI] private Label _label6 = null;
        [UI] private Label _label7 = null;
        [UI] private Button _button1 = null;

        private int _counter;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);
			entry.Text = "a";
            DeleteEvent += Window_DeleteEvent;
			_button1.Clicked += Button1_Clicked;
			ConfirmButton1.Clicked += ConfirmButton1_Clicked;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

		private async void Button1_Clicked(object sender, EventArgs a)
		{
			String city = entry.Text;
            using var client = new HttpClient();
            String first = String.Format("http://api.openweathermap.org/geo/1.0/direct?q={0}&limit=5&appid=", city);
            var content = await client.GetStringAsync(first);
            String name ="";
            String lat = "";
            String lon = "";
            String NeededInformation = "";
            Boolean testify = false;
            Boolean testify1 = false;
            Boolean testify2 = false;
            String icon = "";
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i]=='f' && content[i+1]== 'r' && testify == false)
                {
                    int u =i+5;
                    for (int j = u; j < content.Length; j++)
                    {
                        if (content[j] == '"')
                        {
                            NeededInformation += "City: " + name ;
                            testify = true;
                            _label1.Text = NeededInformation;
                            NeededInformation = "";
                            break;
                        }
                        name += content[j];
                    }
                }
                if (content[i]=='l' && content[i+1]== 'a' &&  content[i+2]== 't' && testify1 == false)
                {
                    int u =i+5;
                    for (int j = u; j < content.Length; j++)
                    {
                        if (content[j] == ',')
                        {
                            NeededInformation += "Lat: " +lat;
                            testify1 = true;
                            _label2.Text = NeededInformation;
                            NeededInformation = "";
                            break;
                        }
                        lat += content[j];
                    }
                }
                 if (content[i]=='l' && content[i+1]== 'o' &&  content[i+2]== 'n' && testify2 == false)
                {
                    int u =i+5;
                    for (int j = u; j < content.Length; j++)
                    {
                        if (content[j] == ',')
                        {
                            NeededInformation += "Lon: " +lon ;
                            testify2 = true;
                            _label3.Text = NeededInformation;
                            NeededInformation = "";
                            break;
                        }
                        lon += content[j];
                    }
                }
            }
            String second = String.Format("https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid=&units=metric", lat, lon);
            var otherscontent = await client.GetStringAsync(second);
            Boolean testify9 = false;
            Boolean testify8 = false;
            Boolean testify7 = false;
            Boolean testify6 = false;
            String temp = "";
            String desc = "";
            String hum = "";
            for (int k = 0; k < otherscontent.Length; k++)
            {
                if (otherscontent[k]=='t' && otherscontent[k+1]== 'e' && otherscontent[k+2]=='m' && otherscontent[k+3]=='p' && testify9 == false)
                {
                    int u = k+6;
                    for (int j = u; j < otherscontent.Length; j++)
                    {
                        if (otherscontent[j] == ',')
                        {
                            NeededInformation += "Temperature: " + temp + "°C ";
                            testify9 = true;
                            _label4.Text = NeededInformation;
                            NeededInformation = "";
                            break;
                        }
                        temp += otherscontent[j];
                    }
                
                }else if (otherscontent[k]== 'i' && otherscontent[k+1]== 'c' && otherscontent[k+2] == 'o' && otherscontent[k+3] == 'n' && testify6 == false)
                {
                    int u = k+7;
                    for (int j = u; j < otherscontent.Length; j++)
                    {
                        if (otherscontent[j] == '"')
                        {
                            _label4.Text = icon;
                            String picture = String.Format("./img/{0}.png", icon);
                            image.Pixbuf = new Gdk.Pixbuf (picture);
                            testify6 = true;
                            break;
                        }
                        icon += otherscontent[j];
                    }
                }else if (otherscontent[k]=='d' && otherscontent[k+1]== 'e' && otherscontent[k+2]=='s' && otherscontent[k+3]=='c' && otherscontent[k+4]=='r' && otherscontent[k+5]=='i' && otherscontent[k+6]=='p' && otherscontent[k+7]=='t' && otherscontent[k+8]=='i' && otherscontent[k+9]=='o' && otherscontent[k+10]=='n' && testify8 == false)
                {
                    int u = k+14;
                    for (int j = u; j < otherscontent.Length; j++)
                    {
                        if (otherscontent[j] == '"')
                        {
                            NeededInformation += "Weather: " + desc ;
                            testify8 = true;
                            _label5.Text = NeededInformation;
                            NeededInformation = "";
                            break;
                        }
                        desc += otherscontent[j];
                    }
                
                }else if (otherscontent[k]=='h' && otherscontent[k+1]== 'u' && otherscontent[k+2]=='m' && otherscontent[k+3]=='i' && otherscontent[k+4]=='d' && otherscontent[k+5]=='i' && otherscontent[k+6]=='t' && otherscontent[k+7]=='y' && testify7 == false)
                {
                    int u = k+10;
                    for (int j = u; j < otherscontent.Length; j++)
                    {
                        if (otherscontent[j] == '}' || otherscontent[j] == ',')
                        {
                            NeededInformation += "Humidity: " + hum + "%";
                            testify7 = true;
                            _label6.Text = NeededInformation;
                            NeededInformation = "";
                            break;
                        }
                        hum += otherscontent[j];
                    }
                
                }
            }           
		}
		private async void ConfirmButton1_Clicked(object sender, EventArgs a)
		{
			HttpClient webclient = new HttpClient();
			var input = CityEntry.Text;
			var lien = String.Format("http://api.openweathermap.org/geo/1.0/direct?q={0}&limit=5&appid=", input);
			try
			{
				var content = await webclient.GetStringAsync(lien);
				var test = JsonConvert.DeserializeObject<ValueOfCities[]>(content);
				var result = test[0].name;
				Title.Text = result;
			}
			catch (Exception e)
            {
                Title.Text = "error";
            }
			
		}
	}

	public class ValueOfCities
	{
		public string name { get; set; }
	    public List<ValueOfCities> objectList { get; set; }
	}
}
