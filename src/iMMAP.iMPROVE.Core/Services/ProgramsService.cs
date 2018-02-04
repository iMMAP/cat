using iMMAP.iMPROVE.Identity;
using iMMAP.iMPROVE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xod;
using System.Collections;

namespace iMMAP.iMPROVE.Core.Services
{
    public interface IProgramsService
    {
        List<RegionalOffice> GetRegionalOffices();
        RegionalOffice GetRegionalOffice(Guid id);

        RegionalOfficeProgram GetProgram(Guid id);
        List<RegionalOfficeProgram> GetProgramsByRegionalOffice(Guid officeId);
        List<RegionalOfficeProgram> GetPrograms();

        void AddRegionalOffice(RegionalOffice office);
        void AddProgram(RegionalOfficeProgram program);

        void UpdateRegionalOffice(RegionalOffice office);
        void UpdateProgram(RegionalOfficeProgram program);

        void DeleteRegionalOffice(Guid id);
        void DeleteProgram(Guid id);
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

        public RegionalOfficeProgram GetProgram(Guid id)
        {
            return db.Data.Find<RegionalOfficeProgram>(s => s.Id == id);
        }

        public List<RegionalOfficeProgram> GetPrograms()
        {
            return db.Data.Select<RegionalOfficeProgram>().ToList();
        }

        public List<RegionalOfficeProgram> GetProgramsByRegionalOffice(Guid officeId)
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

        public void DeleteProgram(Guid id)
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

        public RegionalOffice GetRegionalOffice(Guid id)
        {
            return db.Data.Find<RegionalOffice>(s => s.Id == id);
        }

        public void UpdateRegionalOffice(RegionalOffice office)
        {
            db.Data.Update(office);
        }

        public void DeleteRegionalOffice(Guid id)
        {
            if (db.Data.Find<RegionalOfficeProgram>(s => s.ParentOfficeId == id) == null)
            {
                db.Data.Delete<RegionalOffice>(new RegionalOffice() { Id = id });
            }
        }
    }
}