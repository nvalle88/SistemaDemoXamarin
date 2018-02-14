using AppDemo.Classes;
using AppDemo.Helpers;
using AppDemo.Models;
using AppDemo.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
/// <summary>
/// En el APISERVICE se realizan los metodos que nos facilitan el consumo de el api,
/// estos metodos son tareas asincronas ya que dependemos de la respuesta del servicio en la nube
/// </summary>
namespace AppDemo.Services
{
    public class ApiService
    {
        DialogService dialogService;
        NavigationService navigationService;
        string URL_ws = Constants.Constants.WebServiceURL;

        public ApiService()
        {
            dialogService = new DialogService();
            navigationService = new NavigationService();
        }

        public async Task<Response> Login()
        {          
                var user = new Agente { Id =1,
                Nombre= "Nestor"};
                return new Response
                {
                    IsSuccess = true,
                    Message = "Login Ok",
                    Result = user,
                };                      
        }
        #region cliente
        public async Task<Response> SetPhotoAsync(int multaId, Stream stream)
        {
            try
            {
                var array = ReadFully(stream);

                var photoRequest = new PhotoRequest
                {
                    Id = multaId,
                    Array = array,
                };

                var request = JsonConvert.SerializeObject(photoRequest);
                var body = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "/api/Multas/SetFoto";
                var response = await client.PostAsync(url, body);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Foto asignada Ok",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);


                return ms.ToArray();
            }
        }
        #endregion
      public async Task <List<Cliente>> GetAllClients()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "/api/Clientes";
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var result = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<List<Cliente>>(result);
                return clientes;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Cliente>> GetNearClients(Helpers.GeoUtils.Position position)
        {
            try
            {
                var request = JsonConvert.SerializeObject(position);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "/api/Clientes/GetNearClients";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var result = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<List<Cliente>>(result);
                return clientes;
                //  var log = JsonConvert.DeserializeObject<LogPosition>(result);            

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<ObservableCollection<PinRequest>> GetParqueados()
        {
            try
            {
                var request = JsonConvert.SerializeObject(navigationService.GetAgenteActual());
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "/api/Parqueos/GetParqueados";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                var parqueados = JsonConvert.DeserializeObject<ObservableCollection<PinRequest>>(result);
                return parqueados;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public async Task<Response> postNewClient(Cliente cliente)
        {

            try
            {
                
                var request = JsonConvert.SerializeObject(cliente);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "/api/Clientes";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    new Response
                    {
                        IsSuccess = false,
                        Message = "error",

                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                var cliente_ = JsonConvert.DeserializeObject<Cliente>(result);
                
                return new Response
                {
                    IsSuccess=true,
                    Message="Ok",
                    Result=cliente_
                };
            }
            catch (Exception)
            {
                return null;
                throw;
            }


        }
        public async Task<List<PinRequest>> Getparking()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "/api/Parqueos/GetParqueados";
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var result = await response.Content.ReadAsStringAsync();
                var parqueos = JsonConvert.DeserializeObject<List<PinRequest>>(result);
                return parqueos;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<Position>> GetMyPolygon(int _agenteId)
        {
            try
            {
                Agente _agente = new Agente { Id = _agenteId };
                var request = JsonConvert.SerializeObject(_agente);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "/api/Sectors/GetMyPolygon";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    List<Position> position = new List<Position>();
                    return position;
                }
                var result = await response.Content.ReadAsStringAsync();
                var PuntoSector = JsonConvert.DeserializeObject<List<PuntoSector>>(result);
                List<Position> MyPolygon = new List<Position>();
                foreach (var punto in PuntoSector)
                MyPolygon.Add(new Position(punto.Latitud, punto.Longitud));
                return MyPolygon;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public async Task PostLogPosition(LogPosition position )
        {         
            try
            {                
                var request = JsonConvert.SerializeObject(position);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "/api/LogPositions";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return;
                }
                var result = await response.Content.ReadAsStringAsync();
                return;
              //  var log = JsonConvert.DeserializeObject<LogPosition>(result);            
               
            }
            catch(Exception ex)
            {
                return;
            }
          

        }
    }
}