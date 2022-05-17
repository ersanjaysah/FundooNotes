using DataBaseLayer.Notes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Interfaces
{
    public interface INoteBL
    {
        Task AddNote(NotesPostModel notesPostModel, int UserID);
    }
}
