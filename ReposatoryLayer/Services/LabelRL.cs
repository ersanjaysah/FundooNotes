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
    }
}
