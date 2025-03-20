using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RecipeBox.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;

namespace RecipeBox.Controllers
{
  public class TagsController : Controller
  {
    private readonly RecipeBoxContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public TagsController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Tags.ToList());
    }

    public ActionResult Details(int id)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      Tag thisTag = _db.Tags
        .Include(tag => tag.JoinEntities.Where(join => join.Recipe != null && join.Recipe.User.Id == userId))
        .ThenInclude(join => join.Recipe)
        .FirstOrDefault(tag => tag.TagId == id);
      return View(thisTag);
    }

    [Authorize]
    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Tag tag)
    {
      if (!ModelState.IsValid)
      {
        return View(tag);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        tag.User = currentUser;
        _db.Tags.Add(tag);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    [Authorize]
    public ActionResult AddRecipe(int id)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      List<Recipe> userRecipes = _db.Recipes
          .Where(entry => entry.User.Id == userId)
          .ToList();
      Tag thisTag = _db.Tags
        .Include(tag => tag.JoinEntities.Where(join => join.Recipe != null && join.Recipe.User.Id == userId))
        .FirstOrDefault(tag => tag.TagId == id);
      ViewBag.RecipeId = new SelectList(userRecipes, "RecipeId", "Name");
      return View(thisTag);
    }

    [HttpPost]
    public ActionResult AddRecipe(Tag tag, int recipeId)
    {
      #nullable enable
      RecipeTag? joinEntity = _db.RecipeTags.FirstOrDefault(join => join.RecipeId == recipeId && join.TagId == tag.TagId);
      #nullable disable
      if (joinEntity == null && recipeId != 0)
      {
        _db.RecipeTags.Add(new RecipeTag() { RecipeId = recipeId, TagId = tag.TagId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = tag.TagId });
    }

    [Authorize]
    public ActionResult Edit(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tag => tag.TagId == id);
      if (thisTag.User != null)
      {
        return View(thisTag);
      }
      else
      {
        return RedirectToAction("Details", new { id = thisTag.TagId });
      }
    }

    [HttpPost]
    public ActionResult Edit(Tag tag)
    {
      if (!ModelState.IsValid)
      {
        return View(tag);
      }
      _db.Tags.Update(tag);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize]
    public ActionResult Delete(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tag => tag.TagId == id);
      if (thisTag.User != null)
      {
        return View(thisTag);
      }
      else
      {
        return RedirectToAction("Details", new { id = thisTag.TagId });
      }
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tag => tag.TagId == id);
      _db.Tags.Remove(thisTag);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      RecipeTag joinEntry = _db.RecipeTags.FirstOrDefault(entry => entry.RecipeTagId == joinId);
      _db.RecipeTags.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}