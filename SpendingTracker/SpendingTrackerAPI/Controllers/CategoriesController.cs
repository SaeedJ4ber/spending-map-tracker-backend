using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpendingTrackerDAL;

namespace SpendingTrackerAPI.Controllers
{
    [Route("api/Categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CategoryDTO>> GetAllCategories()
        {
           

            List<CategoryDTO> CategoriesList = SpendingTrackerBLL.clsCategory.GetAllCategories();
            if (CategoriesList.Count == 0)
            {
                return NotFound("No Categories Found!");
            }
            return Ok(CategoriesList);

        }


        [HttpGet("{id}", Name = "GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CategoryDTO>> GetCategoryById(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            SpendingTrackerBLL.clsCategory category = SpendingTrackerBLL.clsCategory.Find(id);

            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            CategoryDTO cDTO = category.cDTO;

            return Ok(category);
        }



        [HttpPost(Name = "AddCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CategoryDTO> AddCategory(CategoryDTO newCategoryDTO)
        {
            if (newCategoryDTO == null || string.IsNullOrEmpty(newCategoryDTO.CategoryName))
            {
                return BadRequest("Invalid category data.");
            }

            SpendingTrackerBLL.clsCategory category = new SpendingTrackerBLL.clsCategory(new CategoryDTO(newCategoryDTO.Id, newCategoryDTO.CategoryName));
            category.Save();

            newCategoryDTO.Id = category.ID;
            return CreatedAtRoute("GetCategoryById", new { id = newCategoryDTO.Id }, newCategoryDTO);
        }


        [HttpPut("{id}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<CategoryDTO> UpdateCategory(int id, CategoryDTO updatedCategory)
        {
            if (id < 1 || updatedCategory == null || string.IsNullOrEmpty(updatedCategory.CategoryName))
            {
                return BadRequest("Invalid category data.");
            }

            SpendingTrackerBLL.clsCategory category = SpendingTrackerBLL.clsCategory.Find(id);

            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            category.CategoryName = updatedCategory.CategoryName;   
           

            if (category.Save())
                return Ok(category.cDTO);
            else
                return StatusCode(500, new { message = "Error Updating Category" });
        }

        [HttpDelete("{id}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteCategory(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            if (SpendingTrackerBLL.clsCategory.DeleteCategory(id))
                return Ok($"Category with ID {id} has been deleted");
            else
                return NotFound($"Category with ID {id} not found. no rows deleted");
        }


    }
}
