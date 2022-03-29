﻿using CDSP_API.Contracts;
using CDSP_API.Contracts.V1.Requests;
using CDSP_API.Contracts.V1.Responses;
using CDSP_API.misc;
using CDSP_API.Models;
using CDSP_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CDSP_API.Controllers.V1
{
    [Route("/api/v1/subthread/{subThreadName}/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IPostService _postService;
        private readonly ILogger<IdentityController> _logger;
        public PostController(IUsersService usersService, IPostService postService, ILogger<IdentityController> logger)
        {
            _logger = logger;
            _usersService = usersService;
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts([FromRoute] string subThreadName, [FromRoute]int id)
        {
           (var ecr, var posts) = await _postService.GetAllPostsOnSubThreadAsync(subThreadName);
            if (ecr.IsSuccess)
            {
                var postDetailsResponse = new List<PostDetailsResponse>(); 
                foreach(var post in posts)
                {
                    (var _ecr, int count) = await _postService.GetVoteCountAsync(id);

                    var res = new PostDetailsResponse().MapToReponse(post);
                    res.VoteCount = count;
                    postDetailsResponse.Add(res);
                }
                return Ok(postDetailsResponse);
            }
            _logger.LogError(ecr.ToString(subThreadName + "," + id));
            return BadRequest(ApiConstant.GenericError);
        }

        [HttpGet(ApiRoutes.Controller.RouteVariable.PostId)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            (var ecr, var post) = await _postService.GetByIdAsync(id);
            if(ecr.IsSuccess && post !=null)
            {
                (var _erc, int count)= await _postService.GetVoteCountAsync(id);
                var res = new PostDetailsResponse().MapToReponse(post);
                res.VoteCount = count;
                return Ok(res);
            }
            else if (ecr.IsSuccess && post is null)
            {
                return NotFound(ApiConstant.Post.NonExistenPost);
            }
            _logger.LogError(ecr.ToString(id));
            return BadRequest(ApiConstant.GenericError);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreatePostRequest createPostRequest, [FromRoute] string subThreadName)
        {
            (var ecr, User loggedUser) = await _usersService.GetLoggedUser(User);

            (var createEcr, var post)  = await _postService.CreateAsync(createPostRequest.MapToModel(), loggedUser, subThreadName);
            if (createEcr.IsSuccess)
            {
                await _postService.AddUpVoteAsync(post.Id, loggedUser);
                var res = new PostDetailsResponse().MapToReponse(post);
                res.VoteCount = 1;
                return Ok(res);
            }

            _logger.LogError(ecr.ToString(createPostRequest));
            return BadRequest(ApiConstant.GenericError);
        }

        [Authorize]
        [HttpPut(ApiRoutes.Controller.RouteVariable.PostId)]
        public async Task<IActionResult> Update([FromBody]UpdatePostRequest updatePostRequest, [FromRoute] int id)
        {
            (var ecr, User loggedUser) = await _usersService.GetLoggedUser(User);

            updatePostRequest.Id = id;
            (var updateEcr, var post) = await _postService.UpdateAsync(updatePostRequest.MapToModel(), loggedUser);
            if (updateEcr.IsSuccess)
            {
                (var _ecr, int count) = await _postService.GetVoteCountAsync(post.Id);
                var res = new PostDetailsResponse().MapToReponse(post);
                res.VoteCount = count;
                return Ok(res);
            }

            _logger.LogError(updateEcr.ToString(updatePostRequest));
            return BadRequest(ApiConstant.GenericError);
        }

        [Authorize]
        [HttpDelete(ApiRoutes.Controller.RouteVariable.PostId)]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            (var ecr, User loggedUser) = await _usersService.GetLoggedUser(User);

            var _ecr = await _postService.DeleteByIdAsync(id, loggedUser);
            if (_ecr.IsSuccess)
            {
                return Ok(ApiConstant.Post.PostDeleted);
            }

            _logger.LogError(ecr.ToString(id));
            return BadRequest(ApiConstant.GenericError);
        }


    }
}