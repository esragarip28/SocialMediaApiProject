using DemoApplication1.Models.Entities;
using DemoApplication1.ResultJsonClass;
using System.Collections.Generic;

namespace DemoApplication1.Models.DataAccess.Abstracts
{
    public interface IPostDal
    {
        void SavePostData(Post post);
        void UpdatePostData();
        List<Post> GetPostData(string id);
        List<string> GetImageUrl(string id); //user id value as foreign key
        bool IsNewPost(string userId,string postId);
        void DeleteAllPostData(string id);
        void CheckIsDeleted(string userId,List<Post>posts);
        bool FindByPostId(string userId,string postId);

        void UpdatePostByIsActive(string userId,string postId);

        List<Post> GetActivePostData(string id);

        void UpdateComments(string userId,string mediaId,List<Post> currentPost);
        bool FindComment(string id,string mediaId,string commentId);

        void UpdatePostCommentFromDb(string id,string mediId,CommentData comment);
    }
}
