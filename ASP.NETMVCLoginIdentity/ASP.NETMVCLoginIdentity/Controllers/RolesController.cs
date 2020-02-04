using ASP.NETMVCLoginIdentity.ViewModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ASP.NETMVCLoginIdentity.Controllers
{
    public class RolesController : Controller
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        // GET: Roles
        public ActionResult Index()
        {
            return View(GetDataRoles());
        }

        public async Task<ActionResult> GetDataRoles()
        {
            var result = await connection.QueryAsync<RoleVM>("exec SP_Retrieve_Role");
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Create(RoleVM role)
        {
            var affectedRows = await connection.ExecuteAsync("exec SP_Insert_Role @name", new { name=role.Name});
            return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Detail(int id)
        {
            var result = await connection.QueryAsync<RoleVM>("exec SP_Get_Role_by_Id @id", new { id = id });
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Edit(RoleVM role)
        {
            var affectedRows = await connection.ExecuteAsync("exec SP_Update_Role @id, @name", new { id = role.Id, name = role.Name });
            return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var affectedRows = await connection.ExecuteAsync("exec SP_Delete_Role @id", new { id = id });
            return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
        }
    }
}