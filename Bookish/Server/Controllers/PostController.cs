﻿using Bookish.DataServices;
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
    public class PostController : ControllerBase
    {
        /// <summary>
        /// Gets a given postmodel by id
        /// </summary>
        /// <param name="postService">The post service for reading the post</param>
        /// <param name="id">The id of the post</param>
        /// <returns>
        /// The post model
        /// </returns>
        [HttpGet]
        public PostModel Get([FromServices] IPostService postService, [FromQuery] int id)
        {
            return postService.GetPost(id);
        }

        /// <summary>
        /// Puts a postmodel to the database
        /// </summary>
        /// <param name="postService">The post service for creation</param>
        /// <param name="postModel">The postmodel that is being created</param>
        /// <returns>
        /// The newly created postmodel
        /// </returns>
        [HttpPut]
        public PostModel Put([FromServices] IPostService postService, [FromBody] PostModel postModel)
        {
            return postService.CreatePost(postModel);
        }
    }
}
