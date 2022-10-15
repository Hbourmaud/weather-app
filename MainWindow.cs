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
        string [] languageOption = new []
        {
            "Afrikaans", "Albanian", "Arabic", "Azerbaijani", "Bulgarian", "Catalan", "Czech", "Danish", "German", "Greek", "English", "Basque", "Persian (Farsi)", "Finnish", "French", "Galician", "Hebrew", "Hindi", "Croatian", "Hungarian", "Indonesian", "Italian", "Japanese", "Korean", "Latvian", "Lithuanian", "Macedonian", "Norwegian", "Dutch", "Polish", "Portuguese", "Português Brasil", "Romanian", "Russian", "Swedish", "Slovak", "Slovenian", "Spanish", "Serbian", "Thai", "Turkish", "Ukrainian", "Vietnamese", "Chinese Simplified", "Chinese Traditional", "Zulu"
        };
        string [] languageAPI = new string[] {
            "af", "al", "ar", "az", "bg", "ca", "cz", "da", "de", "el", "en", "eu", "fa", "fi", "fr", "gl", "he", "hi", "hr", "hu", "id", "it", "ja", "kr", "la", "lt", "mk", "no", "nl", "pl", "pt", "pt_br", "ro", "ru", "sv", "sk", "sl", "sp", "sr", "th", "tr", "ua", "vi", "zh_cn", "zh_tw", "zu"
        };
        string API_KEY = System.Environment.GetEnvironmentVariable("API_KEY");
        [UI] private Image home_image_Image = null;
        [UI] private Entry home_entry_Entry = null;
        [UI] private Label home__label1_Label = null;
        [UI] private Label home__label2_Label = null;
        [UI] private Label home__label3_Label = null;
        [UI] private Label home__label4_Label = null;
        [UI] private Label home__label5_Label = null;
        [UI] private Label home__label6_Label = null;
        [UI] private Label home__label7_Label = null;
        [UI] private Button home__button1_Button = null;

        [UI] private Window main_window = null;
        [UI] private Label fivedays_name_label = null;
        [UI] private Label fivedays_coord_label = null;
        [UI] private Label fivedays_temp_label1 = null;
        [UI] private Label fivedays_temp_label2 = null;
        [UI] private Label fivedays_temp_label3 = null;
        [UI] private Label fivedays_temp_label4 = null;
        [UI] private Label fivedays_temp_label5 = null;
        [UI] private Label fivedays_date_label1 = null;
        [UI] private Label fivedays_date_label2 = null;
        [UI] private Label fivedays_date_label3 = null;
        [UI] private Label fivedays_date_label4 = null;
        [UI] private Label fivedays_date_label5 = null;
        [UI] private Label fivedays_desc_label1 = null;
        [UI] private Label fivedays_desc_label2 = null;
        [UI] private Label fivedays_desc_label3 = null;
        [UI] private Label fivedays_desc_label4 = null;
        [UI] private Label fivedays_desc_label5 = null;
        [UI] private Label fivedays_humid_label1 = null;
        [UI] private Label fivedays_humid_label2 = null;
        [UI] private Label fivedays_humid_label3 = null;
        [UI] private Label fivedays_humid_label4 = null;
        [UI] private Label fivedays_humid_label5 = null;
        [UI] private Image fivedays_img_image1 = null;
        [UI] private Image fivedays_img_image2 = null;
        [UI] private Image fivedays_img_image3 = null;
        [UI] private Image fivedays_img_image4 = null;
        [UI] private Image fivedays_img_image5 = null;

		[UI] private Label settings_resultCity_Label = null;
		[UI] private Entry settings_city_Entry = null;
		[UI] private Button settings_confirm_Button = null;
		[UI] private Label settings_resultLanguage_Label = null;
		[UI] private Entry settings_confirmLanguage_Entry = null;
		[UI] private Button settings_confirmLanguage_Button = null;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("main_window"))
        {
            builder.Autoconnect(this);
			Check();
            Gtk.CssProvider provider = new CssProvider();
            provider.LoadFromPath("style.css");
            Gtk.StyleContext.AddProviderForScreen(Gdk.Screen.Default,provider,800);
            main_window.StyleContext.AddClass("bg"); ////// Exemple pour appliquer du css sur un élément /////////
            DeleteEvent += Window_DeleteEvent;
            home_entry_Entry.Text = "a";
			home__button1_Button.Clicked += home_button1_Button_Clicked;
			settings_confirm_Button.Clicked += settings_confirm_button_Clicked;
			settings_confirmLanguage_Button.Clicked += settings_confirmLanguage_button_Clicked;
        }
        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private async void ForecastFiveDays(object sender, EventArgs a,string lat,string lon)
        {
            string html = string.Empty;
            var client = new HttpClient();
            html = await client.GetStringAsync(String.Format("https://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&units=metric&lang=fr&appid={2}", lat, lon, API_KEY));
            var result = JsonConvert.DeserializeObject<Item>(html);
            for(int i=0;i<(result.list).Count;)
            {
                if((result.list[i].dt_txt.Hour) != 12)
                {
                    result.list.RemoveAt(i);
                }else{
                    i++;
                }
            }
            
            var fivedays_date_array = new[] { fivedays_date_label1,fivedays_date_label2,fivedays_date_label3,fivedays_date_label4,fivedays_date_label5 };
            var fivedays_temp_array = new[] { fivedays_temp_label1,fivedays_temp_label2,fivedays_temp_label3,fivedays_temp_label4,fivedays_temp_label5 };
            var fivedays_img_array = new[] { fivedays_img_image1,fivedays_img_image2,fivedays_img_image3,fivedays_img_image4,fivedays_img_image5 };
            var fivedays_desc_array = new[] { fivedays_desc_label1,fivedays_desc_label2,fivedays_desc_label3,fivedays_desc_label4,fivedays_desc_label5} ;
            var fivedays_humid_array = new[] { fivedays_humid_label1,fivedays_humid_label2,fivedays_humid_label3,fivedays_humid_label4,fivedays_humid_label5 };
            
            fivedays_name_label.Text = result.city.name;
            fivedays_coord_label.Text = "lat: "+result.city.coord.lat +" lon: "+result.city.coord.lon;
            
            for(int k=0;k<(result.list).Count;k++)
            {
                fivedays_date_array[k].Text = (result.list[k].dt_txt).ToString("MMMM dd hh:00")+"pm";
                fivedays_temp_array[k].Text = (result.list[k].main.temp+"°C");
                fivedays_desc_array[k].Text = (result.list[k].weather[0].description);
                fivedays_humid_array[k].Text = ("humidity: "+result.list[k].main.humidity+"%");
                fivedays_img_array[k].Pixbuf = new Gdk.Pixbuf ("./img/"+result.list[k].weather[0].icon+".png");
            }
            return;
             
        }

        private async void home_button1_Button_Clicked(object sender, EventArgs a)
        {   
            String LAng = "";
            string json = File.ReadAllText("options.json");
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            String variable = jsonObj["LanguageDefault"];
            for (int k= 0; k<languageOption.Length; k++)
            {
                if (languageOption[k] == variable)
                    {
                        LAng = languageAPI[k];
                        break;
                    }
            }
            home__label1_Label.Text = "";
            home__label2_Label.Text = "";
            home__label3_Label.Text = "";
            home__label4_Label.Text = "";
            home__label5_Label.Text = "";
            home__label6_Label.Text = "";
            home__label7_Label.Text = "";
            home_image_Image.Pixbuf = null;
            String city = home_entry_Entry.Text;
            for (int m = 0; m< city.Length; m++)
            {
                if (city[m] == '0' || city[m] == '1' || city[m] == '2' || city[m] == '3' || city[m] == '4' || city[m] == '5' || city[m] == '6' || city[m] == '7' || city[m] == '8' || city[m] == '9')
            {
                home__label1_Label.Text= "non-existent city";
                return; 
            }
            }
            using var client = new HttpClient();
            var content = "";
            String first = String.Format("http://api.openweathermap.org/geo/1.0/direct?q={0}&limit=5&appid={1}", city, API_KEY);
            content = await client.GetStringAsync(first);
            if (content.Length == 2)
            {
                home__label1_Label.Text= "non-existent city";
                return; 
            }
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
                if (content[i]=='f' && content[i+1]== 'r' && testify == false) // à changer avec le .machin
                {
                    int u =i+5;
                    for (int j = u; j < content.Length; j++)
                    {
                        if (content[j] == '"')
                        {
                            NeededInformation += "City: " + name ;
                            testify = true;
                            home__label1_Label.Text = NeededInformation;
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
                            home__label2_Label.Text = NeededInformation;
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
                            home__label3_Label.Text = NeededInformation;
                            NeededInformation = "";
                            break;
                        }
                        lon += content[j];
                    }
                }
            }
            ForecastFiveDays(sender,a,lat,lon);
            String second = String.Format("https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units=metric&appid={2}&lang={3}", lat, lon, API_KEY,LAng);
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
                            home__label4_Label.Text = NeededInformation;
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
                            //home__label4_Label.Text = icon; // bizarre
                            String picture = String.Format("./img/{0}.png", icon);
                            home_image_Image.Pixbuf = new Gdk.Pixbuf (picture);
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
                            home__label5_Label.Text = NeededInformation;
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
                            home__label6_Label.Text = NeededInformation;
                            NeededInformation = "";
                            break;
                        }
                        hum += otherscontent[j];
                    }
                
                }
            }
		}

		private async void settings_confirm_button_Clicked(object sender, EventArgs a)
		{
			HttpClient webclient = new HttpClient();
			var input = settings_city_Entry.Text;
			var lien = String.Format("http://api.openweathermap.org/geo/1.0/direct?q={0}&limit=5&appid={1}", input,API_KEY);
			try
			{
				var content = await webclient.GetStringAsync(lien);
				var test = JsonConvert.DeserializeObject<ValueOfCities[]>(content);
				var result = test[0].name;
				settings_resultCity_Label.Text = result;
				AddValue();
			}
			catch
            {
            }
		}

		private void settings_confirmLanguage_button_Clicked(object sender, EventArgs a)
		{
			settings_resultLanguage_Label.Text = settings_confirmLanguage_Entry.Text;
			AddValue();
		}

		private void Check()
		{
			// Checking the existence of the specified
			if (File.Exists("options.json")) {
				string json = File.ReadAllText("options.json");	
				dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
				settings_resultCity_Label.Text = jsonObj["CityDefault"];
				settings_confirmLanguage_Entry.Text = jsonObj["LanguageDefault"];
			}
			else {
				string path = @"options.json";
				using (FileStream fs = File.Create(path));
				string result = "{\x0A \"CityDefault\": \"\",\x0A \"LanguageDefault\": \"English\"\n}";
				File.WriteAllText(@"options.json", result);
				settings_confirmLanguage_Entry.Text = "English";
			}
		}

		private void AddValue() {
			string json = File.ReadAllText("options.json");
			dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
			jsonObj["CityDefault"] = settings_resultCity_Label.Text;
			jsonObj["LanguageDefault"] = settings_confirmLanguage_Entry.Text;
			string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
			File.WriteAllText("options.json", output);
		}
    }
	
	public class ValueOfCities
	{
		public string name { get; set; }
	    public List<ValueOfCities> objectList { get; set; }
	}

    public class Item
    {
        public List<Info> list;
        public City city;
    }

    public class Info
    {
        public Detailled_info main;
        public List<Detail_weather> weather;
        public DateTime dt_txt;
    }
    public class Detailled_info
    {
        public string temp;
        public string humidity;
    }
    public class Detail_weather
    {
        public string description;
        public string icon;
    }
    public class City
    {
        public string name;
        public Coord coord;
    }
    public class Coord
    {
        public string lat;
        public string lon;
    }

}
