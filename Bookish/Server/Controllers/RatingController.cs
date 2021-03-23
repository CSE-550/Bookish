using Bookish.DataServices;
using Bookish.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookish.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        [HttpPut]
        public IActionResult Put([FromServices]IRatingService ratingService, [FromBody] RatingModel ratingModel)
        {
            AuthUserModel authUser = (AuthUserModel)this.HttpContext.Items["authUserModel"];
            if (ratingModel.Comment_Id.HasValue && ratingModel.Comment_Id != 0)
            {
                ratingModel = ratingService.RateComment(authUser, ratingModel.Comment_Id.Value, ratingModel.isUpvote);
            } 
            else if (ratingModel.Post_Id.HasValue && ratingModel.Post_Id != 0)
            {
                ratingModel = ratingService.RatePost(authUser, ratingModel.Post_Id.Value, ratingModel.isUpvote);
            } 
            else
            {
                return BadRequest();
            }

            return Ok(ratingModel);
        }

        [HttpPatch]
        public IActionResult Patch([FromServices]IRatingService ratingService, [FromBody] RatingModel ratingModel)
        {
            AuthUserModel authUser = (AuthUserModel)this.HttpContext.Items["authUserModel"];
            return Ok(ratingService.RatingPatch(authUser, ratingModel));
        }
    }
}
