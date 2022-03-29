using CDSP_API.Data;
using CDSP_API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CDSP_API.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CDSP_API.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;
        private readonly ICommentService _commentService;
        private readonly ISubThreadsService _subThreadsService;
        public PostService(DataContext dataContext, ISubThreadsService subThreadsService, ICommentService commentService)
        {
            _dataContext = dataContext;
            _commentService = commentService;
            _subThreadsService = subThreadsService;

        }

        private async Task InitalizeVoteTypesAsync()
        {
            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
                VoteType voteType = await _dataContext.VoteType.SingleOrDefaultAsync(r => r.Id == 1);
                if(voteType == null)
                {
                    await _dataContext.VoteType.AddAsync(new VoteType { VoteTypeName = "UPVOTE" });
                    await _dataContext.VoteType.AddAsync(new VoteType { VoteTypeName = "DOWNVOTE" });
                    await _dataContext.SaveChangesAsync();
                }
            }catch(Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }
        }

        public async Task<EnityCoreResult> AddDownVoteAsync(int id, User user)
        {
            await InitalizeVoteTypesAsync();

            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
                (var _voteEcr, var vote) = await DoesVoteExistMadeByUserOnPost(id, user);
                if (vote!=null) {
                    if (vote.VoteTypeId == (int)PostVoteEnum.DOWNVOTE)
                    {
                        return ecr;
                    }
                    vote.VoteTypeId = (int)PostVoteEnum.DOWNVOTE;
                    _dataContext.Vote.Update(vote);
                    await _dataContext.SaveChangesAsync();
                    return ecr;
                }
                vote = new Vote { UserId = user.Id, PostId = id, VoteTypeId = (int)PostVoteEnum.DOWNVOTE };
                await _dataContext.Vote.AddAsync(vote);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }

            return ecr;
        }

        public async Task<EnityCoreResult> AddUpVoteAsync(int id, User user)
        {
            await InitalizeVoteTypesAsync();

            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
                (var _voteEcr, var vote) = await DoesVoteExistMadeByUserOnPost(id, user);
                if (vote != null)
                {
                    if (vote.VoteTypeId == (int)PostVoteEnum.UPVOTE)
                    {
                        return ecr;
                    }
                    vote.VoteTypeId = (int)PostVoteEnum.UPVOTE;
                    _dataContext.Vote.Update(vote);
                    await _dataContext.SaveChangesAsync();
                    return ecr;
                }
                vote = new Vote { UserId = user.Id, PostId = id, VoteTypeId = (int)PostVoteEnum.UPVOTE };
                await _dataContext.Vote.AddAsync(vote);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }

            return ecr;
        }

        private async Task<(EnityCoreResult, Vote)> DoesVoteExistMadeByUserOnPost(int postId, User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            Vote vote = null;
            try
            {
                await InitalizeVoteTypesAsync();

                vote = await _dataContext.Vote.SingleOrDefaultAsync(r => r.UserId == user.Id && r.PostId == postId);
                if(vote!= null)
                {
                    return (ecr, vote);
                }
            }
            catch (Exception ex)
            {
                ecr.IsSuccess=false;
                ecr.MapException(ex);
            }
            return (ecr, vote);
        }

        public async Task<(EnityCoreResult, Post)> CreateAsync(Post post, User user, string subThreadName)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
                (var _ecr, var subThread) = await _subThreadsService.GetByNameAsync(subThreadName);
                post.SubThreadId = subThread.Id;
                post.AuthorId = user.Id;
                await _dataContext.Post.AddAsync(post);
                await _dataContext.SaveChangesAsync();
                await AddUpVoteAsync(post.Id, user);
            }
            catch (Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }

            return (ecr, post);
        }

        public async Task<EnityCoreResult> DeleteByIdAsync(int id, User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
                Post post = await _dataContext.Post.FirstOrDefaultAsync(r => r.AuthorId == user.Id && r.Id == id);
                _dataContext.Post.Remove(post);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }

            return (ecr);
        }

        public async Task<(EnityCoreResult, Post)> GetByIdAsync(int id)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            Post post = null;
            try
            {
                post = await _dataContext.Post.SingleOrDefaultAsync(r => r.Id == id);
                if(post != null)
                {
                    (var _ecr, var comments) = await GetAllCommentsAsync(post.Id);
                    post.Comment = comments;
                }
            }
            catch (Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }

            return (ecr, post);
        }

        public async Task<(EnityCoreResult, Post)> UpdateAsync(Post post, User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
                Post _post = await _dataContext.Post.FirstOrDefaultAsync(r=> r.AuthorId==user.Id && r.Id== post.Id);
                _post.Title = post.Title;
                _post.Body = post.Body;
                _dataContext.Post.Update(_post);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }

            return (ecr, post);
        }

        public async Task<(EnityCoreResult, int)> GetVoteCountAsync(int id)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            List<Vote> votes = null;
            int votesValue = 0;
            try
            {
                votes = await _dataContext.Vote.Where(r => r.PostId == id).ToListAsync();
                foreach (var vote in votes)
                {
                    if (vote.VoteTypeId == (int)PostVoteEnum.UPVOTE)
                    {
                        votesValue++;
                    }
                    else
                    {
                        votesValue--;
                    }
                }
            }
            catch (Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }

            return (ecr, votesValue);
        }

        public async Task<(EnityCoreResult, List<Comment>)> GetAllCommentsAsync(int id)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
                Post _post = await _dataContext.Post.SingleOrDefaultAsync(r => r.Id == id);
                (var __ecr, var comments) = await _commentService.GetAll(id);
                _post.Comment = comments;
                return(ecr, comments);
            }
            catch (Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }

            return (ecr, null);
        }

        public async Task<(EnityCoreResult, List<Post>)> GetAllPostsOnSubThreadAsync(string subthreadName)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
                (var _ecr, var subThread) = await _subThreadsService.GetByNameAsync(subthreadName);
                var posts = await _dataContext.Post.Where(r => r.SubThreadId == subThread.Id).ToListAsync();
                return (_ecr, posts);
            }
            catch (Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }

            return (ecr, null);
        }


    }
}