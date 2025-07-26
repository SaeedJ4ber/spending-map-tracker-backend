using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SpendingTrackerDAL;

namespace SpendingTrackerAPI.Controllers
{

    [Route("api/Merchants")]
    [ApiController]

    public class MerchantsController : ControllerBase
    {



        [HttpPost(Name = "AddNewMerchant")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<clsMerchantDTO> AddNewMerchant(clsMerchantDTO newMerchantDTO)
        {
            if (newMerchantDTO == null || string.IsNullOrEmpty(newMerchantDTO.Name) || string.IsNullOrEmpty(newMerchantDTO.Location))
            {
                return BadRequest("Invalid Merchant data.");
            }

            SpendingTrackerBLL.clsMerchant Merchant = new SpendingTrackerBLL.clsMerchant(new clsMerchantDTO(newMerchantDTO.MerchantID, newMerchantDTO.Name, newMerchantDTO.Location));
            Merchant.Save();

            newMerchantDTO.MerchantID = Merchant.MerchantID;
            return CreatedAtRoute("GetMerchantByID", new { MerchantID = newMerchantDTO.MerchantID }, newMerchantDTO);
        }


        [HttpGet("{id}", Name = "GetMerchantByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<clsMerchantDTO>> GetMerchantByID(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            SpendingTrackerBLL.clsMerchant Merchant = SpendingTrackerBLL.clsMerchant.Find(id);

            if (Merchant == null)
            {
                return NotFound($"Merchant with ID {id} not found.");
            }

            clsMerchantDTO eDTO = Merchant.mDTO;

            return Ok(Merchant);
        }

        [HttpGet("All", Name = "GetAllMerchants")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<clsMerchantDTO>> GetAllMerchants()
        {

            List<clsMerchantDTO> MerchantsList = SpendingTrackerBLL.clsMerchant.GetAllMerchants();
            if (MerchantsList.Count == 0)
            {
                return NotFound("No Merchants Found!");
            }
            return Ok(MerchantsList);

        }


        [HttpPut("{id}", Name = "UpdateMerchant")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<clsMerchantDTO> UpdateMerchant(int id, clsMerchantDTO updatedMerchant)
        {
            if (id < 1 || updatedMerchant == null || string.IsNullOrEmpty(updatedMerchant.Name))
            {
                return BadRequest("Invalid Merchant data.");
            }

            SpendingTrackerBLL.clsMerchant Merchant = SpendingTrackerBLL.clsMerchant.Find(id);

            if (Merchant == null)
            {
                return NotFound($"Merchant with ID {id} not found.");
            }

            Merchant.Name = updatedMerchant.Name;
            Merchant.Location = updatedMerchant.Location;

            if (Merchant.Save())
                return Ok(Merchant.mDTO);
            else
                return StatusCode(500, new { message = "Error Updating Merchant" });
        }


        [HttpDelete("{id}", Name = "DeleteMerchant")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteMerchant(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            if (SpendingTrackerBLL.clsMerchant.DeleteMerchant(id))
                return Ok($"Merchant with ID {id} has been deleted");
            else
                return NotFound($"Merchant with ID {id} not found. no rows deleted");
        }


    }
}
