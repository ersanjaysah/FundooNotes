using BussinessLayer.Interfaces;
using ReposatoryLayer.Entities;
using ReposatoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Services
{
    public class LabelBL:ILabelBL
    {
        ILabelRL labelRL;
        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }

        public async Task AddLabel(int userID, int noteID, string labelName)
        {
            try
            {
               await this.labelRL.AddLabel(userID,noteID,labelName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            

        }

        //public async Task<List<Labels>> GetLabelByNoteId(int noteId)
        //{
        //    try
        //    {
        //      return await this.labelRL.GetlabelByNoteId(noteId);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
            
        //}

        public async Task<List<Labels>> GetLabelByNoteId(int userId, int noteId)
        {
            try
            {
                return await this.labelRL.GetlabelByNoteId(userId,noteId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<Labels>> GetLabelByuserId(int userId)
        {
            try
            {
               return await this.labelRL.GetLabelByuserId(userId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
