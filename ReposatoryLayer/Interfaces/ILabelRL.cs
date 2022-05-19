using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryLayer.Interfaces
{
    public interface ILabelRL
    {
        Task AddLabel(int userID, int noteID, string labelName);
    }
}
