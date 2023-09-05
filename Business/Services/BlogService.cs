using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework.Bases;
using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess;

namespace Business.Services
{
    public interface IBlogService : IService<BlogModel>
    {
    }

    public class BlogService : IBlogService
    {
        private readonly RepoBase<Blog> _blogRepo;

        public BlogService(RepoBase<Blog> blogRepo)
        {
            _blogRepo = blogRepo;
        }

        public IQueryable<BlogModel> Query()
        {
            // eğer istenirse Select ile projeksyion işlemi AutoMapper kütüphanesi üzerinden otomatik hale getirilebilir
            return _blogRepo.Query().OrderByDescending(b => b.CreateDate).ThenBy(b => b.Title).Select(b => new BlogModel()
            {
                Content = b.Content,
                CreateDate = b.CreateDate,
                Guid = b.Guid,
                Id = b.Id,
                Score = b.Score,
                Title = b.Title,
                UpdateDate = b.UpdateDate,
                UserId = b.UserId,

                UserNameDisplay = b.User.UserName,
                CreateDateDisplay = b.CreateDate.ToString("MM/dd/yyyy HH:mm"),
                UpdateDateDisplay = b.UpdateDate.HasValue ? b.UpdateDate.Value.ToString("MM/dd/yyyy HH:mm") : "",

                // Many to many ilişkili kayıtları getirme 1. yöntem
                TagsDisplay = b.BlogTags.Select(bt => new TagModel()
                {
                    Guid = bt.Tag.Guid,
                    Id = bt.Tag.Id,
                    IsPopular = bt.Tag.IsPopular,
                    Name = bt.Tag.Name
                }).ToList()

                // Many to many ilişkili kayıtları getirme 2. yöntem
                //TagsDisplay = string.Join("<br/>", b.BlogTags.Select(bt => bt.Tag.Name))
            });
        }

        public Result Add(BlogModel model)
        {
            Blog entity = new Blog()
            {
                Content = model.Content.Trim(),
                CreateDate = DateTime.Now,
                Score = model.Score,
                Title = model.Title.Trim(),
                UserId = model.UserId.Value,
                //UserId = model.UserId ?? 0

                BlogTags = model.TagIds.Select(tagId => new BlogTag()
                {
                    TagId = tagId
                }).ToList()
            };

            _blogRepo.Add(entity);

            return new SuccessResult("Blog added successfully.");
        }

        public Result Update(BlogModel model)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _blogRepo.Dispose();
        }
    }
}
