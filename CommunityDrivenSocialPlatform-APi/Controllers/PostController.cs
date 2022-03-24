using CommunityDrivenSocialPlatform_APi.Data;
using CommunityDrivenSocialPlatform_APi.Model;
using CommunityDrivenSocialPlatform_APi.Model.Request;
using CommunityDrivenSocialPlatform_APi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommunityDrivenSocialPlatform_APi.Controllers
{
    [Route("api/subthread/{subthreadName}/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly CDSPdB DbContext;
        public PostController(CDSPdB dbContext)
        {
            this.DbContext = dbContext;
        }

        //:: handles creating a post  :://
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatPost([FromBody] CreatePostRequest createPostRequest, [FromRoute] string subthreadName)
        {
            User user = await GetLoggedUser();

            SubThread subThread = await GetSubThread(subthreadName);
            if (subThread == null)
                return BadRequest(Constants.NonExistentSubThread(subthreadName));

            SubThreadUser subThreadUser = await DbContext.SubThreadUser.FirstOrDefaultAsync(r => r.UserId == user.Id && r.SubThreadId == subThread.Id);//check if user is a memeber
            if (subThreadUser == null)
                return BadRequest(Constants.NotMemberOfSubthread(subthreadName));

            Post post = new Post
            {
                Title = createPostRequest.Title,
                Body = createPostRequest.Body,
                CreatedAt = createPostRequest.CreatedAt,
                SubThreadId = subThread.Id,
                Author = user.Id
            };

            DbContext.Post.Add(post);
            await DbContext.SaveChangesAsync();

            createPostRequest.Id = post.Id;
            return Ok(createPostRequest);
        }

        //:: handles returning a post by id :://
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById([FromRoute] string subthreadName, [FromRoute] int id)
        {
            SubThread subThread = await GetSubThread(subthreadName);
            if (subThread == null)
                return BadRequest(Constants.NonExistentSubThread(subthreadName));

            var innerJoin = DbContext.User.Join(DbContext.Post, tbl_user => tbl_user.Id, tbl_post => tbl_post.Author,
                (tbl_user, tbl_post) => new { tbl_user, tbl_post })
                .Select(c => new { c.tbl_post.Id, c.tbl_post.Title, c.tbl_post.Body, c.tbl_post.CreatedAt, c.tbl_post.SubThreadId, c.tbl_user.Username })
                .Where(r => r.Id == id && r.SubThreadId == subThread.Id);

            //get votes

            //get comments

            List<PostDetailsResponse> list = new List<PostDetailsResponse>();
            foreach (var item in innerJoin)
            {
                list.Add(new PostDetailsResponse
                {
                    Title = item.Title,
                    Body = item.Body,
                    Author = item.Username,
                    Id = item.Id
                });
            }

            if (innerJoin == null || list.Count == 0)
                return BadRequest(Constants.NonExistentPost(id, subthreadName));

            return Ok(list.ElementAt(0));
        }

        //:: handles returning all post in a subthread :://
        [HttpGet]
        public async Task<IActionResult> GetAllPosts([FromRoute] string subthreadName)
        {
            SubThread subThread = await GetSubThread(subthreadName);
            if (subThread == null)
                return BadRequest(Constants.NonExistentSubThread(subthreadName));

            var innerJoin = DbContext.User.Join(DbContext.Post, tbl_user => tbl_user.Id, tbl_post => tbl_post.Author,
                (tbl_user, tbl_post) => new { tbl_user, tbl_post })
                .Select(c => new { c.tbl_post.Id, c.tbl_post.Title, c.tbl_post.Body, c.tbl_post.CreatedAt, c.tbl_post.SubThreadId, c.tbl_user.Username })
                .Where(r => r.SubThreadId == subThread.Id);

            List<PostDetailsResponse> list = new List<PostDetailsResponse>();
            foreach (var item in innerJoin)
            {
                list.Add(new PostDetailsResponse
                {
                    Title = item.Title,
                    Body = item.Body,
                    Author = item.Username,
                    Id = item.Id,
                });
            }

            if (innerJoin == null || list.Count == 0)
                return BadRequest(Constants.NonExistentSubThread(subthreadName));

            return Ok(list);
        }

        //:: handles updating a post :://
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromRoute] string subthreadName, [FromBody] UpdatePostRequest updatePostRequest)
        {
            User user = await GetLoggedUser();

            SubThread subThread = await GetSubThread(subthreadName);
            if (subThread == null)
                return BadRequest(Constants.NonExistentSubThread(subthreadName));

            Post post = await DbContext.Post.FirstOrDefaultAsync(r => r.Author == user.Id);
            if (post == null)
                return BadRequest(Constants.NotTheAuthorOfPost);

            updatePostRequest.Id = post.Id;
            post.Title = updatePostRequest.Title;
            post.Body = updatePostRequest.Body;

            DbContext.Post.Update(post);
            await DbContext.SaveChangesAsync();

            return Ok(updatePostRequest);
        }

        //:: handles deleting a post :://
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] string subthreadName, [FromBody] DeletePostRequest deletePostRequest)
        {
            User user = await GetLoggedUser();

            SubThread subThread = await GetSubThread(subthreadName);
            if (subThread == null)
                return BadRequest(Constants.NonExistentSubThread(subthreadName));

            Post post = await DbContext.Post.FirstOrDefaultAsync(r => r.Id == deletePostRequest.Id && r.Author == user.Id);
            if (post == null)
                return BadRequest(Constants.NotTheAuthorOfPost);

            DbContext.Remove(post);
            await DbContext.SaveChangesAsync();

            return Ok(Constants.PostDeleted);
        }

        //:: handles voting :://
        [Authorize]
        [HttpPost("vote")]
        public async Task<IActionResult> Vote([FromRoute] string subthreadName, [FromBody] VoteRequest voteRequest)
        {
            User user = await GetLoggedUser();
            SubThread subThread = await GetSubThread(subthreadName);
            Post post = await GetPost(voteRequest.Id);

            if (subThread == null)
                return BadRequest(Constants.NonExistentSubThread(subthreadName));
            if (post == null)
                return BadRequest(Constants.NonExistentPost(voteRequest.Id));


            Vote vote = await DbContext.Vote.FirstOrDefaultAsync(r => r.UserId == user.Id && r.PostId == voteRequest.Id);
            if (vote != null)
            {
                vote.VoteTypeId = voteRequest.VoteType;
                DbContext.Update(vote);
                await DbContext.SaveChangesAsync();
                return Ok(Constants.VoteSuccess);
            }

            vote = new Vote
            {
                UserId = user.Id,
                PostId = post.Id,
                VoteTypeId = voteRequest.VoteType,
            };

            DbContext.Add(vote);
            await DbContext.SaveChangesAsync();
            return Ok(Constants.VoteSuccess);
        }

        //Get all comments on a post  body, author, created at

        //:: handles creating comments :://
        [Authorize]
        [HttpPost("{postId}/comment")]
        public async Task<IActionResult> CreateComment([FromRoute] int postId, [FromRoute] string subthreadName, [FromBody] CreateCommentRequest createCommentRequest)
        {
            User user = await GetLoggedUser();
            SubThread subThread = await GetSubThread(subthreadName);
            Post post = await GetPost(postId);

            if (subThread == null)
                return BadRequest(Constants.NonExistentSubThread(subthreadName));
            if (post == null)
                return BadRequest(Constants.NonExistentPost(postId));

            SubThreadUser subThreadUser = await DbContext.SubThreadUser.FirstOrDefaultAsync(r => r.UserId == user.Id && r.SubThreadId == subThread.Id);//check if user is a memeber
            if (subThreadUser == null)
                return BadRequest(Constants.NotMemberOfSubthread(subthreadName));

            Comment comment = new Comment
            {
                PostId = postId,
                UserId = user.Id,
                Body = createCommentRequest.Body,
                CreatedAt = createCommentRequest.CreatedAt,
            };

            await DbContext.Comment.AddAsync(comment);
            await DbContext.SaveChangesAsync();

            return Ok(Constants.CommentMadeSuccessfully);
        }

        //:: handles updating comments :://
        [Authorize]
        [HttpPut("comment")]
        public async Task<IActionResult> UpdateComment([FromRoute] string subthreadName, [FromBody] UpdateCommentRequest updateCommentRequest)
        {
            User user = await GetLoggedUser();
            Comment comment = await GetComment(updateCommentRequest.CommentId);
            if (comment == null)
                return BadRequest(Constants.NonExistentComment);

            SubThread subThread = await GetSubThread(subthreadName);
            Post post = await GetPost(comment.PostId);

            if (subThread == null)
                return BadRequest(Constants.NonExistentSubThread(subthreadName));
            if (post == null)
                return BadRequest(Constants.NonExistentPost(comment.PostId));

            if (comment.UserId != user.Id)
                return BadRequest(Constants.NotTheAuthorOfComment);

            comment.Body = updateCommentRequest.Body;
            
            DbContext.Comment.Update(comment);
            await DbContext.SaveChangesAsync();

            return Ok(Constants.CommentUpdatedSuccessfully);
        }

        //:: handles updating comments :://
        [Authorize]
        [HttpDelete("comment")]
        public async Task<IActionResult> DeleteComment([FromRoute] string subthreadName, [FromBody] UpdateCommentRequest updateCommentRequest)
        {
            User user = await GetLoggedUser();
            Comment comment = await GetComment(updateCommentRequest.CommentId);
            if (comment == null)
                return BadRequest(Constants.NonExistentComment);

            SubThread subThread = await GetSubThread(subthreadName);
            Post post = await GetPost(comment.PostId);

            if (subThread == null)
                return BadRequest(Constants.NonExistentSubThread(subthreadName));
            if (post == null)
                return BadRequest(Constants.NonExistentPost(comment.PostId));

            if (comment.UserId != user.Id)
                return BadRequest(Constants.NotTheAuthorOfComment);

            DbContext.Comment.Remove(comment);
            await DbContext.SaveChangesAsync();

            return Ok(Constants.CommentDeletedSuccessfully);
        }


        public async Task<User> GetLoggedUser()
        {
            string loggedUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return await DbContext.User.FirstOrDefaultAsync(r => r.Username == loggedUser);
        }
        public async Task<SubThread> GetSubThread(string name)
        {
            return await DbContext.SubThread.FirstOrDefaultAsync(r => r.Name == name);
        }
        public async Task<Post> GetPost(int id)
        {
            return await DbContext.Post.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Comment> GetComment(int id)
        {
            return await DbContext.Comment.FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}

