using Blog.BLL.Repository;
using Blog.BLL.Services;
using Blog.DAL.Data;
using Blog.Models.Entities;
using Blog.Models.IdentityModels;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Category, int> _categoryRepo;
        private readonly IRepository<Article, string> _articleRepo;
        private readonly IRepository<Photograph, string> _photoRepo;
        private readonly IRepository<Comment, string> _commentRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _db;



        public HomeController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db, IRepository<Category, int> categoryRepo, IRepository<Article, string> articleRepo, IRepository<Photograph, string> photoRepo, IRepository<Comment, string> commentRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;
            _categoryRepo = categoryRepo;
            _articleRepo = articleRepo;
            _photoRepo = photoRepo;
            _commentRepo = commentRepo;
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();

        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();

        }


        [HttpGet]
        public IActionResult Index(int id)
        {

            try
            {
               
               
               
                List<Article> articles;
                var control = false;

                if (id == 0)
                {
                    articles = _articleRepo.GetAll().Include(x => x.Photographs).Include(x => x.Category).Include(x => x.Comments).ToList();
                    control = true;
                }
                else
                {
                    articles = _articleRepo.GetAll().Include(x => x.Photographs).Include(x => x.Category).Include(x => x.Comments).ToList();
                    articles = articles.Where(x => x.Category.Id == id).ToList();
                }
               

                List<ArticleViewModel> articlesVM = new List<ArticleViewModel>();

                if (articles != null)
                {

                    foreach (var item in articles)
                    {

                        List<string> tempPhotoPaths = new List<string>();

                        foreach (var item1 in item.Photographs)
                        {

                            tempPhotoPaths.Add(item1.Path);
                        }

                        articlesVM.Add(new ArticleViewModel()
                        {
                            Title = item.Title,
                            Category = item.Category.Name,
                            Content = item.Content,
                            ExistingTime = item.ExistingTime,
                            PhotoPath = tempPhotoPaths,
                            Id = item.Id,
                            CommentsCount = item.Comments.Count

                        });


                    }

                    ViewBag.All = control;
                    return View(articlesVM);
                }
                else
                {
                    return View(articlesVM);
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public JsonResult IndexCategories()
        {
     
            var categories = _categoryRepo.GetAll().ToList();

            return Json(new ResponseData()
            {
                Success = true,
                Data = categories
   
            });
        }

      

        [HttpGet]
        [Route("/Home/BlogPage/{artId}")]
        public IActionResult BlogPage([FromRoute]string artId)
        {

            
            List<Article> articles;


            articles = _articleRepo.GetAll().Include(x => x.Photographs).Include(x => x.Category).Include(x => x.Comments).ToList();

            Article article = articles.FirstOrDefault(x => x.Id == artId);

            List<string> tempPhotoPaths = new List<string>();

            foreach (var item in article.Photographs)
            {
                tempPhotoPaths.Add(item.Path);
            }

            ArticleViewModel articleVm = new ArticleViewModel()
            {
                Id = article.Id,
                Category = article.Category.Name,
                Content = article.Content,
                Title = article.Title,
                ExistingTime = article.ExistingTime.Date,
                PhotoPath = tempPhotoPaths,
                CommentsCount = article.Comments.Count
            };


            return View(articleVm);
        }

        [HttpGet]
        [Route("/Home/BlogComment/{id}")]
        public JsonResult BlogComment([FromRoute]string id)
        {
            List<Comment> comments = null;

            if (id != null)
            {

                 comments = _commentRepo.GetAll(x => x.ArticleId == id).ToList();

            }

            List<CommentViewModel> commentsVM = new List<CommentViewModel>();

            foreach (var comment in comments)
            {
                if (comment.isConfirmed)
                {
                    commentsVM.Add(new CommentViewModel()
                    {
                        Name = comment.Name,
                        Content = comment.Content,
                        Email = comment.Email,
                        ArticleId = comment.ArticleId,
                        ExistingTime = comment.ExistingTime.ToString()
                    });
                }
                 
            }
        


            return Json(new ResponseData()
            {
                Success = true,
                Data = commentsVM
            });
        }

        [HttpPost]
        public async Task<JsonResult> BlogComment(CommentViewModel model)
        {

            var errMsg = "";

            if (!ModelState.IsValid)
            {
                foreach (var modelStateValue in ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        errMsg += error.ErrorMessage + "\n";
                    }
                }
                return Json(new ResponseData()
                {
                    Success = false,
                    Message = "Bir hata oluştu, " + errMsg
                });
            }

           
            Comment comment = new Comment()
            {
                ArticleId = model.ArticleId,
                Content = model.Content,
                Email = model.Email,
                Name = model.Name,
                ExistingTime = DateTime.Now,
                isConfirmed = false
            };

            _commentRepo.Insert(comment);


        
            var article = _articleRepo.GetById(model.ArticleId);


            var users =  await _userManager.GetUsersInRoleAsync("Admin");
            string targetEmail = "";

            foreach (var user in users)
            {
                targetEmail = user.Email;
            }

            string siteUrl = HttpContext.Request.GetEncodedUrl().Substring(0, 23);

            var emailService = new EmailService();

            var body = $"<b>{model.Name} </b> İsimli kişi \"{article.Title} \" adlı makalenize şu yorumu yapmak istiyor : \" {model.Content} \" <br> Yorumun sitenizde görünmesini istiyorsanız: <a href='{siteUrl}/Home/ConfirmComment/{comment.Id}'>Onayla</a> linkine tıklayın.<br>İstemiyorsanız bu maili görmezden gelebilirsiniz.";

            await emailService.SendAsync(new MessageViewModel()
            {
                Body = body,
                Subject = "Yorum"

            }, targetEmail);

           
            return Json(new ResponseData()
            {
                Success =true,
                Message = "Yorumunuz Gönderilmiştir Ve Admin Onayını Beklemektedir."
                
            });
        }

        [HttpGet]
        [Route("/Home/ConfirmComment/{id}")]
        public IActionResult ConfirmComment([FromRoute]string id)
        {

            var comment = _commentRepo.GetById(id);

            comment.isConfirmed = true;

            _commentRepo.Save();

            return RedirectToAction("Index","Home",10);
        }


    }
}
