using DataBaseLayer.Notes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReposatoryLayer.DBContext;
using ReposatoryLayer.Entities;
using ReposatoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryLayer.Services
{
    public class NoteRL : INoteRL
    {
        FundooContext fundoo; // used field here
        public IConfiguration Configuration { get; }
        public NoteRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundoo = fundooContext;
            this.Configuration=configuration;
        }
        public async Task AddNote(NotesPostModel notesPostModel,int Userid)
        {
            try
            {
                Note note = new Note();
                note.Userid = Userid;
                note.Title = notesPostModel.Title;
                note.Description = notesPostModel.Description;
                note.Color= notesPostModel.Color;
                note.IsArchive = false;
                note.IsRemainder = false;
                note.IsPin= false;
                note.IsTrash = false;
                note.CreatedDate = DateTime.Now;
                note.ModifedDate= DateTime.Now;
                fundoo.Add(note);
                await fundoo.SaveChangesAsync();

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Note> UpdateNote(int userId, int noteId, NoteUpdateModel noteUpdateModel)
        {
            try
            {
                var note= fundoo.Notes.FirstOrDefault(u => u.Userid == userId && u.NoteID == noteId);
                if (note!=null)
                {
                    note.Title = noteUpdateModel.Title;
                    note.Description = noteUpdateModel.Description;
                    note.IsArchive = noteUpdateModel.IsArchive;
                    note.Color = noteUpdateModel.Color;
                    note.IsPin = noteUpdateModel.IsPin;
                    note.IsRemainder = noteUpdateModel.IsRemainder;
                    note.IsTrash = noteUpdateModel.IsTrash;
                    await fundoo.SaveChangesAsync();

                }
                return await fundoo.Notes.Where(u => u.Userid == u.Userid && u.NoteID == noteId).Include(u => u.user).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //deleting the notes
        public async Task DeleteNote(int noteId, int userId)
        {

            try
            {
                var note = fundoo.Notes.FirstOrDefault(u => u.NoteID == noteId && u.Userid == userId);
                if (note != null)
                {
                    fundoo.Notes.Remove(note);
                  await  fundoo.SaveChangesAsync();

                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        // change colour
        public async Task ChangeColour(int userId, int noteId, string color)
        {

            try
            {
                var note = fundoo.Notes.FirstOrDefault(u => u.Userid == userId && u.NoteID == noteId);
                if (note != null)
                {
                    note.Color = color;
                    await fundoo.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task ArchiveNote(int userId, int noteId)
        {

            try
            {
                var note = fundoo.Notes.FirstOrDefault(u => u.Userid == userId && u.NoteID == noteId);
                if (note != null)
                {
                    if (note.IsArchive == true)
                    {
                        note.IsArchive = false;
                    }

                    else
                    {
                        note.IsArchive = true;
                    }
                }

                await fundoo.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task Remainder(int userId, int noteId, DateTime remainder)
        {

            try
            {
                var note = fundoo.Notes.FirstOrDefault(u => u.Userid == userId && u.NoteID == noteId);
                if(note != null)
                {
                    note.IsRemainder = true;
                    note.Remainder = remainder;
                }
                await fundoo.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task Trash(int userId, int noteId)
        {
            try
            {
                var note=fundoo.Notes.FirstOrDefault(u=>u.Userid==userId&& u.NoteID==noteId);
                if (note!=null)
                {
                    if (note.IsTrash==true)
                    {
                        note.IsTrash = false;
                    }
                    else
                    {
                        note.IsTrash = true;
                    }
                }
               await fundoo.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task Pin(int userId, int noteId)
        {
            try
            {
               var note= fundoo.Notes.FirstOrDefault(u => u.Userid == userId && u.NoteID == noteId);
                if (note!=null)
                {
                    if (note.IsPin==true)
                    {
                        note.IsPin = false;
                    }
                    if (note.IsPin == false)
                    {
                        note.IsPin = true;
                    }
                }
               await fundoo.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<Note>> GetAllNotes(int userId)
        {
            try
            {
                return await fundoo.Notes.Where(u => u.Userid == userId).Include(u => u.user).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
