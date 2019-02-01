using DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;

namespace DataLayer
{
    public interface IDataService
    {
        //------search-----------
        List<SearchHistories> SearchHistories(int id);
        SearchHistories SaveSearch(string newSearch, int newUserId);

        //--------Answers--------
        List<Answer> GetAnswers();
        Answer GetAnswer(int id);
        List<Answer> GetAnswersByParent(int id);
        int GetNumberOfAnswers();
        List<Comment> GetCommentsByAnswer(int id);

        //-------Questions--------
        List<Comment> GetQuestionComments(int id);
        List<SearchResult> GetQuestionsByString(string title, int page, int pageSize);
        int GetNumberOfQuestions();
        int GetNumberOfSearches();
         List<PostLink> GetPostLinksByQuestion(int id);
        List<Question> GetQuestions(int page, int pageSize);
        Question GetQuestion(int id);

        //------Users----------
        List<User> GetUsers();
        User GetUser(int id);
        User CreateUser(string name, string password);
        bool DeleteUser(int id);
        bool UpdateUser(int userId, string newName, string newPassword);
        
        //-------Annotations-------
        Annotations CreateAnnotation(string body, int userid, int postid);
        bool UpdateAnnotation(string body, int id);
        bool DeleteAnnotation(int id);
        List<Annotations> GetAnnotations(int userid, int page, int pageSize);
        int GetNumberOfAnnotations();
        Annotations GetAnnotation(int userid, int AnnoId);

        //----------Marked------------------
        Mark CreateMarking(int postid, int userid);
        List<Mark> GetMarks(int userid, int page, int pageSize);
        bool DeleteMarking(int userid, int postid);
        int GetNumberOfMarks();
        Mark GetMark(int userid, int postid);

        List<WordCloud> GetWordCloud(string word);



    }


    public class DataService : IDataService
    {
        //Questions
        
        public List<Question> GetQuestions(int page, int pageSize)
        {
            using (var db = new SOVAContext())
            {
                var questions = db.Questions
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();
                    
                return questions;
            }
        }

        public Question GetQuestion(int id) 
        {
            using (var db = new SOVAContext())
            {
                var question = db.Questions
                    .FirstOrDefault(x => x.Id == id);
                return question;
            }
        }






        //Comments
        public List<Comment> GetQuestionComments(int id)
        {
            using (var db = new SOVAContext())
            {

                var commentsToQuestion = db.Comments
                      
                      .Where(x => x.PostId == id)
                      .ToList();
                return commentsToQuestion;
            }
        }

        public List<Comment> GetCommentsByAnswer(int id)
        {
            using (var db = new SOVAContext())
            {
                var answercomment = db.Comments
                    .Where(x => x.PostId == id)
                    .ToList();
                return answercomment;
            }

        }

    
        public List<SearchResult> GetQuestionsByString(string title, int page, int pageSize)
        {
            using (var db = new SOVAContext())
            {
                var question = db.SearchResults.FromSql("select * from TFIDF_MATCH({0})", title);
                return question
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();

            }
        }
        
        public List<WordCloud> GetWordCloud(string word)
        {
            using (var db = new SOVAContext())
            {
                var cloud = db.WordClouds.FromSql("select * from wrdCloud({0})", word);
                return cloud.ToList();
            }
        }
        //Answers

        public List<Answer> GetAnswers()
        {
            using (var db = new SOVAContext())
            {
                var answers = db.Answers.ToList();
                    
                return answers;
            }
        }
       
        public Answer GetAnswer(int id)
        {
            using (var db = new SOVAContext())
            {
                var answer = db.Answers
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
                    return answer;
            }
        }
        public List<Answer> GetAnswersByParent(int id)
        {
            using (var db = new SOVAContext())
            {
                var answerbyparent = db.Answers
                    .Where(x => x.ParentId == id);
                return answerbyparent.ToList();
            }
        }

        public int GetNumberOfAnswers()
        {
            using (var db = new SOVAContext())
            {
                return db.Answers.Count();
            }
        }
        public int GetNumberOfQuestions()
        {
            using (var db = new SOVAContext())
            {
                return db.Questions.Count();
            }
        }

        

        //-----------------PostLinks--------------------------
        public List<PostLink> GetPostLinksByQuestion(int id)
        {
            using (var db = new SOVAContext())
            {
                var qpostlinks = db.PostLinks
                    .Where(x => x.Id == id);
                return qpostlinks.ToList();
            }
        }
        

        //----------------users----------------
        public List<User> GetUsers()
        {
            using (var db = new SOVAContext())
            {
                var userss = db.Users
                    .ToList();
                return userss;
            }
        }

        public User GetUser(int id)
        {
            using (var db = new SOVAContext())
            {
                var user = db.Users
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
                return user;
            }
        }

 
        public User CreateUser(string name, string password)
        {
            using (var db = new SOVAContext())
            {


                var creationdate = DateTime.Now;
                var newUser = new User()
                {

                    UserName = name,
                    Password = password,
                    CreationDate = creationdate
                };
                db.Users.Add(newUser);
                db.SaveChanges();
                return newUser;


            }
        }

        public bool DeleteUser(int id)
        {

            try
            {
                using (var db = new SOVAContext())
                {
                    var deluser = db.Users.Find(id);
                    db.Users.Remove(deluser);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        // when update is run the createtionDate is updated to. Need to be fixed. 
        public bool UpdateUser(int userId, string newName, string newPassword)
        {
            using (var db = new SOVAContext())
            {
                var user = db.Users.FirstOrDefault(x => x.Id == userId);
                if (user != null)
                {
                    user.UserName = newName;
                    user.Password = newPassword;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        //--------------------- Searchs ----------------
        public SearchHistories SaveSearch(string newSearch, int newUserId)
        {
            using (var db = new SOVAContext())
            {
                var currentDate = DateTime.Now;
                var newSearchHistory = new SearchHistories()
                {
                    Search = newSearch,
                    UserId = newUserId,
                    Date = currentDate
                };
                db.SearchHistory.Add(newSearchHistory);
                db.SaveChanges();
                return newSearchHistory;
            }
        }

        public List<SearchHistories> SearchHistories(int id)
        {
            using (var db = new SOVAContext())
            {
                var question = db.SearchHistory
                    .Where(x => x.UserId == id)
                    .OrderByDescending(x => x.Date)
                    .ToList();
                    //.FirstOrDefault();
                return question;
            }
        }

        public int GetNumberOfSearches()
        {
            using (var db = new SOVAContext())
            {
                return db.SearchResults.Count();
            }
        }

        //---------------- ANNOTATIONS ------------------



        public Annotations CreateAnnotation(string body, int userid, int postid)
        {
            using (var db = new SOVAContext())
            {
                var creationdate = DateTime.Now;
                var newannotation = new Annotations()
                {
                   
                   CreationDate = creationdate,
                    Body = body,
                    UserId = userid,
                    PostId = postid
                    
                };
                db.Annotations.Add(newannotation);
                db.SaveChanges();
                return newannotation;
            }
        }

        public int GetNumberOfAnnotations()
        {
            using (var db = new SOVAContext())
            {
                return db.Annotations.Count();
            }
        }

        public Annotations GetAnnotation(int userid, int AnnoId)
        {
            using (var db = new SOVAContext())
            {
                var annotation = db.Annotations
                    .FirstOrDefault(x => x.UserId == userid && x.Id == AnnoId);
                return annotation;

            }

        }

        public List<Annotations> GetAnnotations(int userid, int page, int pageSize)
        {
            using (var db = new SOVAContext())
            {
                var annotations = db.Annotations
                    .Where (x => x.UserId == userid)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();

                return annotations;
           
            }
        }
    
        public bool UpdateAnnotation(string body, int id)
        {
            using (var db = new SOVAContext())
            {
                var anno = db.Annotations.FirstOrDefault(x => x.Id ==id); 
                if (anno != null)
                {
                    anno.Body = body;
                    db.SaveChanges();
                    return true;

                }
                return false;
            }
        }



        public bool DeleteAnnotation( int id)
        {
            try
            {
                using (var db = new SOVAContext())
                {
                    var delannotation = new Annotations()
                    {
                        Id=id
                      
                    };
                    db.Annotations.Remove(delannotation);
                    db.SaveChanges();

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
          
            
        public Mark CreateMarking(int postid, int userid)
        {
            using (var db = new SOVAContext())
            {

                var creationdate = DateTime.Now;
                var newmark = new Mark()
                {
                
                    CreationDate = creationdate,
                    PostId = postid,
                    UserId = userid
                };
                db.Marked.Add(newmark);
                db.SaveChanges();
                return newmark;
            }
        }

        public Mark GetMark(int userid, int postid)
        {
            using (var db = new SOVAContext())
            {
                var mark = db.Marked
                    .FirstOrDefault(x => x.UserId == userid && x.PostId == postid);
                return mark;
                
            }
            
        }

        public int GetNumberOfMarks()
        {
            using (var db = new SOVAContext())
            {
                return db.Marked.Count();
            }
        }

        public List<Mark> GetMarks(int userid, int page, int pageSize)
        {
            using (var db = new SOVAContext())
            {
                var marks = db.Marked
                    .Where (x => x.UserId == userid)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();
                //.Where(x => x.UserId == id)
                //.ToList(); 
                return marks;
           
            }
        }

        public bool DeleteMarking(int userid,int postid)
        {
            try
            {
                using (var db = new SOVAContext())
                {
                    var delmarking = new Mark()
                    {
                        UserId=userid,
                        PostId=postid
                       
                    
                      
                    };
                    db.Marked.Remove(delmarking);
                    db.SaveChanges();

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
