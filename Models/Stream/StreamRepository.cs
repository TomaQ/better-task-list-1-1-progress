using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterTaskList.Models
{

    public class StreamRepository
    {
        private BetterTaskListDataContext db = new BetterTaskListDataContext();


        public IEnumerable<Stream> GetStream(string userName)
        {
            // pull the user id of the currently logged in user
            var uid = (from r in db.Users where r.LoweredUserName.Equals(userName) select r.UserId).SingleOrDefault();
            
            // grab the list of his friends
            var listOfFriends = (from r in db.CoWorkers where r.UserId.Equals(uid) && r.AreFriends.Equals(true) select r.CoWorkerUserId);
            
            // return all streams that his friends or him/her has created
            return (from r in db.Streams where listOfFriends.Contains(r.StreamCreatorUserId) || r.StreamCreatorUserId.Equals(uid) orderby r.StreamLastUpdatedTimeStamp descending select r).AsEnumerable();
        }

        public IEnumerable<Stream> GetStream(long streamId)
        {
            return (from r in db.Streams where r.StreamId.Equals(streamId) select r);
        }

        public IEnumerable<StreamComment> GetStreamComments(long streamId)
        {
            return (from r in db.StreamComments where r.StreamId.Equals(streamId) orderby r.StreamCommentTimeStamp ascending select r).AsEnumerable();
        }

        public void Add(Stream stream)
        {
            db.Streams.InsertOnSubmit(stream);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

    }


    public class StreamCommentRepository
    {
        private BetterTaskListDataContext db = new BetterTaskListDataContext();
        
        public void Add(StreamComment streamComment)
        {
           db.StreamComments.InsertOnSubmit(streamComment);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

    }
}