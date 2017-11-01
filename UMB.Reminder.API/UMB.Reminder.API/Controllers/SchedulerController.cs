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
    public class SchedulerController : Controller
    {

        //For Data Tables
        [HttpGet("get_schedule")]
        public JsonResult GetSchedule()
        {
            ApiResult result = new ApiResult();
            List<Kelasprogram> _payload = new List<Kelasprogram>();

            using (var db = new Reminder())
            {
                _payload = db.GetTable<Kelasprogram>().ToList();                

                if (_payload.Any())
                {
                    result.Status = true;
                    result.Payload = _payload.ToList();
                }
                else
                {
                    result.Status = true;
                    result.Code = "DATA_NOT_FOUND";
                    result.Messages = "No data found.";
                }
                return Json(result, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            }
            
        }        

        //For Creating The Kelas Program For Admin
        [HttpPost("create_kelas_program", Name = "CreateKelasProgram")]
        public JsonResult CreateKelasProgram(Kelasprogram model) {
            if(model == null)
            {
                model = new Kelasprogram();
            }
            using (Reminder db = new Reminder())
            {
                try
                {
                    db.BeginTransaction();

                    //Create The Code
                    string subKelasProgram = model.KelasProgramReg.Substring(0, 3);
                    string angkaKelasProgram = model.KelasProgramReg.Substring(model.KelasProgramReg.Length-1, 1);
                    string code = model.TahunAjaran + '-' + model.Semester + '-' + subKelasProgram + angkaKelasProgram;

                    model.KodeKelasProgram = code;

                    //Insert data
                    db.InsertWithIdentity(model);                    
                    db.CommitTransaction();

                    return Json(new ApiResult() { Status = true, Payload = model }, new JsonSerializerSettings() { Formatting = Formatting.Indented });
                }
                catch(Exception ex)
                {
                    db.RollbackTransaction();
                    return Json(new ApiResult() { Status = false, Code = "API_ERROR_ADD", Messages = ex.Message.ToString() });
                }
            }                
        }

    }
}
