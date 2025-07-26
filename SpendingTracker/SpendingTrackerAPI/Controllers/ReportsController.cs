using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SpendingTrackerDAL;

namespace SpendingTrackerAPI.Controllers
{

    [Route("api/Reports")]
    [ApiController]

    public class ReportsController : ControllerBase
    {



        [HttpPost(Name = "AddNewReport")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<clsReportDTO> AddNewReport(clsReportDTO newReportDTO)
        {
            if (newReportDTO == null || string.IsNullOrEmpty(newReportDTO.Place))
            {
                return BadRequest("Invalid Report data.");
            }

            SpendingTrackerBLL.clsReport Report = new SpendingTrackerBLL.clsReport(new clsReportDTO(newReportDTO.ReportID, newReportDTO.Place,newReportDTO.Amount));
            Report.Save();

            newReportDTO.ReportID = Report.ReportID;
            return CreatedAtRoute("GetReportByID", new { id = newReportDTO.ReportID }, newReportDTO);
        }


        [HttpGet("{id}", Name = "GetReportByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<clsReportDTO>> GetReportByID(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            SpendingTrackerBLL.clsReport Report = SpendingTrackerBLL.clsReport.Find(id);

            if (Report == null)
            {
                return NotFound($"Report with ID {id} not found.");
            }

            clsReportDTO rDTO = Report.rDTO;

            return Ok(Report);
        }

        [HttpGet("All", Name = "GetAllReports")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<clsReportDTO>> GetAllReports()
        {

            List<clsReportDTO> ReportsList = SpendingTrackerBLL.clsReport.GetAllReports();
            if (ReportsList.Count == 0)
            {
                return NotFound("No Reports Found!");
            }
            return Ok(ReportsList);

        }


        [HttpPut("{id}", Name = "UpdateReport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<clsReportDTO> UpdateReport(int id, clsReportDTO updatedReport)
        {
            if (id < 1 || updatedReport == null)
            {
                return BadRequest("Invalid Report data.");
            }

            SpendingTrackerBLL.clsReport Report = SpendingTrackerBLL.clsReport.Find(id);

            if (Report == null)
            {
                return NotFound($"Report with ID {id} not found.");
            }


            Report.Amount = updatedReport.Amount;
            

            if (Report.Save())
                return Ok(Report.rDTO);
            else
                return StatusCode(500, new { message = "Error Updating Report" });
        }


        [HttpDelete("{id}", Name = "DeleteReport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteReport(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            if (SpendingTrackerBLL.clsReport.DeleteReport(id))
                return Ok($"Report with ID {id} has been deleted");
            else
                return NotFound($"Report with ID {id} not found. no rows deleted");
        }


    }
}
