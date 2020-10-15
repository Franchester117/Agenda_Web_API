using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parcial.Modelo
{
    public class ContactoAPI
    {        
       
        
        public string Id { get; set; }       
        public string firstname { get; set; }        
        public string lastname { get; set; }        
        public string phoneNumber { get; set; }        
        public string email { get; set; }

        public ContactoAPI()
        {
            
        }

        public ContactoAPI(String id, string firstname, string lastname, string phoneNumber, string email)
        {
            Id = id;
            this.firstname = firstname;
            this.lastname = lastname;
            this.phoneNumber = phoneNumber;
            this.email = email;
        }   
        

    }
}
