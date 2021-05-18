
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using UI.Models.Common;
using UI.Models.Staff;

namespace UI.Helpers
{
    public class SessionHelper
    {
        static public void AddString(HttpRequest request, string key, string value)
        {
            request.HttpContext.Session.SetString(key, value);
        }

        static public string GetString(HttpRequest request, string key)
        {
            return request.HttpContext.Session.GetString(key);
        }

        static public StaffSession GetStaff(HttpRequest request)
        {
            var value = request.HttpContext.Session.GetString("Staff");
            return value == null ? default(StaffSession) : JsonConvert.DeserializeObject<StaffSession>(value);
        }

        static public void SetStaff(HttpRequest request, StaffSession staff)
        {
            request.HttpContext.Session.SetString("Staff", JsonConvert.SerializeObject(staff));
        }

        //static public List<Business.Role> GetRoles(HttpRequest request)
        //{
        //    var value = request.HttpContext.Session.GetString("Roles");
        //    return value == null ? default(List<Business.Role>) : JsonConvert.DeserializeObject<List<Business.Role>>(value);
        //}

        //static public void SetRoles(HttpRequest request, List<Business.Role> roles)
        //{
        //    request.HttpContext.Session.SetString("Roles", JsonConvert.SerializeObject(roles));
        //}

        //static public bool IsSuperAdmin(HttpRequest request)
        //{
        //    var roles = GetRoles(request);
        //    if (roles.Where(x => x.RoleCode == "SUPER").Count() > 0)
        //        return true;
        //    else
        //        return false;
        //}

        //static public bool IsGroupAdmin(HttpRequest request)
        //{
        //    var roles = GetRoles(request);
        //    if (roles.Where(x => x.RoleCode == "GRPADMN").Count() > 0)
        //        return true;
        //    else
        //        return false;
        //}

        //static public bool IsCompanyAdmin(HttpRequest request)
        //{
        //    var roles = GetRoles(request);
        //    if (roles.Where(x => x.RoleCode == "CMPSDMN").Count() > 0)
        //        return true;
        //    else
        //        return false;
        //}

        static public void SetStaffForCompare(HttpRequest request, Staff staff)
        {
            request.HttpContext.Session.SetString("Staff", JsonConvert.SerializeObject(staff));
        }
        static public Staff GetStaffForCompare(HttpRequest request)
        {
            var value = request.HttpContext.Session.GetString("Staff");
            return value == null ? default(Staff) : JsonConvert.DeserializeObject<Staff>(value);
        }
    }
}
