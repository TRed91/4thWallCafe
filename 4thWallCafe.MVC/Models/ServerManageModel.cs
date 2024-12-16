using _4thWallCafe.Core.Entities;
using _4thWallCafe.MVC.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _4thWallCafe.MVC.Models;

public class ServerManageModel
{
    public ServerManageForm Form { get; set; }
    public List<Server> Servers { get; set; }
    public SelectList? OrderBySelectItems { get; set; }
}

public class ServerManageForm
{
    public ServerManageOrderBy OrderBy { get; set; } = ServerManageOrderBy.ServerName;
    public string? SearchString { get; set; } = null;
}