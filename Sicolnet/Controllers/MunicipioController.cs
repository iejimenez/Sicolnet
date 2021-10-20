using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sicolnet.Models.BD;
using Sicolnet.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sicolnet.Controllers
{
    public class MunicipioController : Controller
    {
        IMapper _mapper;
        SicolnetDBContext dBContext;

        public MunicipioController(IMapper mapper, SicolnetDBContext contenxt) 
        {
            _mapper = mapper;
            dBContext = contenxt;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetMunicipios()
        {
            List<DepartamentoDto> departamentos = _mapper.Map<List<DepartamentoDto>>(dBContext.GetDepartamentos());
            List<MunicipioDto> municipios = _mapper.Map<List<MunicipioDto>>(dBContext.GetMunicipios());
            municipios.ForEach(s => s.Departamento = departamentos.Where(d => d.IdDepartamento == s.IdDepartamento).FirstOrDefault());
            return Json(municipios);
        }
    }
}
