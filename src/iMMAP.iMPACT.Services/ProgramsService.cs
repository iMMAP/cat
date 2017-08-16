using iMMAP.iMPACT.Identity;
using iMMAP.iMPACT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xod;

namespace iMMAP.iMPACT.Services
{
    public interface IProgramsService
    {
        List<RegionalOffice> GetRegionalOffices();
        RegionalOffice GetRegionalOffice(int id);

        RegionalOfficeProgram GetProgram(int id);
        List<RegionalOfficeProgram> GetProgramsByRegionalOffice(int officeId);
        List<RegionalOfficeProgram> GetPrograms();

        void AddRegionalOffice(RegionalOffice office);
        void AddProgram(RegionalOfficeProgram program);

        void UpdateRegionalOffice(RegionalOffice office);
        void UpdateProgram(RegionalOfficeProgram program);

        void DeleteRegionalOffice(int id);
        void DeleteProgram(int id);
    }

    public class ProgramsService : IProgramsService
    {
        private IDataService db = null;
        private IUsersDataService usersDb = null;
        private IMessagesService messagesService = null;

        public ProgramsService(IDataService db, IUsersDataService usersDb, IMessagesService messagesService)
        {
            this.db = db;
            this.usersDb = usersDb;
            this.messagesService = messagesService;
        }

        public void AddRegionalOffice(RegionalOffice office)
        {
            db.Data.Insert(office);
        }

        public RegionalOfficeProgram GetProgram(int id)
        {
            return db.Data.Find<RegionalOfficeProgram>(s => s.Id == id);
        }

        public List<RegionalOfficeProgram> GetPrograms()
        {
            return db.Data.Select<RegionalOfficeProgram>().ToList();
        }

        public List<RegionalOfficeProgram> GetProgramsByRegionalOffice(int officeId)
        {
            return db.Data.Query<RegionalOfficeProgram>(s => s.ParentOfficeId == officeId).ToList();
        }

        public void AddProgram(RegionalOfficeProgram program)
        {
            db.Data.Insert(program);
        }

        public void UpdateProgram(RegionalOfficeProgram program)
        {
            db.Data.Update(program);
        }

        public void DeleteProgram(int id)
        {
            if (db.Data.Find<Product>(s => s.ProgramId == id) == null)
            {
                db.Data.Delete<RegionalOfficeProgram>(new RegionalOfficeProgram() { Id = id });
            }
        }

        public List<RegionalOffice> GetRegionalOffices()
        {
            return db.Data.Select<RegionalOffice>().ToList();
        }

        public RegionalOffice GetRegionalOffice(int id)
        {
            return db.Data.Find<RegionalOffice>(s => s.Id == id);
        }

        public void UpdateRegionalOffice(RegionalOffice office)
        {
            db.Data.Update(office);
        }

        public void DeleteRegionalOffice(int id)
        {
            if (db.Data.Find<RegionalOfficeProgram>(s => s.ParentOfficeId == id) == null)
            {
                db.Data.Delete<RegionalOffice>(new RegionalOffice() { Id = id });
            }
        }
    }
}