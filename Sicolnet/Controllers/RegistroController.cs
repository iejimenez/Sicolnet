using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sicolnet.Models.BD;
using Sicolnet.Models.Dtos;
using Sicolnet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sicolnet.Controllers
{
    public class RegistroController : Controller
    {
        IMapper _mapper;
        SicolnetDBContext dBContext;

        public RegistroController(IMapper mapper, SicolnetDBContext context)
        {
            _mapper = mapper;
            dBContext = context;
        }
        public IActionResult Index(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                ViewBag.IdReferente = "";
            else
                ViewBag.IdReferente = Encriptador.DesEncriptar(Encoding.UTF8.GetString(Convert.FromBase64String(Id)));
            return View();
        }

        public IActionResult Persona()
        {
            return View();
        }
        public JsonResult Save(PersonaDto persona, string token)
            
        {
            if (string.IsNullOrEmpty(token))
                throw new Exception("Petición invalida");

            AjaxData retorno = new AjaxData();
            try
            {
                if (persona.IdReferente == 0)
                    persona.IdReferente = 1;

                if (token != "NVD")
                {
                    Token tokendb = dBContext.Tokens.Where(t => t.Celular == persona.Celular && t.Cedula == persona.Cedula).FirstOrDefault();
                    if (tokendb != null)
                    {
                        if (tokendb.Key.ToString() == token)
                        {
                            dBContext.Tokens.Remove(tokendb);
                            dBContext.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("Token invalido");
                        }
                    }
                    else
                    {
                        throw new Exception("Petición invalida");
                    }
                }
                Persona personaDB = dBContext.Personas.Where(p => p.Cedula == persona.Cedula).FirstOrDefault();

                if (personaDB != null)
                    throw new Exception("Esa cedula ya se encuentra registrada.");

                persona.IdEstado = 1;
                persona.FechaRegistro = DateTime.Now;
                persona.FechaUltimaModificacion = DateTime.Now;

                dBContext.InsertarPersona(_mapper.Map<Persona>(persona));
            }
            catch (Exception ex)
            {
                retorno.Msj = ex.Message;
                retorno.Is_Error = true;
            }

            return Json(retorno);
        }

        public JsonResult GenerarToken(string celular, string cedula)
        {
            AjaxData retorno = new AjaxData();
            try
            {

                Persona personaDB = dBContext.Personas.Where(p => p.Cedula == cedula).FirstOrDefault();

                if (personaDB != null)
                    throw new Exception("Esa cedula ya se encuentra registrada.");

                Token token = dBContext.Tokens.Where(t => t.Celular == celular && t.Cedula == cedula).FirstOrDefault();
                if(token!= null)
                {
                    dBContext.Tokens.Remove(token);
                }

                Random rdm = new Random();
                int n = rdm.Next(100000, 1000000);
                Token NewToken = new Token()
                {
                    Celular = celular,
                    Cedula = cedula,
                    FechaRegistro = DateTime.Now,
                    Key = n
                };

                dBContext.Tokens.Add(NewToken);
                dBContext.SaveChanges();
                SmsSender.SendSMS(celular, "Su codigo de verificación de registro es " + n.ToString());
                retorno.Objeto = NewToken;
            }
            catch (Exception ex)
            {
                retorno.Msj = ex.Message;
                retorno.Is_Error = true;
            }

            return Json(retorno);
        }

        public JsonResult ConsutarPersona(string cedula)
        {
            AjaxData retorno = new AjaxData();
            try
            {

                if (string.IsNullOrEmpty(cedula))
                    throw new Exception("Petición invalida.");


                //PersonaDto p =  _mapper.Map<PersonaDto>(dBContext.Personas.Where(p => p.Cedula == cedula.Trim()).FirstOrDefault());

                PersonaDto p = DbHelper.RawSqlQuery<PersonaDto>(@"SELECT  [IdPersona] ,[Cedula] ,[Nombres] ,[Apellidos] ,[Celular] ,[Email] 
                                ,[IdMunicipio],[FechaNacimiento] ,[IdReferente] ,[IdEstado] ,[FechaRegistro],[FechaUltimaModificacion]
                                ,[ShortUrl] ,[ShortUrlToken]FROM[dbo].[Personas] where Cedula ='" + cedula + "'", x=> 
                new PersonaDto()
                {
                    IdPersona = (int)(x[0]),
                    Cedula = (string)x[1],
                    Nombres = (string)x[2],
                    Apellidos = (string)x[3],
                    Celular = (string)x[4],
                    Email = (string)x[5],
                    IdMunicipio = (int)x[6],
                    FechaNacimiento = (DateTime)x[7],
                    IdReferente =(int)x[8],
                    ShortUrl = (string)x[12],
                    ShortUrlToken = (string)x[13]
                }, dBContext).FirstOrDefault();


                if (p!= null)
                {
                    Token token = dBContext.Tokens.Where(t => t.Celular == p.Celular && t.Cedula == p.Cedula).FirstOrDefault();
                    if (token != null)
                    {
                        dBContext.Tokens.Remove(token);
                    }

                    Random rdm = new Random();
                    int n = rdm.Next(100000, 1000000);
                    Token NewToken = new Token()
                    {
                        Celular = p.Celular,
                        Cedula = p.Cedula,
                        FechaRegistro = DateTime.Now,
                        Key = n
                    };

                    dBContext.Tokens.Add(NewToken);
                    dBContext.SaveChanges();
                    SmsSender.SendSMS(p.Celular, "Su codigo de verificación de registro es " + n.ToString());
                    retorno.Objeto = NewToken;

                }
                else
                {
                    retorno.Msj = "Cedula no encontrada.";
                    retorno.Is_Error = true;
                    retorno.Order_Switch = -1;
                }
            }
            catch (Exception ex)
            {
                retorno.Msj = ex.Message;
                retorno.Is_Error = true;
            }

            return Json(retorno);
        }

        public async Task<JsonResult> ConsultarArbolPersona(string cedula, string token)
        {
            AjaxData retorno = new AjaxData();
            try
            {
                if (string.IsNullOrEmpty(cedula))
                    throw new Exception("Petición invalida.");

                if (string.IsNullOrEmpty(token))
                    throw new Exception("Petición invalida.");

                Persona dboPersona = dBContext.Personas.Where(p => p.Cedula == cedula.Trim()).FirstOrDefault();
                PersonaDto p = _mapper.Map<PersonaDto>(dboPersona);
                
                Token tokendb = dBContext.Tokens.Where(t => t.Celular == p.Celular && t.Cedula == p.Cedula).FirstOrDefault();
                if (tokendb != null)
                {
                    if (tokendb.Key.ToString() == token)
                    {
                        dBContext.Tokens.Remove(tokendb);
                        dBContext.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Token invalido");
                    }
                }
                else if (token != "NVD")
                    throw new Exception("Token invalido");

                if (string.IsNullOrEmpty(p.ShortUrl))
                {
                    string Urldomain = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
                    string id = Convert.ToBase64String(Encoding.UTF8.GetBytes(Encriptador.Encriptar(p.IdPersona.ToString())));
                    UrlShorterResponse resultShorter = await UrlShorter.ShortUrl(Urldomain + "/Registro/Index?Id=" + id);
                    if (resultShorter.error == null)
                    {
                        dboPersona.ShortUrlToken = resultShorter.data.token;
                        dboPersona.ShortUrl = resultShorter.data.short_url;
                        p.ShortUrl = dboPersona.ShortUrl;
                        p.ShortUrlToken = dboPersona.ShortUrlToken;
                        dBContext.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("No se pudo generar el link. Err: " + JsonConvert.SerializeObject(resultShorter.error));
                    }
                }

                List<PersonaDto> amigos = InvitadosDirectos(p.IdPersona).OrderByDescending(p=>p.NumeroInvitados).ToList();
                int contarAmigos = amigos.Count + amigos.Sum(p => p.NumeroInvitados);
                retorno.Objeto = new
                {
                    NumeroAmigos = contarAmigos,
                    Persona = p,
                    Amigos = amigos.OrderBy(s=>s.Nombres).ToList()
                };
            }
            catch (Exception ex)
            {
                retorno.Msj = ex.Message;
                retorno.Is_Error = true;
            }
            var plainTextBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(retorno.Objeto));
            Response.Cookies.Append("appData", Convert.ToBase64String(plainTextBytes));
            return Json(retorno);

            //string url = "whatsapp://send?text=https://www.anerbarrena.com/boton-compartir-whatsapp-4801/";
        }

        private List<PersonaDto> InvitadosDirectos(int idPersona)
        {
            List<PersonaDto> personas = _mapper.Map<List<PersonaDto>>(dBContext.Personas.Where(p => p.IdReferente == idPersona && p.IdPersona != idPersona).ToList());
            int contar = 0;
            foreach (PersonaDto p in personas)
            {
                contar += 1;
                p.NumeroInvitados = ContarRamas(p.IdPersona);
                contar += p.NumeroInvitados;
            }
            return personas;
        }

        private int ContarRamas(int idPersona)
        {
            int contar = 0;
            List<PersonaDto> personas = _mapper.Map<List<PersonaDto>>(dBContext.Personas.Where(p => p.IdReferente == idPersona && p.IdPersona != idPersona).ToList());
            foreach(PersonaDto p in personas)
            {
                contar += 1;
                contar += ContarRamas(p.IdPersona); 
            }
            return contar;
        }
    }
}
