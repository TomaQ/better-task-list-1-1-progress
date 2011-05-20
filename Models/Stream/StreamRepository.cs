using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterTaskList.Models
{

    public class StreamRepository
    {
        private BetterTaskListDataContext db = new BetterTaskListDataContext();


        public IEnumerable<Stream> GetStream()
        {
            return (from r in db.Streams orderby r.StreamLastUpdatedTimeStamp descending select r).AsEnumerable();
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