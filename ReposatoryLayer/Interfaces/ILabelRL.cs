using ReposatoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryLayer.Interfaces
{
    public interface ILabelRL
    {
        Task AddLabel(int userID, int noteID, string labelName);
        Task<List<Labels>> GetLabelByuserId(int userId);
        Task<List<Labels>> GetlabelByNoteId(int userId, int NoteId);
    }
}
