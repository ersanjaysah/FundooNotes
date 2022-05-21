using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReposatoryLayer.DBContext;
using ReposatoryLayer.Entities;
using ReposatoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryLayer.Services
{
    public class LabelRL:ILabelRL
    {
        FundooContext fundoo; // used field here
        public IConfiguration Configuration { get; }
        public LabelRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundoo = fundooContext;
            this.Configuration = configuration;
        }

        public async Task AddLabel(int userID, int noteID, string labelName)
        {

            try
            {
                Labels label = new Labels();
                label.Userid = userID;
                label.NoteID = noteID;
                label.LabelName = labelName;
                fundoo.Add(label);
               await fundoo.SaveChangesAsync();
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
                List<Labels> reuslt =await fundoo.labels.Where(u => u.Userid == userId).ToListAsync();
                return reuslt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<Labels>> GetlabelByNoteId(int userId,int NoteId)
        {
            try
            {
                List<Labels> reuslt = await fundoo.labels.Where(u => u.NoteID == NoteId && u.Userid==userId).Include(u => u.user).Include(u => u.note).ToListAsync();
                return reuslt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Labels> UpdateLabel(int userID, int LabelId, string LabelName)
        {
            try
            {
                var reuslt = fundoo.labels.FirstOrDefault(u => u.LabelId == LabelId && u.Userid == userID);

                if (reuslt != null)
                {
                    reuslt.LabelName = LabelName;
                    await fundoo.SaveChangesAsync();
                    var result = fundoo.labels.Where(u => u.LabelId == LabelId).FirstOrDefaultAsync();
                    return reuslt;
                }

                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public async Task DeleteLabel(int labelId, int userId)
        {
            try
            {
                var result = fundoo.labels.FirstOrDefault(u => u.LabelId == labelId && u.Userid == userId);
                fundoo.labels.Remove(result);
                await fundoo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

      

    }
    
}
        


    
    

