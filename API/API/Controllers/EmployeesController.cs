using API.Base;
using API.Context;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private EmployeeRepository employeeRepository;

        private readonly MyContext context;

        public IConfiguration _configuration;
        public EmployeesController(EmployeeRepository employeeRepository,  MyContext context, IConfiguration configuration) : base(employeeRepository)
        {
            this.employeeRepository = employeeRepository;
            this.context = context;
            this._configuration = configuration;
        }

        [HttpPost]
        [Route("Register")]

        public ActionResult Register(RegisterVM registerVM)
        {
            var result = employeeRepository.Register(registerVM);
            if (result == 1)
            {
                return Ok(new { status = HttpStatusCode.OK, result, message = "Berhasil Register" });
            }
            else if (result == 2)
            {
                return Ok(new { status = HttpStatusCode.OK, result, message = "Nik sama, Register Gagal" });
            }
            else if (result == 3)
            {
                return Ok(new { status = HttpStatusCode.OK, result, message = "Phone sama, Register Gagal" });
            }
            else if (result == 4)
            {
                return Ok(new { status = HttpStatusCode.OK, result, message = "Email sama, Register Gagal" });
            }
            else
            {
                return Ok(new { status = HttpStatusCode.OK, result, message = "Register Gagal" });
            }
        }

        [Authorize(Roles = "Manager, Director")]
        [HttpGet]
        [Route("Profile")]
        /*[EnableCors("AllowOrigin")]*/

        public ActionResult<RegisterVM> GetProfile()
        {
            var getProfile = employeeRepository.GetProfile();

            if (getProfile != null)
            {
                return Ok(new { status = HttpStatusCode.OK, getProfile, messsage = "Data ditampilkan" });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.OK, getProfile, message = "Tidak ada data tampil" });
            }
        }

        [HttpGet]
        [Route("Profile/{NIK}")]

        public ActionResult<RegisterVM> GetProfileNIK(string NIK)
        {
            var getProfileNIK = employeeRepository.GetProfileNIK(NIK);
            if (getProfileNIK != null)
            {
                return Ok(new { status = HttpStatusCode.OK, result = getProfileNIK, messsage = $"Data ditampilkan dengan NIK {NIK}" });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.OK, result = getProfileNIK, message = $"Tidak ada data tampil dengan NIK {NIK}" });
            }
        }

        [HttpPost]
        [Route("Login")]

        public ActionResult LoginAutentifikasi(LoginVM loginVM)
        {
            var result = employeeRepository.LoginAutentifikasi(loginVM);
            if (result == 2)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Email salah, tidak bisa login" });
            }
            else if (result == 3)
            {
                return Ok(new { status = HttpStatusCode.OK, result, message = "Berhasil Login" });
            }
            else 
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Login gagal" });
            }
        }

        /*[Authorize(Roles = "Director")]*/
        [HttpPost]
        [Route("SignManager")]

        public ActionResult Sign(LoginVM loginVM)
        {
            var ceks = employeeRepository.Sign(loginVM);
            if (ceks == 2)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "gagal login, email tidak ditemukan" });
            }
            if (ceks == 0)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "gagal login, password salah" });
            }
            else
            {
                var getRoles = employeeRepository.GetRole(loginVM.Email);

                var data = new LoginDataVM()
                {
                    Email = loginVM.Email,
                };
                var claims = new List<Claim>
                {
                    new Claim("Email", data.Email),
                  
                };

                foreach (var a in getRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, a.ToString()));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(10),
                            signingCredentials: signIn
                            );
                var idToken = new JwtSecurityTokenHandler().WriteToken(token);
                claims.Add(new Claim("TokenSecurity", idToken.ToString()));

                return Ok(new { status = HttpStatusCode.OK, idToken, message = "Selamat berhasil login" });

            }  
        }

        [Authorize]
        [HttpGet("TestJWT")]

        public ActionResult TestJWT()
        {
            return Ok("Test JWT Berhasil");
        }

        [Authorize(Roles = "Director")]
        [HttpPost]
        [Route("AddManager")]
        public ActionResult AddManager(AccountRole accountRole)
        {
            var result = employeeRepository.AddManager(accountRole);
            if (result == 1)
            {
                return Ok(new { status = HttpStatusCode.OK, messsage = "Data berhasil ditambah" });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data gagal di tambah" });
            }
        }
    }
}
