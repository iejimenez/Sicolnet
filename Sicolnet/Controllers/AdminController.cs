using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sicolnet.Models.BD;
using Sicolnet.Models.Dtos;
using Sicolnet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sicolnet.Controllers
{
    public class AdminController : Controller
    {

        IMapper _mapper;
        SicolnetDBContext dBContext;

        public AdminController(IMapper mapper, SicolnetDBContext context)
        {
            _mapper = mapper;
            dBContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetAdmins()
        {
            AjaxData retorno = new AjaxData();
            try
            {
                List<UsuarioDto> admins = _mapper.Map<List<UsuarioDto>>(dBContext.GetAdmins());
                foreach(UsuarioDto usuario in admins) {
                    Persona p = dBContext.Personas.Where(p => p.IdPersona == usuario.IdPersona).FirstOrDefault();
                    usuario.Tercero = _mapper.Map<PersonaDto>(p);
                }
                retorno.Objeto = admins;
            }
            catch (Exception ex)
            {
                retorno.Msj = ex.Message;
                retorno.Is_Error = true;
            }

            return Json(retorno);
        }

        public JsonResult Save(int id)
        {
            AjaxData retorno = new AjaxData();
            try
            {
                if (id <= 0)
                    throw new Exception("Petición invalida");

                Persona personaDB = dBContext.Personas.Where(p => p.IdPersona == id).FirstOrDefault();
             
                if (personaDB == null)
                    throw new Exception("Esa cedula no se encuentra registrada.");

                UsuarioDto nuevoAdmin = new UsuarioDto()
                {
                    IdPersona = id,
                    UserName = personaDB.Cedula,
                    Password = personaDB.Cedula + "@test"
                };

                dBContext.InsertarUsuario(_mapper.Map<Usuario>(nuevoAdmin));
            }
            catch (Exception ex)
            {
                retorno.Msj = ex.Message;
                retorno.Is_Error = true;
            }

            return Json(retorno);
        }

        public JsonResult Delete(int id)
        {


            AjaxData retorno = new AjaxData();
            try
            {
                if (id <= 0)
                    throw new Exception("Petición invalida");

                Usuario usuarioDb = dBContext.Usuarios.Where(p => p.IdUsuario == id).FirstOrDefault();

                if (usuarioDb == null)
                    throw new Exception("Usuario no encontrado.");

                dBContext.Usuarios.Remove(usuarioDb);
                dBContext.SaveChanges();

                retorno.Msj = "OK";
                retorno.Is_Error = false;
            }
            catch (Exception ex)
            {
                retorno.Msj = ex.Message;
                retorno.Is_Error = true;
            }

            return Json(retorno);
        }

    }
}
