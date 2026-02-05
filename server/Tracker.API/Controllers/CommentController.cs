using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tracker.Core.DTOs.Comments;
using Tracker.Core.Interfaces;

namespace Tracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst("UserId");
        if (claim == null) throw new UnauthorizedAccessException("User ID not found in token");
        return int.Parse(claim.Value);
    }

    [HttpGet("discussion/{discussionId}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetByDiscussion(int discussionId)
    {
        return Ok(await _commentService.GetDiscussionCommentsAsync(discussionId));
    }

    [HttpPost("discussion/{discussionId}")]
    public async Task<ActionResult<CommentDto>> Create(int discussionId, CreateCommentDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _commentService.CreateCommentAsync(userId, discussionId, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var userId = GetUserId();
            var success = await _commentService.DeleteCommentAsync(userId, id);
            if (!success) return NotFound();
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPost("{id}/like")]
    public async Task<ActionResult> Like(int id)
    {
        try
        {
            var userId = GetUserId();
            await _commentService.LikeCommentAsync(userId, id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/dislike")]
    public async Task<ActionResult> Dislike(int id)
    {
        try
        {
            var userId = GetUserId();
            await _commentService.DislikeCommentAsync(userId, id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}/like")]
    public async Task<ActionResult> RemoveLike(int id)
    {
        try
        {
            var userId = GetUserId();
            await _commentService.RemoveLikeAsync(userId, id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
