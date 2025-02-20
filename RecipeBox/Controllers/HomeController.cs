using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using RecipeBox.Models;

namespace RecipeBox.Controllers
{
  public class HomeController : Controller
  {
    private readonly RecipeBoxContext _db;

    public HomeController(RecipeBoxContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      Recipe[] recipes = _db.Recipes.ToArray();
      Tag[] tags = _db.Tags.ToArray();
      Dictionary<string, object[]> model = new Dictionary<string, object[]>();
      model.Add("recipes", recipes);
      model.Add("tags", tags);
      return View(model);
    }
  }
}