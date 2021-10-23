using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

                PersonaDto p =  _mapper.Map<PersonaDto>(dBContext.Personas.Where(p => p.Cedula == cedula.Trim()).FirstOrDefault());

                if(p!= null)
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

        public JsonResult ConsultarArbolPersona(string cedula, string token)
        {
            AjaxData retorno = new AjaxData();
            try
            {
                if (string.IsNullOrEmpty(cedula))
                    throw new Exception("Petición invalida.");

                if (string.IsNullOrEmpty(token))
                    throw new Exception("Petición invalida.");

                PersonaDto p = _mapper.Map<PersonaDto>(dBContext.Personas.Where(p => p.Cedula == cedula.Trim()).FirstOrDefault());

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
                int contarAmigos = ContarRamas(p.IdPersona);
                retorno.Objeto = new
                {
                    IdEnlace = Convert.ToBase64String(Encoding.UTF8.GetBytes(Encriptador.Encriptar(p.IdPersona.ToString()))),
                    NumeroAmigos = contarAmigos,
                    Persona = p
                };
            }
            catch (Exception ex)
            {
                retorno.Msj = ex.Message;
                retorno.Is_Error = true;
            }
            return Json(retorno);

            //string url = "whatsapp://send?text=https://www.anerbarrena.com/boton-compartir-whatsapp-4801/";
        }

        private int ContarRamas(int idPersona)
        {
            int contar = 1;
            List<PersonaDto> personas = _mapper.Map<List<PersonaDto>>(dBContext.Personas.Where(p => p.IdReferente == idPersona && p.IdPersona != idPersona).ToList());
            foreach(PersonaDto p in personas)
            {
                contar += ContarRamas(p.IdPersona); 
            }
            return contar;
        }
    }
}
