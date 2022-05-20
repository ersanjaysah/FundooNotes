using BussinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReposatoryLayer.DBContext;
using ReposatoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// this method used for adding label
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="noteId"></param>
        /// <param name="labelName"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("AddLabel/{noteId}/{labelName}")]
        public async Task<ActionResult> AddLabel(int noteId, string labelName)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int userID = Int32.Parse(userid.Value);
                //int noteId = Int32.Parse(userid.Value);

                await this.labelBL.AddLabel(userID,noteId,labelName);
                return this.Ok(new { success = true, message = "Lable Added Successfully " });


            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// get label by UserId
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetLabelByuserId/{userId}")]
        public async Task<ActionResult> GetLabelByuserId()
        {
            try
            {
                List<Labels> res = new List<Labels>();
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                res = await this.labelBL.GetLabelByuserId(userId);
                if (res == null)
                {
                    return this.BadRequest(new { success = false, message = " Sorry!!! unable to get label" });
                }
                return this.Ok(new { success = true, message = $" get Label information successfully", data = res });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get label by NoteId
        /// </summary>
        /// <param name="NoteId"></param>
        /// <returns></returns>

        [Authorize]
        [HttpGet("GetlabelByNoteId/{NoteId}")]
        public async Task<ActionResult> GetLabelByNoteId(int NoteId)
        {
            try
            {
                List<Labels> res = new List<Labels>();
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                res = await this.labelBL.GetLabelByNoteId(userId,NoteId);
                if (res == null)
                {
                    return this.BadRequest(new { success = true, message = " Sorry!!! Unable to get label" });
                }
                return this.Ok(new { success = true, message = $" get Label information successfully", data = res });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
