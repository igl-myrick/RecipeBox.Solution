using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Models
{
  public class Recipe
  {
    public int RecipeId { get; set; }
    [Required(ErrorMessage = "Please enter a valid name.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Please add some ingredients first.")]
    public string Ingredients { get; set; }
    [Required(ErrorMessage = "Please enter some instructions first.")]
    public string Instructions { get; set; }
    public string Rating { get; set; }
    public List<RecipeTag> JoinEntities { get; }
  }
}