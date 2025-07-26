using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpendingTrackerDAL;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace SpendingTrackerAPI.Controllers
{
    [Route("api/Budgets")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllBudgets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<clsBudgetDTO>> GetAllBudgets()
        {
            
            List<clsBudgetDTO> BudgetsList = SpendingTrackerBLL.clsBudget.GetAllBudgets();
            if (BudgetsList.Count == 0)
            {
                return NotFound("No Budgets Found!");
            }
            return Ok(BudgetsList);

        }

        [HttpGet("Valid", Name = "GetValidBudgets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<clsBudgetDTO>> GetValidBudgets()
        {
            var validBudgets = SpendingTrackerBLL.clsBudget.GetValidBudgets();
            return Ok(validBudgets);
        }



        [HttpGet("{id}", Name = "GetBudgetByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<clsBudgetDTO>> GetBudgetByID(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            SpendingTrackerBLL.clsBudget budget = SpendingTrackerBLL.clsBudget.Find(id);

            if (budget == null)
            {
                return NotFound($"Budget with ID {id} not found.");
            }

            clsBudgetDTO bDTO = budget.bDTO;

            return Ok(budget);
        }



        [HttpPost(Name = "AddBudget")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<clsBudgetDTO> AddBudget(clsBudgetDTO newBudgetDTO)
        {
            //if (newBudgetDTO == null || string.IsNullOrEmpty(newBudgetDTO.Name) || newBudgetDTO.DateTimeFrom > DateTime.Now && newBudgetDTO.DateTimeFrom < DateTime.MinValue || newBudgetDTO.DateTimeTo > DateTime.Now)
            if (newBudgetDTO == null || string.IsNullOrEmpty(newBudgetDTO.Name))
            {
                return BadRequest("Invalid budget data.");
            }

            SpendingTrackerBLL.clsBudget budget = new SpendingTrackerBLL.clsBudget(new clsBudgetDTO(newBudgetDTO.BudgetID, newBudgetDTO.Name, newBudgetDTO.Amount, newBudgetDTO.DateTimeFrom, newBudgetDTO.DateTimeTo, newBudgetDTO.BudgetStatus));
            budget.Save();

            newBudgetDTO.BudgetID = budget.BudgetID;
            return CreatedAtRoute("GetBudgetByID", new { id = newBudgetDTO.BudgetID }, newBudgetDTO);
        }

        [HttpPut("{id}", Name = "UpdateBudget")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<clsBudgetDTO> UpdateBudget(int id, clsBudgetDTO updatedBudget)
        {
            if (id < 1 || updatedBudget == null || string.IsNullOrEmpty(updatedBudget.Name))
            {
                return BadRequest("Invalid budget data.");
            }

            SpendingTrackerBLL.clsBudget budget = SpendingTrackerBLL.clsBudget.Find(id);

            if (budget == null)
            {
                return NotFound($"Budget with ID {id} not found.");
            }

            budget.Name = updatedBudget.Name;
            budget.Amount = updatedBudget.Amount;
            budget.DateTimeFrom = updatedBudget.DateTimeFrom;
            budget.DateTimeTo = updatedBudget.DateTimeTo;
            budget.BudgetStatus = updatedBudget.BudgetStatus;


            if (budget.Save())
                return Ok(budget.bDTO);
            else
                return StatusCode(500, new { message = "Error Updating Budget" });
        }

        [HttpDelete("{id}", Name = "DeleteBudget")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteBudget(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            if (SpendingTrackerBLL.clsBudget.DeleteBudget(id))
                return Ok($"Budget with ID {id} has been deleted");
            else
                return NotFound($"Budget with ID {id} not found. no rows deleted");
        }


    }
}
