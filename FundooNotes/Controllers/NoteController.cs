using BussinessLayer.Interfaces;
using DataBaseLayer.Notes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReposatoryLayer.DBContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {

        FundooContext fundooContext;
        INoteBL noteBL;

        public NoteController(FundooContext fundoo, INoteBL noteBL)
        {
            this.fundooContext = fundoo;
            this.noteBL = noteBL;
        }
        [Authorize]
        [HttpPost("AddNote")]

        public async Task<ActionResult> AddUser(NotesPostModel notesPostModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                await this.noteBL.AddNote(notesPostModel, userId);

                return this.Ok(new { success = true, message = $"User Added Successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
