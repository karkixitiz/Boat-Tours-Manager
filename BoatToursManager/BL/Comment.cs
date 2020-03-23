using BoatToursManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class CommentBL
    {
        public DAL.BoatTourManagerEntities1 db { get; } = new DAL.BoatTourManagerEntities1();
        public int id { get; set; }
        public string comment { get; set; }
        public int userId { get; set; }
        public int boatId { get; set; }
        public DateTime commentDate { get; set; }
        public List<CommentModel> GetCommentsByBoatId(int boatId)
        {
            var boat = (from x in db.Comments
                        where x.boatId == boatId
                        select new CommentModel
                        {
                            id = x.id,
                            comment = x.comments1,
                            userId = (int)x.userId,
                            commentDate =(DateTime) x.commentDate,
                            boatId = (int)x.boatId
                        }).ToList();
            return boat;
        }
        public bool SaveComment(CommentModel cmodel)
        {
            try
            {
                var insertComment = new DAL.Comments
                {
                    userId = cmodel.userId,
                    boatId = cmodel.boatId,
                    commentDate = DateTime.Now,
                    comments1 = cmodel.comment
                };
                db.Comments.Add(insertComment);
                db.SaveChanges();
            }catch(Exception ex)
            {
                throw ex;
               // return false;
            }
            return true;
        }
        public bool DeleveComment(CommentModel cmodel)
        {
            try
            {
                var employer = new DAL.Comments { id = cmodel.id };
                db.Entry(employer).State = EntityState.Deleted;
                db.SaveChanges();
               
            }
            catch (Exception ex)
            {
                throw ex;
                // return false;
            }
            return true;
        }
    }
}