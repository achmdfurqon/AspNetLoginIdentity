using ASP.NETMVCLoginIdentity.ViewModels;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAPI.Models;

namespace ASP.NETMVCLoginIdentity.Controllers
{
    public class RolesController : Controller
    {
        //    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //    // GET: Roles
        //    public ActionResult Index()
        //    {
        //        return View(GetDataRoles());
        //    }

        //    public async Task<ActionResult> GetDataRoles()
        //    {
        //        var result = await connection.QueryAsync<RoleVM>("exec SP_Retrieve_Role");
        //        return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //    }

        //    public async Task<ActionResult> Create(RoleVM role)
        //    {
        //        var affectedRows = await connection.ExecuteAsync("exec SP_Insert_Role @name", new { name=role.Name});
        //        return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
        //    }

        //    public async Task<ActionResult> Detail(int id)
        //    {
        //        var result = await connection.QueryAsync<RoleVM>("exec SP_Get_Role_by_Id @id", new { id = id });
        //        return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //    }

        //    public async Task<ActionResult> Edit(RoleVM role)
        //    {
        //        var affectedRows = await connection.ExecuteAsync("exec SP_Update_Role @id, @name", new { id = role.Id, name = role.Name });
        //        return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
        //    }

        //    public async Task<ActionResult> Delete(int id)
        //    {
        //        var affectedRows = await connection.ExecuteAsync("exec SP_Delete_Role @id", new { id = id });
        //        return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
        //    }

        readonly HttpClient client = new HttpClient();
        public RolesController()
        {
            client.BaseAddress = new Uri("http://localhost:60853/API/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ActionResult Index()
        {
            return View(List());
        }

        public JsonResult List()
        {
            IEnumerable<RoleModels> role = null;
            var responseTask = client.GetAsync("Role");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<RoleModels>>();
                readTask.Wait();
                role = readTask.Result;
            }
            else
            {
                role = Enumerable.Empty<RoleModels>();
                ModelState.AddModelError(string.Empty, "404 Not Found");
            }
            return Json(new { data = role },JsonRequestBehavior.AllowGet);
            
        }

        public JsonResult Create(RoleModels role)
        {
            var context = JsonConvert.SerializeObject(role);
            var buffer = System.Text.Encoding.UTF8.GetBytes(context);
            var byteContext = new ByteArrayContent(buffer);
            byteContext.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var postTask = client.PostAsync("Role/", byteContext).Result;
            return Json(new { data = postTask }, JsonRequestBehavior.AllowGet);            
        }

        public JsonResult Edit(int id, RoleModels role)
        {
            var context = JsonConvert.SerializeObject(role);
            var buffer = System.Text.Encoding.UTF8.GetBytes(context);
            var byteContext = new ByteArrayContent(buffer);
            byteContext.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var putTask = client.PutAsync("Role/" + id, byteContext).Result;
            return Json(new { data = putTask }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            var deleteTask = client.DeleteAsync("Role/" + id);
            deleteTask.Wait();
            return Json(new { data = deleteTask }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Detail(int id)
        {
            var responseTask = client.GetAsync("Role/" + id).Result.Content.ReadAsAsync<RoleModels>().Result;
            
            return Json(new { data = responseTask }, JsonRequestBehavior.AllowGet);
        }
    }
}