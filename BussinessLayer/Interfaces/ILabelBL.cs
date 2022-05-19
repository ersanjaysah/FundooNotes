using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Interfaces
{
    public interface ILabelBL
    {
        Task AddLabel(int userID, int noteID, string labelName);
    }
}
