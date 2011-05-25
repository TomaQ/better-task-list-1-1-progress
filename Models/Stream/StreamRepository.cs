using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterTaskList.Models
{

    public class StreamRepository
    {
        private BetterTaskListDataContext db = new BetterTaskListDataContext();

        public IEnumerable<Stream> GetStream(Guid userId)
        {
            // pull the user id of the currently logged in user
            //var uid = (from r in db.Users where r.LoweredUserName.Equals(userName) select r.UserId).SingleOrDefault();

            // grab the list of his friends
            var listOfFriends = (from r in db.CoWorkers where r.UserId.Equals(userId) && r.AreFriends.Equals(true) select r.CoWorkerUserId);

            // return all streams that his friends or him/her has created
            return (from r in db.Streams where listOfFriends.Contains(r.StreamCreatorUserId) || r.StreamCreatorUserId.Equals(userId) orderby r.StreamLastUpdatedTimeStamp descending select r).AsEnumerable();
        }

        public IEnumerable<Stream> GetStream(string userName)
        {
            // pull the user id of the currently logged in user
            var uid = (from r in db.Users where r.LoweredUserName.Equals(userName) select r.UserId).SingleOrDefault();

            // grab the list of his friends
            var listOfFriends = (from r in db.CoWorkers where r.UserId.Equals(uid) && r.AreFriends.Equals(true) select r.CoWorkerUserId);

            // return all streams that his friends or him/her has created
            return (from r in db.Streams where listOfFriends.Contains(r.StreamCreatorUserId) || r.StreamCreatorUserId.Equals(uid) orderby r.StreamLastUpdatedTimeStamp descending select r).AsEnumerable();
        }

        public IEnumerable<Stream> GetStatusStream(string userName)
        {
            // pull the user id of the currently logged in user
            var uid = (from r in db.Users where r.LoweredUserName.Equals(userName) select r.UserId).SingleOrDefault();

            // grab the list of his friends
            var listOfFriends = (from r in db.CoWorkers where r.UserId.Equals(uid) && r.AreFriends.Equals(true) select r.CoWorkerUserId);

            // return all streams that his friends or him/her has created
            return (from r in db.Streams where listOfFriends.Contains(r.StreamCreatorUserId) || r.StreamCreatorUserId.Equals(uid) && r.StreamType.Equals("STATUS") orderby r.StreamLastUpdatedTimeStamp descending select r).AsEnumerable();
        }

        public IEnumerable<Stream> GetWallPostStream(Guid userId, string userName)
        {
            // pull the user id of the currently logged in user
            var uid = (from r in db.Users where r.LoweredUserName.Equals(userName) select r.UserId).SingleOrDefault();

            // grab the list of his friends
            var listOfFriends = (from r in db.CoWorkers where r.UserId.Equals(uid) && r.AreFriends.Equals(true) select r.CoWorkerUserId);

            // grab all the streams that are targetted at this userId
            var listOfStreams = (from r in db.Streams where r.StreamTargetUserId.Equals(userId) && r.StreamType.Equals("WALLPOST") orderby r.StreamLastUpdatedTimeStamp descending select r);

            // grab the streams that I created or that my friends have created
            var results = (from r in listOfStreams where listOfFriends.Contains(r.StreamCreatorUserId) || r.StreamCreatorUserId.Equals(uid) select r);

            return results;

        }

        public Stream GetStream(long streamId)
        {
            return (from r in db.Streams where r.StreamId.Equals(streamId) select r).SingleOrDefault();
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

        public List<Guid> GetStatusCommentators(long streamId)
        {
            // use distinct because a user could have commented more then once on a single status
            return (from r in db.StreamComments where r.StreamId.Equals(streamId) select r.StreamCommentSubmitterUserId).Distinct().ToList();
        }

        public List<string> GetStatusCommentatorsEmailAddresses(long streamId)
        {
            // use distinct because a user could have commented more then once on a single status
            var listOfCommentators = (from r in db.StreamComments where r.StreamId.Equals(streamId) select r.StreamCommentSubmitterUserId).Distinct().ToList();

            // based on the listOfCommentators above grab their pertaining email address
            var listOfEmailAddresses = (from r in db.Users where listOfCommentators.Contains(r.UserId) select r.LoweredUserName).ToList();
            return listOfEmailAddresses;
        }

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