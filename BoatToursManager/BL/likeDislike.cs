using BoatToursManager.DAL;
using BoatToursManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class LikeDislike
    {
        public DAL.BoatTourManagerEntities1 db { get; } = new DAL.BoatTourManagerEntities1();
        public int Id { get; set; }
        public int BoatId { get; set; }
        public int UserId { get; set; }
        public PostAction Action { get; set; }
        private List<LikeDislike> likes = new List<LikeDislike>();

        public enum PostAction
        {
            None = 0,
            Dislike = 1,
            Like = 2
        }
        public List<BL.Boat> getBoatsFromDB()
        {
            var boat = (from x in db.Boat
                        select new BL.Boat
                        {
                            id = x.id,
                            name = x.name,
                            capacity = x.capacity,
                            pricePerHour = x.pricePerHour,
                            likeCount = (int)x.likeCount,
                            disLikeCount = (int)x.disLikeCount,
                            title = x.title,
                            imagePath = x.imagePath,
                            seasonId = (int)x.seasonId,
                            locationId = (int)x.location_id
                        }).ToList();
            return boat;
        }
        //public bool Like(DAL.LikeDislikeBoat model)
        //{
        //    LikeDislikeBoat likeDislike = db.LikeDislikeBoats.FirstOrDefault(x => x.boatId == model.boatId && x.userid == model.userid);
        //    if (likeDislike == null)
        //    {
        //        db.LikeDislikeBoats.Add(model);
        //        db.SaveChanges();
        //        return true;
        //    }
        //    else
        //    {
        //        if (likeDislike.action == (int)PostAction.Like)
        //        {
        //            // user has previously Liked the post, remove the Like
        //            db.LikeDislikeBoats.Remove(model);
        //            db.SaveChanges();
        //            return true;
        //        }
        //        return false;
        //    }
        //}
        //public bool DisLike(DAL.LikeDislikeBoat model)
        //{
        //    LikeDislikeBoat likeDislike = db.LikeDislikeBoats.FirstOrDefault(x => x.boatId == model.boatId && x.userid == model.userid);
        //    if (likeDislike == null)
        //    {
        //        db.LikeDislikeBoats.Add(model);
        //        db.SaveChanges();
        //        return true;
        //    }
        //    else
        //    {
        //        if (likeDislike.action == (int)PostAction.Like)
        //        {
        //            // user has previously Liked the post, remove the Like
        //            db.LikeDislikeBoats.Remove(model);
        //            db.SaveChanges();
        //            return true;
        //        }
        //        return false;
        //    }
        //}
        public string Like(int boatId, bool status, int userId)
        {
            var boat = db.Boat.FirstOrDefault(x => x.id == boatId);
            var toggle = false;
            LikeDislikeBoat like = db.LikeDislikeBoat.FirstOrDefault(x => x.boatId == boatId && x.userid == userId);
            // here we are checking whether user have done like or dislike    
            if (like == null)
            {
                like = new LikeDislikeBoat();
                like.userid = userId;
                like.isLike = status;
                like.boatId = boatId;
                if (status)
                {
                    if (boat.likeCount == null) // if no one has done like or dislike and first time any one doing like and dislike then assigning 1 and                                                                                0    
                    {
                        boat.likeCount = boat.likeCount < 1 ? 1 : boat.likeCount;
                        boat.disLikeCount = boat.disLikeCount < 0 ? 1 : boat.disLikeCount;
                    }
                    else
                    {
                        boat.likeCount = boat.likeCount + 1;
                    }
                }
                else
                {
                    if (boat.disLikeCount == null)
                    {
                        boat.disLikeCount = boat.disLikeCount < 1 ? 1 : boat.disLikeCount;
                        boat.likeCount = boat.likeCount < 1 ? 1 : boat.likeCount;
                    }
                    else
                    {
                        boat.disLikeCount = boat.disLikeCount + 1;
                    }
                }
                db.LikeDislikeBoat.Add(like);
            }
            else
            {
                toggle = true;
            }
            if (toggle)
            {
                like.userid = userId;
                like.isLike = status;
                like.boatId = boatId;
                if (status)
                {
                    // if user has click like button then need to increase +1 in like and -1 in Dislike    
                    boat.likeCount = boat.likeCount + 1;
                    if (boat.disLikeCount == 0 || boat.disLikeCount < 0)
                    {
                        boat.disLikeCount = 0;
                    }
                    else
                    {
                        boat.disLikeCount = boat.disLikeCount - 1;
                    }
                }
                else
                {
                    // if user has click dislike then need to increase +1 in dislike and -1 in like    
                    boat.disLikeCount = boat.disLikeCount + 1;
                    if (boat.likeCount == 0 || boat.likeCount < 0)
                    {
                        boat.likeCount = 0;
                    }
                    else
                    {
                        boat.likeCount = boat.likeCount - 1;
                    }
                }
            }
            db.SaveChanges();
            return boat.likeCount + "/" + boat.disLikeCount;
        }
        public int? Getlikecounts(int id) // to count like  
        {
            var count = (from x in db.Boat where (x.id == id && x.likeCount != null) select x.likeCount).FirstOrDefault();
            return count;
        }
        //To Get DisLike Count  
        public int? Getdislikecounts(int id)
        {
            var count = (from x in db.Boat where x.id == id && x.disLikeCount != null select x.disLikeCount).FirstOrDefault();
            return count;

        }
        // to get all users who have done like and dislike of the society  
        public List<DAL.LikeDislikeBoat> GetallUser(int id)
        {

            var count = (from x in db.LikeDislikeBoat where x.boatId == id select x).ToList();
            return count;
        }
        public BoatModel GetBoatDetailById(int boatId)
        {
            var boat = (from x in db.Boat
                        where x.id == boatId
                        select new BoatModel
                        {
                            id = x.id,
                            name = x.name,
                            capacity = x.capacity,
                            pricePerHour = x.pricePerHour,
                         //  imagePath = x.imagePath
                        }).FirstOrDefault();
            return boat;
        }
        public List<PopularityModel> getPopularity()
        {
            var boat = (from x in db.Popularity
                        select new PopularityModel
                        {
                            id = x.id,
                            name = x.name
                        }).ToList();
            return boat;
        }
    }
}