using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RecipeBox.Models;

namespace RecipeBox.Controllers
{
  public class TagsController : Controller
  {
    private readonly RecipeBoxContext _db;

    public TagsController(RecipeBoxContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Tags.ToList());
    }

    public ActionResult Details(int id)
    {
      Tag thisTag = _db.Tags
        .Include(tag => tag.JoinEntities)
        .ThenInclude(join => join.Recipe)
        .FirstOrDefault(tag => tag.TagId == id);
      return View(thisTag);
    }
  }
}