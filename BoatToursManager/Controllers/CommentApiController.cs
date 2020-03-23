using BoatToursManager.BL;
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
    public class CommentApiController : ApiController
    {
        CommentBL comment = new CommentBL();
        LikeDislike like = new LikeDislike();
        [System.Web.Http.HttpGet]
        public List<CommentModel> GetCommentsByBoatId(int boatId)
        {
            if (boatId != 0)
            {
                return comment.GetCommentsByBoatId(boatId);
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        public bool Post(CommentModel commentModel)
        {
            return comment.SaveComment(commentModel);
        }
        public bool Delete(CommentModel commentModel)
        {
            return comment.SaveComment(commentModel);
        }

    }
}
