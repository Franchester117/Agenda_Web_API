using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Parcial.Dependency;
using Parcial.Droid.DependencyService;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(MensajeAndroid))]
namespace Parcial.Droid.DependencyService
{    
    public class MensajeAndroid : InterfazMensaje
    {
        string InterfazMensaje.GetMensaje(string m)
        {
            return m;
        }
    }
}