using BussinessLayer.Interfaces;
using DataBaseLayer.Notes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using ReposatoryLayer.DBContext;
using ReposatoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        //Fields
        FundooContext fundooContext;
        INoteBL noteBL;
        private readonly IDistributedCache distributedCache;
        private readonly IMemoryCache memoryCache;

        // constructor
        public NoteController(FundooContext fundoo, INoteBL noteBL, IDistributedCache distributedCache, IMemoryCache memoryCache)
        {
            this.fundooContext = fundoo;
            this.noteBL = noteBL;
            this.distributedCache = distributedCache;
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// Add Notes
        /// </summary>
        /// <param name="notesPostModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("AddNote")]
        public async Task<ActionResult> AddUser(NotesPostModel notesPostModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                await this.noteBL.AddNote(notesPostModel, userId);

                return this.Ok(new { success = true, message = $"Note Added Successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update Notes
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="noteUpdateModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("Update/{noteId}")]
        public async Task<ActionResult> UpdateNote(int noteId, NoteUpdateModel noteUpdateModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);

                var note = fundooContext.Notes.FirstOrDefault(u => u.Userid == UserId && u.NoteID == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = " Sorry!!! Failed to Update note" });
                }
                await this.noteBL.UpdateNote(UserId, noteId, noteUpdateModel);
                return this.Ok(new { success = true, message = "Note Updated successfully!!!" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Delete the Notes
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("Delete/{noteId}")]
        public async Task<ActionResult> DeleteNote(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.Userid == UserId && u.NoteID == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Oops!! This note is not available" });

                }
                
                await this.noteBL.DeleteNote(noteId, UserId);
                return this.Ok(new { success = true, message = "Note Deleted Successfully" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Change colour of the Notes
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="colour"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("ChangeColour/{noteId}/{colour}")]
        public async Task<ActionResult> ChangeColour(int noteId, string colour)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);

                var note = fundooContext.Notes.FirstOrDefault(u => u.Userid == UserId && u.NoteID == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Sorry!!! Note does not exist" });
                }
                //if (note.IsTrash == true)
                //{
                //    return this.BadRequest(new { success = false, message = "sorry!! Note Already deleted, please create new note" });
                //}

                await this.noteBL.ChangeColour(UserId, noteId, colour);
                return this.Ok(new { success = true, message = "Note Colour Changed Successfully " });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Add Archive in Notes
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("ArchiveNote/{noteId}")]
        public async Task<ActionResult> IsArchieveNote(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);

                var note = fundooContext.Notes.FirstOrDefault(u => u.Userid == userId && u.NoteID == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = " Sorry !!! Failed to archieve notes" });
                }
                //if (note.IsTrash == true)
                //{
                //    return this.BadRequest(new { success = false, message = "sorry!! Note has been deleted, please create new note" });
                //}
                await this.noteBL.ArchiveNote(userId, noteId);
                return this.Ok(new { success = true, message = "Note Archieved successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///Add Remainder in Note
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="remainder"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("remainderNote/{noteId}/{remainder}")]
        public async Task<ActionResult> RemainderNote(int noteId, DateTime remainder)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);

                var note = fundooContext.Notes.FirstOrDefault(u => u.Userid == userId && u.NoteID == noteId);
                if (note == null)
                {
                     return this.BadRequest(new { success = false, message = "Sorry !! Note does't Exist" });
                }
                if (note.IsTrash == true)
                {
                    return this.BadRequest(new { success = false, message = "sorry!! Note has been deleted, please create new note" });
                }
               
                await this.noteBL.Remainder(userId, noteId, remainder);
                return this.Ok(new { success = true, message = "Remainder Sets Successfully!!!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Trash method created
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("Trash/{noteId}")]
        public async Task<ActionResult> IsTrash(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);

                var note = fundooContext.Notes.FirstOrDefault(u => u.Userid == userId && u.NoteID  == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = " Sorry!!! Failed to Trash Note" });
                }
                //if (note.IsTrash == true)
                //{
                //    return this.BadRequest(new { success = false, message = "sorry!! Note has been deleted, please create new note" });
                //}
                await this.noteBL.Trash(userId, noteId);
                return this.Ok(new { success = true, message = "Trash added successfully!!!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Pin method added in notes
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("Pin/{noteId}")]
        public async Task<ActionResult> IsPin(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);

                var note = fundooContext.Notes.FirstOrDefault(u => u.Userid == userId && u.NoteID == noteId);

                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = " Sorry!!! Failed to Pin note" });
                }
                if (note.IsTrash == true)
                {
                    return this.BadRequest(new { success = false, message = "sorry!! Note has been deleted, please create new note" });
                }
                await this.noteBL.Pin(userId, noteId);
                return this.Ok(new { success = true, message = "Pin Added successfully!!!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get All notes
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetAllNotes")]
        public async Task<ActionResult> GetAllNotes()
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                List<Note> result = new List<Note>();
                result = await this.noteBL.GetAllNotes(userId);
                return this.Ok(new {success=true,message=$"Here is your all Notes", data=result});

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Using Redis Cache
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetAllNotesByRedis")]
        public async Task<ActionResult> GetAllNotesUsingRedisCache()
         {
            try
            {
                string key = "NotesList";
                string serializedNoteList;
                var noteList = new List<Note>();
                var redisNoteList = await distributedCache.GetAsync(key);
                if (redisNoteList != null)
                {

                    serializedNoteList = Encoding.UTF8.GetString(redisNoteList);
                    noteList = JsonConvert.DeserializeObject<List<Note>>(serializedNoteList);
                }
                else
                {
                    var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                    int userId = Int32.Parse(userid.Value);
                    noteList = await this.noteBL.GetAllNotes(userId);
                    serializedNoteList = JsonConvert.SerializeObject(noteList);
                    redisNoteList = Encoding.UTF8.GetBytes(serializedNoteList);
                    var option = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20)).SetAbsoluteExpiration(TimeSpan.FromHours(6));
                    await distributedCache.SetAsync(key, redisNoteList, option);

                }
                return this.Ok(new { success = true, message = $"Get all Notes Successfully Fetch", Data = noteList });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
