using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SpendingTrackerDAL;

namespace SpendingTrackerAPI.Controllers
{

    [Route("api/Expenses")]
    [ApiController]

    public class ExpensesController : ControllerBase
    {



        [HttpPost(Name = "AddNewExpense")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<clsExpenseDTO> AddNewExpense(clsExpenseDTO newExpenseDTO)
        {
            if (newExpenseDTO == null || string.IsNullOrEmpty(newExpenseDTO.Location))
            {
                return BadRequest("Invalid expense data.");
            }

            SpendingTrackerBLL.clsExpense expense = new SpendingTrackerBLL.clsExpense(new clsExpenseDTO(newExpenseDTO.ExpenseID, newExpenseDTO.Amount, newExpenseDTO.Location , newExpenseDTO.Merchant, newExpenseDTO.Category, newExpenseDTO.DateTime));
            expense.Save();

            newExpenseDTO.ExpenseID = expense.ExpenseID;
            return CreatedAtRoute("GetExpenseByID", new { id = newExpenseDTO.ExpenseID }, newExpenseDTO);
        }


        [HttpGet("{id}", Name = "GetExpenseByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<clsExpenseDTO>> GetExpenseByID(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            SpendingTrackerBLL.clsExpense expense = SpendingTrackerBLL.clsExpense.Find(id);

            if (expense == null)
            {
                return NotFound($"Expense with ID {id} not found.");
            }

            clsExpenseDTO eDTO = expense.eDTO;

            return Ok(expense);
        }

        [HttpGet("All", Name = "GetAllExpenses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<clsExpenseDTO>> GetAllExpenses()
        {

            List<clsExpenseDTO> ExpensesList = SpendingTrackerBLL.clsExpense.GetAllExpenses();
            if (ExpensesList.Count == 0)
            {
                return NotFound("No Expenses Found!");
            }
            return Ok(ExpensesList);

        }


        [HttpPut("{id}", Name = "UpdateExpense")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<clsExpenseDTO> UpdateExpense(int id, clsExpenseDTO updatedExpense)
        {
            if (id < 1 || updatedExpense == null)
            {
                return BadRequest("Invalid expense data.");
            }

            SpendingTrackerBLL.clsExpense expense = SpendingTrackerBLL.clsExpense.Find(id);

            if (expense == null)
            {
                return NotFound($"Expense with ID {id} not found.");
            }

           
            expense.Amount = updatedExpense.Amount;
            expense.Location = updatedExpense.Location;
            expense.DateTime = updatedExpense.DateTime;

            if (expense.Save())
                return Ok(expense.eDTO);
            else
                return StatusCode(500, new { message = "Error Updating Expense" });
        }


        [HttpDelete("{id}", Name = "DeleteExpense")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteExpense(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            if (SpendingTrackerBLL.clsExpense.DeleteExpense(id))
                return Ok($"Expense with ID {id} has been deleted");
            else
                return NotFound($"Expense with ID {id} not found. no rows deleted");
        }


    }
}
