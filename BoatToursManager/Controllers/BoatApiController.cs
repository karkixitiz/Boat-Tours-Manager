using BoatToursManager.BL;
using BoatToursManager.DAL;
using BoatToursManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace BoatToursManager.Controllers
{
    [EnableCors(origins: "http://boattours.azurewebsites.net", headers: "*", methods: "*")]
    public class BoatApiController : ApiController
    {
        public DAL.BoatTourManagerEntities1 db { get; } = new DAL.BoatTourManagerEntities1();
        public int Id { get; set; }
        public int BoatId { get; set; }
        public int UserId { get; set; }
        LikeDislike like = new LikeDislike();
        CommentBL comment = new CommentBL();

        [System.Web.Http.HttpGet]
        public List<BL.Boat> GetBoatsByLocationId(int locationId, int popularity)
        {
            BL.Location l = MainClass.Instance.getLocations().Find(v => v.id == locationId);
            if (l != null)
            {
                switch (popularity)
                {
                    case 1:
                        return like.getBoatsFromDB().Where(x => x.seasonId == popularity).Where(x => x.locationId == locationId).ToList();
                        break;
                    case 2:
                        return like.getBoatsFromDB().Where(x => x.seasonId == popularity).Where(x => x.locationId == locationId).ToList();
                        break;
                    case 3:
                        return like.getBoatsFromDB().OrderByDescending(b => b.pricePerHour).Where(x => x.locationId == locationId).ToList();
                        break;
                    case 4:
                        return like.getBoatsFromDB().OrderBy(b => b.pricePerHour).Where(x => x.locationId == locationId).ToList();
                        break;
                    case 5:
                        return like.getBoatsFromDB().OrderBy(b => b.capacity).Where(x => x.locationId == locationId).ToList();
                        break;
                    case 6:
                        return like.getBoatsFromDB().OrderByDescending(b => b.capacity).Where(x => x.locationId == locationId).ToList();
                        break;
                    case 7:
                        return like.getBoatsFromDB().OrderByDescending(b => b.likeCount).Where(x => x.locationId == locationId).ToList();
                        break;
                    case 8:
                        return like.getBoatsFromDB().OrderBy(b => b.disLikeCount).Where(x => x.locationId == locationId).ToList();
                        break;
                    default:
                        return like.getBoatsFromDB().Where(x => x.locationId == locationId).ToList();
                }
            }
            else
            {
                List<BL.Boat> boat = MainClass.Instance.getBoats();
                if (boat != null)
                {
                    switch (popularity)
                    {
                        case 1:
                            return like.getBoatsFromDB().Where(x => x.seasonId == popularity).ToList();
                            break;
                        case 2:
                            return like.getBoatsFromDB().Where(x => x.seasonId == popularity).ToList();
                            break;
                        case 3:
                            return like.getBoatsFromDB().OrderByDescending(b => b.pricePerHour).ToList();
                            break;
                        case 4:
                            return like.getBoatsFromDB().OrderBy(b => b.pricePerHour).ToList();
                            break;
                        case 5:
                            return like.getBoatsFromDB().OrderBy(b => b.capacity).ToList();
                            break;
                        case 6:
                            return like.getBoatsFromDB().OrderByDescending(b => b.capacity).ToList();
                            break;
                        case 7:
                            return like.getBoatsFromDB().OrderByDescending(b => b.likeCount).ToList();
                            break;
                        case 8:
                            return like.getBoatsFromDB().OrderBy(b => b.disLikeCount).ToList();
                            break;
                        default:
                            return like.getBoatsFromDB();
                    }
                }
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        [System.Web.Http.HttpGet]
        public BoatModel GetBoatDetailsById(int boatId)
        {
            if (boatId != 0)
            {
                return like.GetBoatDetailById(boatId);
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
        [System.Web.Http.HttpGet]
        public List<BL.Boat> GetBoat()
        {
            try { 
            List<BL.Boat> boat = like.getBoatsFromDB();
            if (boat != null)
            {
                return boat;
            }
            }catch(Exception ex)
            {
                return null;
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }


        // [Route("api/BoatApi/{boatId:int}")]


        //[ActionName("likeboat")]
        public string Post(LikeDislikeModel likeDislikeModel)
        {
            //if (!SessionManager.userIsLoggedIn())
            //    return new HttpStatusCodeResult(403).ToString() ;
            var boat = db.Boat.FirstOrDefault(x => x.id == likeDislikeModel.boatId);
            var toggle = false;
            LikeDislikeBoat like = db.LikeDislikeBoat.FirstOrDefault(x => x.boatId == likeDislikeModel.boatId && x.userid == likeDislikeModel.userId);
            // here we are checking whether user have done like or dislike    
            if (like == null)
            {
                like = new LikeDislikeBoat();
                like.userid = likeDislikeModel.userId;
                like.isLike = likeDislikeModel.status;
                like.boatId = likeDislikeModel.boatId;
                if (likeDislikeModel.status)
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
                like.userid = likeDislikeModel.userId;
                like.isLike = likeDislikeModel.status;
                like.boatId = likeDislikeModel.boatId;
                if (likeDislikeModel.status)
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
        public bool checkIfUserSelectSameAction(int userId, bool isLike)
        {
            LikeDislikeBoat like = db.LikeDislikeBoat.Where(x=>x.isLike==isLike).Where(x=>x.userid == userId).FirstOrDefault();
            if (like != null)
            {
                return false;
            }
            return true;
        }
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
        
    }
}
