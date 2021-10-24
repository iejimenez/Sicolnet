using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Sicolnet.Models.BD;
using Sicolnet.Utils;
using System;
using System.Text;

namespace Sicolnet.Test
{
    public class Tests
    {
        public SicolnetDBContext dbcontext;
        [SetUp]
        public void Setup()
        {

            var optionsBuilder = new DbContextOptionsBuilder<SicolnetDBContext>();
            optionsBuilder.UseSqlServer(Encoding.UTF8.GetString(Convert.FromBase64String("RGF0YSBTb3VyY2U9MTQ0LjkxLjk0LjEyMztJbml0aWFsIENhdGFsb2c9U2ljb2xuZXQ7UGVyc2lzdCBTZWN1cml0eSBJbmZvPVRydWU7VXNlciBJRD1GaXNjbztQYXNzd29yZD1GaXNjbzIwMTkrO011bHRpcGxlQWN0aXZlUmVzdWx0U2V0cz1UcnVl")));
            dbcontext = new SicolnetDBContext(optionsBuilder.Options);
        }

        [Test]
        public void ExistenDepartamentos()
        {

            Assert.GreaterOrEqual(dbcontext.GetDepartamentos().Count, 1);
        }

        [Test]
        public void ExistenMunicipios()
        {
            Assert.GreaterOrEqual(dbcontext.GetMunicipios().Count, 1);
        } 

        
    }
}