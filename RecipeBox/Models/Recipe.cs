using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Models
{
  public class Recipe
  {
    public int RecipeId { get; set; }
    [Required(ErrorMessage = "Please enter a valid name.")]
    public string Name { get; set; }
    public string Ingredients { get; set; }
    public string Instructions { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
    public List<RecipeTag> JoinEntities { get; }
  }
}