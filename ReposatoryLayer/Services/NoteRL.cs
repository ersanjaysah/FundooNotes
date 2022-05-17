using DataBaseLayer.Notes;
using Microsoft.Extensions.Configuration;
using ReposatoryLayer.DBContext;
using ReposatoryLayer.Entities;
using ReposatoryLayer.Interfaces;
using System;
using System.Collections.Generic;
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
            catch
            {

            }
        }
    }
}
