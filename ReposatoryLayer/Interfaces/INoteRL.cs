using DataBaseLayer.Notes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryLayer.Interfaces
{
    public interface INoteRL
    {
        Task AddNote(NotesPostModel notesPostModel,int UserID);

    }
}
