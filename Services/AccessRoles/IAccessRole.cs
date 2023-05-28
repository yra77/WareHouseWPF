

using WareHouseWPF.Enums;


namespace WareHouseWPF.Services.AccessRoles
{
    internal interface IAccessRole
    {
        void GetAccessPermission(out bool highest, out bool medium, out bool low, out bool lowest);
        void SetRole(string role);
    }
}
