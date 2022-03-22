using CommunityDrivenSocialPlatform_APi.Data;
using CommunityDrivenSocialPlatform_APi.Model;
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
                return BadRequest(ConstantsService.NonExistentSubThread(subthreadName));

            SubThreadUser subThreadUser = await DbContext.SubThreadUser.FirstOrDefaultAsync(r=> r.UserId==user.Id && r.SubThreadId==subThread.Id);//check if user is a memeber
            if (subThreadUser == null)
                return BadRequest(ConstantsService.NotMemberOfSubthread(subthreadName));

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
                return BadRequest(ConstantsService.NonExistentSubThread(subthreadName));

            var innerJoin = DbContext.User.Join(DbContext.Post, tbl_user => tbl_user.Id, tbl_post => tbl_post.Author,
                (tbl_user, tbl_post) => new { tbl_user, tbl_post })
                .Select(c => new { c.tbl_post.Id, c.tbl_post.Title, c.tbl_post.Body, c.tbl_post.CreatedAt, c.tbl_post.SubThreadId, c.tbl_user.Username })
                .Where(r => r.Id == id && r.SubThreadId == subThread.Id);

            List<GetPostRequest> list = new List<GetPostRequest>();
            foreach (var item in innerJoin)
            {
                list.Add(new GetPostRequest
                {
                    Title = item.Title,
                    Body = item.Body,
                    Author = item.Username,
                    Id = item.Id
                });
            }

            if (innerJoin == null || list.Count == 0)
                return BadRequest(ConstantsService.NonExistentPost(id,subthreadName));

            return Ok(list.ElementAt(0));
        }

        //:: handles returning all post in a subthread :://
        [HttpGet]
        public async Task<IActionResult> GetAllPosts([FromRoute] string subthreadName)
        {
            SubThread subThread = await GetSubThread(subthreadName);
            if (subThread == null)
                return BadRequest(ConstantsService.NonExistentSubThread(subthreadName));

            var innerJoin = DbContext.User.Join(DbContext.Post, tbl_user => tbl_user.Id, tbl_post => tbl_post.Author,
                (tbl_user, tbl_post) => new { tbl_user, tbl_post })
                .Select(c => new { c.tbl_post.Id, c.tbl_post.Title, c.tbl_post.Body, c.tbl_post.CreatedAt, c.tbl_post.SubThreadId, c.tbl_user.Username })
                .Where(r => r.SubThreadId == subThread.Id);

            List<GetPostRequest> list = new List<GetPostRequest>();
            foreach (var item in innerJoin)
            {
                list.Add(new GetPostRequest
                {
                    Title = item.Title,
                    Body = item.Body,
                    Author = item.Username,
                    Id = item.Id,
                });
            }

            if (innerJoin == null || list.Count == 0)
                return BadRequest(ConstantsService.NonExistentSubThread(subthreadName));

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
                return BadRequest(ConstantsService.NonExistentSubThread(subthreadName));

            Post post = await DbContext.Post.FirstOrDefaultAsync(r => r.Author == user.Id);
            if (post == null)
                return BadRequest(ConstantsService.NotTheAuthorOfPost);

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
                return BadRequest(ConstantsService.NonExistentSubThread(subthreadName));

            Post post = await DbContext.Post.FirstOrDefaultAsync(r => r.Id == deletePostRequest.Id && r.Author == user.Id);
            if (post == null)
                return BadRequest(ConstantsService.NotTheAuthorOfPost);

            DbContext.Remove(post);
            await DbContext.SaveChangesAsync();

            return Ok(ConstantsService.PostDeleted);
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
                return BadRequest(ConstantsService.NonExistentSubThread(subthreadName));
            if (post == null)
                return BadRequest(ConstantsService.NonExistentPost(voteRequest.Id));


            Vote vote = await DbContext.Vote.FirstOrDefaultAsync(r => r.UserId == user.Id && r.PostId == voteRequest.Id);
            if (vote != null)
            {
                vote.VoteTypeId = voteRequest.VoteType;
                DbContext.Update(vote);
                await DbContext.SaveChangesAsync();
                return Ok(ConstantsService.VoteSuccess);
            }

            vote = new Vote
            {
                UserId = user.Id,
                PostId = post.Id,
                VoteTypeId = voteRequest.VoteType,
            };

            DbContext.Add(vote);
            await DbContext.SaveChangesAsync();
            return Ok(ConstantsService.VoteSuccess);
        }


        //comments:: TODO
        [Authorize]
        [HttpPost("comment")]
        public async Task<IActionResult> CreateComment([FromRoute]string subthreadName,[FromBody]CreateCommentRequest createCommentRequest)
        {
            User user = await GetLoggedUser();
            SubThread subThread = await GetSubThread(subthreadName);
            Post post = await GetPost(createCommentRequest.PostId);

            if (subThread == null)
                return BadRequest(ConstantsService.NonExistentSubThread(subthreadName));
            if (post == null)
                return BadRequest(ConstantsService.NonExistentPost(createCommentRequest.PostId));

            SubThreadUser subThreadUser = await DbContext.SubThreadUser.FirstOrDefaultAsync(r => r.UserId == user.Id && r.SubThreadId == subThread.Id);//check if user is a memeber
            if (subThreadUser == null)
                return BadRequest(ConstantsService.NotMemberOfSubthread(subthreadName));

            return Ok();
        }

        //del comment
        //update comment


        public class CreatePostRequest
        {
            [Required(ErrorMessage = "Sorry, post title is required.")]
            [MinLength(3, ErrorMessage = "Sorry, post title must be atleast 3 characters long.")]
            [MaxLength(255, ErrorMessage = "Sorry, post title must not be longer than 255 characters long.")]
            public string Title { get; set; }

            [Required(ErrorMessage = "Sorry, post body is required.")]
            [MinLength(3, ErrorMessage = "Sorry, post body must be atleast 3 characters long.")]
            public string Body { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.Now;
            public int Id { get; set; }
        }

        public class UpdatePostRequest
        {
            [Required(ErrorMessage = "Sorry, post id is required.")]
            [RegularExpression("[1-9]+", ErrorMessage = "Sorry id must be a number and cannot be 0.")]
            public int Id { get; set; }
            [Required(ErrorMessage = "Sorry, post title is required.")]
            [MinLength(3, ErrorMessage = "Sorry, post title must be atleast 3 characters long.")]
            [MaxLength(255, ErrorMessage = "Sorry, post title must not be longer than 255 characters long.")]
            public string Title { get; set; }

            [Required(ErrorMessage = "Sorry, post body is required.")]
            [MinLength(3, ErrorMessage = "Sorry, post body must be atleast 3 characters long.")]
            public string Body { get; set; }
        }

        public class GetPostRequest
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public string Author { get; set; }
            public int Votes { get; set; }
        }

        public class DeletePostRequest
        {
            [Required(ErrorMessage = "Sorry, post id is required.")]
            public int? Id { get; set; }
        }

        public class VoteRequest
        {
            [Required(ErrorMessage = "Sorry, post id is required.")]
            [RegularExpression("[1-9]+", ErrorMessage = "Sorry id must be a number and cannot be 0.")]
            public int Id { get; set; }
            [Required(ErrorMessage = "Sorry, post vote type is required.")]
            [RegularExpression("(1|2)", ErrorMessage = "Sorry, vote value must be '1' or '2'.")]
            public byte VoteType { get; set; }
        }

        public class CreateCommentRequest
        {
            [Required(ErrorMessage = "Sorry, user id is required.")]
            [RegularExpression("[1-9]+", ErrorMessage = "Sorry id must be a number and cannot be 0.")]
            public int UserId { get; set; }

            [Required(ErrorMessage = "Sorry, post id is required.")]
            [RegularExpression("[1-9]+", ErrorMessage = "Sorry id must be a number and cannot be 0.")]
            public int PostId { get; set; }

            [Required(ErrorMessage ="Sorry, comment body cannot be empty.")]
            public string Body { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.Now;
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
    }
}

