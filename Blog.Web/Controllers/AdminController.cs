using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Blog.BLL.Helpers;
using Blog.BLL.Repository;
using Blog.DAL.Data;
using Blog.Models.Entities;
using Blog.Models.IdentityModels;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;


namespace Blog.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IRepository<Category, int> _categoryRepo;
        private readonly IRepository<Article, string> _articleRepo;
        private readonly IRepository<Photograph, string> _photoRepo;
        private readonly IRepository<Comment, string> _commentRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _env;


        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db, IHostingEnvironment env, IRepository<Category, int> categoryRepo, IRepository<Article, string> articleRepo, IRepository<Photograph, string> photoRepo, IRepository<Comment, string> commentRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
            _categoryRepo = categoryRepo;
            _articleRepo = articleRepo;
            _photoRepo = photoRepo;
            _commentRepo = commentRepo;
        }


        [HttpGet]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> BannerProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            return Json(user);
        }

        [HttpGet]
        public IActionResult CreateArticle()
        {
         

            var categories = _categoryRepo.GetAll().ToList();

            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArticle(ArticleViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            Article article = new Article();

            article.Title = model.Title;
            article.Content = model.Content;
            article.ExistingTime = DateTime.Now;

            var cat = _categoryRepo.GetAll(x => x.Name.ToString() == model.Category).ToList();



            foreach (var x in cat)
            {
                article.CategoryId = x.Id;
                break;
            }

            _articleRepo.Insert(article);


            if (model.PostedFile.Count > 0)
            {
                model.PostedFile.ForEach(async file =>
                {
                    if (file == null || file.Length <= 0)
                    {
                        var filepath2 = Path.Combine("/assets/images/image-not-available.png");

                        using (var fileStream = new FileStream(filepath2, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        _photoRepo.Insert(new Photograph()
                        {
                            ArticleId = article.Id,
                            Path = "/assets/images/image-not-available.png"
                        });

                        return;
                    }

                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var extName = Path.GetExtension(file.FileName);
                    fileName = StringHelpers.UrlFormatConverter(fileName);
                    fileName += StringHelpers.GetCode();
                    var webpath = _env.WebRootPath;
                    var directorypath = Path.Combine(webpath, "Uploads");
                    var filePath = Path.Combine(directorypath, fileName + extName);

                    if (!Directory.Exists(directorypath))
                    {
                        Directory.CreateDirectory(directorypath);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    _photoRepo.Insert(new Photograph()
                    {
                        ArticleId = article.Id,
                        Path = "/Uploads/" + fileName + extName
                    });

                });
            }

            var fotograflar = _photoRepo.GetAll(x => x.ArticleId == article.Id).ToList();
            var foto = fotograflar.Select(x => x.Path).ToList();
            article.PhotoPath = foto;
            _articleRepo.Update(article);


            var categories = _categoryRepo.GetAll().ToList();
            ViewBag.Categories = categories;

            TempData["Message"] = "Makaleniz başarı ile oluşturuldu.";


            return View();
        }

        [HttpGet]
        public IActionResult MyArticles()
        {
          

            var articles = _articleRepo.GetAll().Include(x => x.Category).Include(x => x.Photographs).Include(x => x.Comments).ToList();

            List<EditArticleViewModel> articlesVM = new List<EditArticleViewModel>();

            if (articles != null)
            {

                foreach (var item in articles)
                {

                    articlesVM.Add(new EditArticleViewModel()
                    {
                        Title = item.Title,
                        Category = item.Category.Name,
                        ExistingTime = item.ExistingTime,
                        Id = item.Id
                    });


                }


                return View(articlesVM);
            }
            else
            {
                return View(articlesVM);
            }
        }

        [HttpPost]
        public IActionResult MyArticles(int id)
        {
            return View();
        }

        [HttpPost]
        public JsonResult EditArticle(EditArticleViewModel model)
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

            var dbArt = (_articleRepo as ArticleRepo);
            var dbCat = (_categoryRepo as CategoryRepo);

            var article = dbArt.GetAll().Include("Category")
                .FirstOrDefault(x => x.Id == model.Id);

            var category = dbCat.GetAll().FirstOrDefault(x => x.Name == model.Category);

            if (category != null)
            {
                article.Category.Name = model.Category;
            }
            else
            {
                return Json(new ResponseData()
                {
                    Success = false,
                    Message = "Kaydetmek istediğiniz isimde kategori mevcut değildir."
                });
            }

            article.Title = model.Title;
            article.ExistingTime = model.ExistingTime;

            dbArt.Save();


            return Json(new ResponseData()
            {
                Success = true
            });
        }

        [HttpGet]
        public IActionResult EditArticleContent(string id)
        {
           
            var article = _articleRepo.GetAll().Include(x => x.Category)
                .FirstOrDefault(x => x.Id == id);

            EditArticleContentViewModel model = new EditArticleContentViewModel()
            {
                Content = article.Content,
                Title = article.Title
              
            };


            return View(model);
        }

        [HttpPost]
        public IActionResult EditArticleContent(EditArticleContentViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var article = _articleRepo.GetAll().Include(x => x.Category)
                .FirstOrDefault(x => x.Id == model.Id);

            article.Content = model.Content;
            model.Title = article.Title;

            _articleRepo.Save();

            TempData["Message"] = "Makalenizin içeriği başarı ile değiştirildi.";
            return View(model);

        }

        [HttpGet]
        [Route("/Admin/DeleteArticle/{data}")]
        public JsonResult DeleteArticle([FromRoute]string data)
        {

            ArticleViewModel model = new ArticleViewModel();

            var article = _articleRepo.GetById(data);
            var photos = _photoRepo.GetAll().Include(x => x.Article).Where(x => x.ArticleId == article.Id).ToList();
            var comments = _commentRepo.GetAll(x => x.ArticleId == article.Id).ToList();

            foreach (var photo in photos)
            {
                _photoRepo.Delete(photo);
            }

            foreach (var comment in comments)
            {
                _commentRepo.Delete(comment);
            }

            _articleRepo.Delete(article);

            return Json(new ResponseData()
            {
                Success = true,
                Message = "Makale silme işlemi başarılı"
            });
        }

        [HttpPost]
        public JsonResult CreateCategory(CategoryInsertViewModel model)
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

     

            var category = _categoryRepo.GetAll().FirstOrDefault(x => x.Name == model.Name);

            if (category == null)
            {
                _categoryRepo.Insert(new Category()
                {
                    Name = model.Name,

                });

                return Json(new ResponseData()
                {
                    Data = model.Name,
                    Success = true,
                    Message = "Kategori başarı ile eklenmiştir."
                });

            }
            else
            {
                return Json(new ResponseData()
                {
                    Success = false,
                    Message = "Bu isimde bir kategori zaten mevcuttur."
                });
            }


        }

        [HttpGet]
        public IActionResult EditArticleComment(string id)
        {
            

            var article = _articleRepo.GetAll().Include(x => x.Category).Include(x => x.Comments)
                .FirstOrDefault(x => x.Id == id);

            List<CommentViewModel> comments = new List<CommentViewModel>();

            foreach (var comment in article.Comments)
            {
                comments.Add(new CommentViewModel()
                {
                    Name = comment.Name,
                    Email = comment.Email,
                    Content = comment.Content,
                    Id = comment.Id
                });
            }

            TempData["Title"] = $"\" {article.Title} \" İsimli Makalenin Yorumları";
            return View(comments);
        }

        [HttpGet]
        [Route("/Admin/DeleteComment/{data}")]
        public IActionResult DeleteComment([FromRoute]string data)
        {
     

            var comment = _commentRepo.GetById(data);
            var article = _articleRepo.GetById(comment.ArticleId);

            _commentRepo.Delete(comment);

            List<CommentViewModel> comments = new List<CommentViewModel>();

            foreach (var item in article.Comments)
            {
                comments.Add(new CommentViewModel()
                {
                    Name = item.Name,
                    Email = item.Email,
                    Content = item.Content,
                    Id = item.Id
                });
            }

            return Json(new ResponseData()
            {
                Success = true,
                Data = article.Id

            });
            
        }
    }
}