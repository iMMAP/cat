﻿using iMMAP.iMPACT.Identity;
using iMMAP.iMPACT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xod;

namespace iMMAP.iMPACT.Services
{
    public interface IActivitiesService
    {
        List<TrainingSupport> GetTrainingSupportActivities();
        List<TrainingSupport> GetTrainingSupportActivities(string userId);
        List<WorkTrip> GetWorkTripPlans();
        List<WorkTrip> GetWorkTripPlans(string userId);
        TrainingSupport GetTrainingSupportActivity(Guid id);
        WorkTrip GetWorkTripPlan(Guid id);
        TrainingSupport AddTrainingSupportActivity(TrainingSupport product);
        WorkTrip AddWorkTripPlan(WorkTrip product);
        void UpdateTrainingSupportActivity(TrainingSupport activity, string[] fields = null);
        void UpdateWorkTripPlan(WorkTrip activity, string[] fields = null);
        void DeleteTrainingSupportActivity(Guid id);
        void DeleteWorkTripPlan(Guid id);
    }

    public class ActivitiesService : IActivitiesService
    {
        private IDataService db = null;
        private IUsersDataService usersDb = null;
        private IMessagesService messagesService = null;

        public ActivitiesService(IDataService db, IUsersDataService usersDb, IMessagesService messagesService)
        {
            this.db = db;
            this.usersDb = usersDb;
            this.messagesService = messagesService;
        }

        public List<TrainingSupport> GetTrainingSupportActivities()
        {
            return db.Data.Select<TrainingSupport>().ToList();
        }
        public List<TrainingSupport> GetTrainingSupportActivities(string userId)
        {
            return db.Data.Query<TrainingSupport>(s => s.ConductedBy == userId).ToList();
        }
        public TrainingSupport GetTrainingSupportActivity(Guid id)
        {
            return db.Data.Find<TrainingSupport>(s => s.ConductedBy == id);
        }
        public TrainingSupport AddTrainingSupportActivity(TrainingSupport activity)
        {
            return (TrainingSupport)db.Data.Insert(activity);
        }
        public void DeleteTrainingSupportActivity(Guid id)
        {
            db.Data.Delete(new TrainingSupport() { Id = id });
        }
        public void UpdateTrainingSupportActivity(TrainingSupport activity, string[] fields = null)
        {
            if (fields == null)
            {
                db.Data.Update(activity);
            }
            else
            {
                db.Data.Update(activity, new UpdateFilter()
                {
                    Behavior = UpdateFilterBehavior.Target,
                    Properties = fields
                });
            }
        }

        public List<WorkTrip> GetWorkTripPlans()
        {
            return db.Data.Select<WorkTrip>().ToList();
        }
        public List<WorkTrip> GetWorkTripPlans(string userId)
        {
            return db.Data.Query<WorkTrip>(s => s.PlannedBy == userId).ToList();
        }
        public WorkTrip GetWorkTripPlan(Guid id)
        {
            return db.Data.Find<WorkTrip>(s => s.PlannedBy == id);
        }
        public WorkTrip AddWorkTripPlan(WorkTrip activity)
        {
            return (WorkTrip)db.Data.Insert(activity);
        }
        public void DeleteWorkTripPlan(Guid id)
        {
            db.Data.Delete(new WorkTrip() { Id = id });
        }
        public void UpdateWorkTripPlan(WorkTrip activity, string[] fields = null)
        {
            if (fields == null)
            {
                db.Data.Update(activity);
            }
            else
            {
                db.Data.Update(activity, new UpdateFilter()
                {
                    Behavior = UpdateFilterBehavior.Target,
                    Properties = fields
                });
            }
        }
    }
}