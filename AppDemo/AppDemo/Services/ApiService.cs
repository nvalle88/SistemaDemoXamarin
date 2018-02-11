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
        public async Task<Response> InsertarMulta(Multa multa)
        {
            try
            {
                var request = JsonConvert.SerializeObject(multa);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "/api/Multas/InsertarMulta";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al Insertar la multa",
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var multar = JsonConvert.DeserializeObject<Multa>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Multa Registrada  Ok",
                    Result = multar,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
                throw;
            }
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
        internal async void VerificarAuto(string placa, string plaza)
        {
            try
            {
                var placaRequest = new PlacaRequest
                {
                    Placa = placa,
                    Plaza = plaza,
                    AgenteId = navigationService.GetAgenteActual().Id,
                };
                var carro = new Carro
                {
                    Placa = placa.Replace("-", "").ToUpper(),
                };
                var request = JsonConvert.SerializeObject(placaRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "/api/Parqueos/BuscarPlaca";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    await dialogService.ShowMessage("Información", "El vehículo posee tiempo de parqueo...");
                    return;
                }

                var request2 = JsonConvert.SerializeObject(carro);
                var content2 = new StringContent(request2, Encoding.UTF8, "application/json");
                var url2 = "/api/Carroes/GetCarroByPlaca";
                var response2 = await client.PostAsync(url2, content2);

                if (!response2.IsSuccessStatusCode)
                {
                    await dialogService.ShowMessage("Información", "La placa del vehiculo no se encuentra en nuestra base de datos");
                    return;
                }

                var result = await response2.Content.ReadAsStringAsync();
                var VehiculoData = JsonConvert.DeserializeObject<CarroRequest>(result);

                var a = MainViewModel.GetInstance();
                a.NuevaMulta.Placa = placa;
                a.NuevaMulta.Carro = VehiculoData;

                await navigationService.Navigate("PonerMulta");
                return;
            }
            catch (Exception)
            {
                return;
            }
        }
        public async Task<Response> UpdatePasword(UsuarioPasswordRequest usuario)
        {
            try
            {
                var request = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "/api/Agentes/PasswordUpdate";
                var response = await client.PutAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = true,
                        Message = "Un Error ha ocurrido, verifique su contraseña",
                        Result = null,
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                // var usuarioupdate = JsonConvert.DeserializeObject<Usuario>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Petición establecida con exito",
                    Result = null,
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<Multa>> loadVehiculosMultados(string usuarioId)
        {
            try
            {
                var agenteRequest = new AgenteRequest
                {
                    AgenteId = usuarioId,
                };

                var request = JsonConvert.SerializeObject(agenteRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "/api/Multas/GetMultas";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                var multas = JsonConvert.DeserializeObject<List<Multa>>(result);

                return multas;
            }
            catch (Exception)
            {
                return null;
                throw;
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
        public async Task<List<TipoMultas>> loadTipoDeMultas()
        {
            try
            {
                var empresa = new Empresa
                {
                    EmpresaId = Settings.companyId,
                };

                var request = JsonConvert.SerializeObject(empresa);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "/api/TiposMultas/GetTiposMultasPorEmpresa";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                var multas = JsonConvert.DeserializeObject<List<TipoMultas>>(result);

                return multas;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
    }
}