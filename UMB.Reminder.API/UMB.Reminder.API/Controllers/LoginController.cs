using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UMB.Reminder.API.ViewModel;
using LinqToDB;

namespace UMB.Reminder.API.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        // GET api/values
        [HttpGet]
        public JsonResult Get()
        {
            using (var db = new Reminder())
            {
                var result = db.GetTable<Admin>().Where(a => a.NIK == "" && a.Password == "").FirstOrDefault();
                return Json(result, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            }

        }

        //Check The Valid User Id And Password In the Database
        [HttpPost("validate_login", Name = "ValidateLogin")]
        public JsonResult ValidateLogin(string NIK, string Password)
        {
            AdminViewModel oReturn = new AdminViewModel();
            ApiResult result = new ApiResult();
            try
            {
                using (var db = new Reminder())
                {
                    Admin login = db.GetTable<Admin>().Where(x => x.NIK == NIK && x.Password == Password).FirstOrDefault();
                    if (login == null)
                    {
                        result.Messages = "Username or Password Doesn't Exist";
                        result.Code = "OK";
                        result.Status = false;
                        result.Payload = null;
                    }
                    else
                    {
                        oReturn.IDAdmin = login.IDAdmin;
                        oReturn.NIK = login.NIK;
                        oReturn.NamaAdmin = login.NamaAdmin;
                        oReturn.Kampus = login.Kampus;

                        result.Messages = "Username And Password Match";
                        result.Code = "OK";
                        result.Status = true;
                        result.Payload = oReturn;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Messages = ex.Message;
                result.Code = "Error";
                result.Status = false;
                result.Payload = null;
            }

            return Json(result, new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }

    }
}
