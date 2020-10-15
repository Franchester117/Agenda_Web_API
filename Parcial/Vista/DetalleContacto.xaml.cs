using Parcial.Modelo;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parcial.Vista;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using System.Security.Principal;
using Parcial.Dependency;
using System.Net.Http;
using Flurl.Http;
using Newtonsoft.Json;

namespace Parcial.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetalleContacto : ContentPage
    {
        public static Label id;
        public static Entry nombre;
        public static Entry apellido;
        public static Entry telefono;
        public static Entry email;        
        private string IdTxt="";

        public DetalleContacto()
        {
            InitializeComponent();            

            BtnCrear.IsVisible = true;
            BtnActualizar.IsVisible = false;
            BtnEliminar.IsVisible = false;
            estadoVentanaLbl.Text = "Nuevo contacto";
            Detalle.ConvertirTextoAVoz("Accediendo a crear nuevo contacto");
        }
        
       

        //METODO QUE MUESTRA LOS DATOS DEL CONTACTO
        public DetalleContacto(int id)
        {
            InitializeComponent();

            IdTxt = ObjetoEstatico.Id;
            System.Diagnostics.Debug.Write(IdTxt);
            NombresTxt.Text = ObjetoEstatico.firstname;
            ApellidosTxt.Text = ObjetoEstatico.lastname;
            TelefonoTxt.Text = Convert.ToString(ObjetoEstatico.phoneNumber);
            EmailTxt.Text = ObjetoEstatico.email;
            BtnCrear.IsVisible = false;            
        }

        public async Task<ContactoAPI> Post(string id, string firstname, string lastname, string phoneNumber, string email)
        {
            ContactoAPI contact = new ContactoAPI()
            {
                Id="",
                firstname = firstname,
                lastname = lastname,
                phoneNumber = phoneNumber,
                email = email

            };
            HttpClient cliente = await Detalle.getConection();
            var response = await cliente.PostAsync(Detalle.url,
                new StringContent(JsonConvert.SerializeObject(contact), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<ContactoAPI>(await response.Content.ReadAsStringAsync());

        }

        public async Task Update(string id, string firstname, string lastname, string phoneNumber, string email)
        {
            ContactoAPI contact = new ContactoAPI()
            {
                Id = id,
                firstname = firstname,
                lastname = lastname,
                phoneNumber = phoneNumber,
                email = email                
            };
            System.Diagnostics.Debug.Write(id+" HOLA MUNDO");
            HttpClient cliente = await Detalle.getConection();
            await cliente.PutAsync(Detalle.url + "/"+id, new StringContent(JsonConvert.SerializeObject(contact), Encoding.UTF8, "application/json"));
        }

        public async Task Delete(string id)
        {                        
            HttpClient cliente = await Detalle.getConection();
            await cliente.DeleteAsync(Detalle.url + "/" + id);
            var servicioM = DependencyService.Get<InterfazMensaje>().GetMensaje("Contacto eliminado");
            Detalle.ConvertirTextoAVoz(servicioM);
            await DisplayAlert("Mensaje Dependency", servicioM, "Ok");
        }

        private async void EliminarContacto(object sender, EventArgs e)
        {
            await Delete(IdTxt);
            Detalle.ObtenerContactos();
        }

        private  async void ActualizarContacto(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(NombresTxt.Text) || String.IsNullOrEmpty(ApellidosTxt.Text) || String.IsNullOrEmpty(TelefonoTxt.Text) || String.IsNullOrEmpty(EmailTxt.Text))
            {
                var servicioM = DependencyService.Get<InterfazMensaje>().GetMensaje("Los campos están vacios");
                Detalle.ConvertirTextoAVoz(servicioM);
                await DisplayAlert("Mensaje Dependency", servicioM, "Ok");
            }
            else
            {
                await Update(IdTxt, NombresTxt.Text, ApellidosTxt.Text, TelefonoTxt.Text, EmailTxt.Text);
                var servicioM = DependencyService.Get<InterfazMensaje>().GetMensaje("Contacto actualizado");
                Detalle.ConvertirTextoAVoz(servicioM);
                await DisplayAlert("Mensaje Dependency", servicioM, "Ok");
                Detalle.ObtenerContactos();
                
            }
        }

        private async void CrearContacto(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(NombresTxt.Text) || String.IsNullOrEmpty(ApellidosTxt.Text) || String.IsNullOrEmpty(TelefonoTxt.Text) || String.IsNullOrEmpty(EmailTxt.Text))
            {
                var servicioM = DependencyService.Get<InterfazMensaje>().GetMensaje("Los campos están vacios");
                Detalle.ConvertirTextoAVoz(servicioM);
                await DisplayAlert("Mensaje Dependency", servicioM, "Ok");
            }
            else
            {
                var x = await Post("", NombresTxt.Text, ApellidosTxt.Text, TelefonoTxt.Text, EmailTxt.Text);
                var servicioM = DependencyService.Get<InterfazMensaje>().GetMensaje("Contacto creado");
                Detalle.ConvertirTextoAVoz(servicioM);
                await DisplayAlert("Mensaje Dependency", servicioM, "Ok");
                Detalle.ObtenerContactos();

            }
        }





    }
}