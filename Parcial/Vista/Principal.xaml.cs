using Newtonsoft.Json;
using Parcial.Modelo;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Parcial.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Detalle : ContentPage
    {
        
        public static ListView listaContacto;
        public static string llave;
        public static  string url = "http://10.0.2.2:5000/contacts";
        private static HttpClient cl;        
        private static ObservableCollection<ContactoAPI> observable_contactosAPI;
        public Detalle()
        {
            InitializeComponent();
            ObtenerContactos();
            ConvertirTextoAVoz("Bienvenido a la aplicacion de prueba");           
        }

        //METODOS NUEVOS CON APIS

        //METODO QUE OBTIENE LOS CONTACTOS DE LA BD MONGO DB MEDIANTE UNA API
        public static async void ObtenerContactos()
        {
            cl = new HttpClient();
            string content = await cl.GetStringAsync(url);
            System.Diagnostics.Debug.Write(content);
            List<ContactoAPI> post = JsonConvert.DeserializeObject<List<ContactoAPI>>(content);
            observable_contactosAPI = new ObservableCollection<ContactoAPI>(post);
            ContactosList.ItemsSource = observable_contactosAPI;

            foreach(ContactoAPI a in post)
            {
                System.Diagnostics.Debug.Write(a.Id);
            }
            
            System.Diagnostics.Debug.Write(content);    
        }


        public static async Task<HttpClient> getConection()
        {
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            return cliente;
        }

       
        //METODO PARA SELECCIONAR UN CONTACTO DE LA LISTA Y ACCEDER A LA VISTA SECUNDARIA
        private void ContactosList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var obj = (ContactoAPI)e.SelectedItem;
            System.Diagnostics.Debug.Write(obj+" PRUEBA DE IDENTIFICACION");            
            ConvertirTextoAVoz(obj.firstname);
            ObjetoEstatico.Id = obj.Id;
            ObjetoEstatico.firstname = obj.firstname;
            ObjetoEstatico.lastname = obj.lastname;
            ObjetoEstatico.phoneNumber = obj.phoneNumber;
            ObjetoEstatico.email = obj.email;
            System.Diagnostics.Debug.Write("PRUEBA DE ID: " + obj.Id);
            Navigation.PushAsync(new DetalleContacto(1));

        }

        //METODO PARA HACER UNA BUSQUEDA FILTRADA
        private void BuscarTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var key = BuscarTxt.Text;
            ContactosList.ItemsSource = observable_contactosAPI.Where(
                                x => x.firstname.ToLower().Contains(key.ToLower())
                        );
        }

        //METODO PARA CONVERTIR A VOZ CUALQUIER TEXTO
        public static async void ConvertirTextoAVoz(string text)
        {

            await TextToSpeech.SpeakAsync(text, new SpeechOptions
            {
                Volume = 1.0f
            });
        }
    }
}