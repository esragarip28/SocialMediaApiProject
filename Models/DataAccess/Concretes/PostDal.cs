using DemoApplication1.Models.DataAccess.Abstracts;
using DemoApplication1.Models.Entities;
using DemoApplication1.ResultJsonClass;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace DemoApplication1.Models.DataAccess.Concretes
{
    public class PostDal : IPostDal
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoClient dbClient;
        private readonly IMongoDatabase db;

        public PostDal(IConfiguration configuration)
        {
            _configuration = configuration;
            dbClient = new MongoClient(_configuration.GetConnectionString("DemoAppCon"));
            db = dbClient.GetDatabase("userdb");
        }

        /// <summary>
        /// with this method we will recognize that whether user shared new post or not .After that it will save this one to the db.
        /// </summary>
        /// <param name="userId"></param>to understand which user's post
        /// <param name="postId"></param>this is unique for all post so we will understand which post data there is in our db.
        /// <returns></returns>
        public bool IsNewPost(string userId, string postId)
        {
            List<Post> posts = new List<Post>();
            posts = GetPostData(userId);
            if (posts != null)
            {
                foreach (var item in posts)
                {
                    if (item.IgId != postId)
                    {
                        return true; //
                    }
                    else return false;
                }
            }
            return true;
        }


        public void DeleteAllPostData(string id)
        {
            var filter= Builders<Post>.Filter.Eq(p => p.UserId, id);
            db.GetCollection<Post>("Post").DeleteMany(filter);
        }

        public List<string> GetImageUrl(string id)// user id as foreign key
        {
            List<string> imageUrlList = new List<string>();
            var postList = db.GetCollection<Post>("Post").Find(a => a.UserId.Equals(id)).ToListAsync().Result;
            if (postList != null)
            {
              foreach(var item in postList)
                {
                    imageUrlList.Add(item.MediaUrl.ToString());
                }
            }
            return imageUrlList;
        }

        public List<Post> GetPostData(string id)
        {
            List<Post> postList = new List<Post>();
            var postData = db.GetCollection<Post>("Post").Find(a => a.UserId.Equals(id)).ToListAsync().Result;
            if (postData != null)
            {
                foreach(var item in postData)
                {
                    postList.Add(item);
                }
            }
            return postList;
        }


       
        public void SavePostData(Post post)
        {
            var data = db.GetCollection<Post>("Post");
            post.IsActive = true;
            data.InsertOne(post);
            
        }

        public void UpdatePostData()
        {
            
        }

        /// <summary>
        /// it will be updated
        /// </summary>this method check whether a post is deleted or not if it is deleted,the system will update its'
        /// isActive property as false.
        /// <param name="userId"></param>
        /// <param name="posts"></param>
        /// <returns></returns>

        public void CheckIsDeleted(string userId, List<Post> posts)
        {
            List<Post> dbPosts = GetPostData(userId);
            if (posts != null)
            {
                foreach (var item in dbPosts)
                {
                    var postId = item.Id;
                    bool response = FindByPostId(userId, postId);
                    if (response == false) UpdatePostByIsActive(userId, postId);
                }
            }         
        }

        public bool FindByPostId(string userId, string postId)
        {
            var post = db.GetCollection<Post>("Post").Find(a => a.IgId.Equals(postId)).FirstOrDefault();
            if (post != null) return true;
            return false;  
        }

        /// <summary>
        /// if user deleted her or his post this method made it inactive in db using the method.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        public void UpdatePostByIsActive(string userId, string postId)
        {
            var filter = Builders<Post>.Filter.Eq(a => a.UserId, userId )&Builders<Post>.Filter.Eq(a=>a.Id,postId);
            var update = Builders<Post>.Update.Set(a=>a.IsActive,false);
            db.GetCollection<Post>("Post").UpdateOne(filter,update);
        }

        // this method will return all active post of person
        public List<Post> GetActivePostData(string id)
        {
          
            List<Post> activePosts = db.GetCollection<Post>("Post").Find(a=>a.UserId.Equals(id)&a.IsActive==true).ToListAsync().Result;
            return activePosts;
            
        }

        public void UpdateComments(string userId, string mediaId, List<Post> currentPost)
        {
            List<Post> activePostFromDb = GetActivePostData(userId);
            foreach(var item in currentPost)
            {
                foreach(var comment in item.Comments.Data)
                {
                   bool isCommentFound= FindComment(userId,item.Id,comment.Id);

                    if (isCommentFound != true)
                    {
                        //if comment not found we will save it to db.
                    }
                    
                }

            }
        }

        //this method will return bool according to that whether the comment there is in db or not.
        public bool FindComment(string id, string mediaId,string commentId)
        {
            Post post = db.GetCollection<Post>("Post").Find(a => a.UserId.Equals(id) & a.Id.Equals(mediaId)).FirstOrDefault();
            if (post.CommentsCount != 0)
            {
                foreach(var item in post.Comments.Data)
                {
                    if (item.Id.Equals(commentId)) return true;
                }
                return false;
            }
            return false;


        }

        public void UpdatePostCommentFromDb(string id, string mediId, CommentData comment)
        {
            Post post = db.GetCollection<Post>("Post").Find(a => a.UserId.Equals(id) & a.Id.Equals(mediId)).FirstOrDefault();
            if (post != null)
            {
                CommentData[] data = post.Comments.Data;
                var lenght = data.Length - 1;
                data[lenght] = comment;
                post.Comments.Data = data; 
                
            }

        }
    }
}
