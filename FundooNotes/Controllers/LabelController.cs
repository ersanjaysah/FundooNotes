using BussinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ReposatoryLayer.DBContext;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LabelController : ControllerBase
    {
        FundooContext fundooContext;
        ILabelBL labelBL;

        public LabelController(FundooContext fundooContext, ILabelBL labelBL)
        {
            this.fundooContext = fundooContext;
            this.labelBL = labelBL;
        }

        /// <summary>
        /// this metod used for adding label
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="noteId"></param>
        /// <param name="labelName"></param>
        /// <returns></returns>
        [HttpPost("AddLabel/{userId}/{noteId}/{labelName}")]
        public async Task<ActionResult> AddLabel(int userId, int noteId, string labelName)
        {
            try
            {
                await this.labelBL.AddLabel(userId, noteId, labelName);
                return this.Ok(new { success = true, message = "Label Added Successfully" });

            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
    }
}
