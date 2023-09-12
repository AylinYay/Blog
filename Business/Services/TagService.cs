using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework.Bases;
using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess;

namespace Business.Services
{
    public interface ITagService : IService<TagModel>
    {
        List<TagModel> GetList();
        TagModel GetItem(int id);
    }

    public class TagService : ITagService
    {
        private readonly RepoBase<Tag> _tagRepo;

        public TagService(RepoBase<Tag> tagRepo)
        {
            _tagRepo = tagRepo;
        }

        public Result Add(TagModel model)
        {
            if (_tagRepo.Exists(t => t.Name.ToLower() == model.Name.ToLower().Trim()))
            {
                return new ErrorResult("Tag with the same name exists!");
            }

            Tag tagEntity = new Tag()
            {
                Name = model.Name,
                IsPopular = model.IsPopular,               
            };

            _tagRepo.Add(tagEntity);

            return new SuccessResult("Tag added successfully.");
        }

        public Result Delete(int id)
        {
            // önce tag ile iliþkili blog tag kayýtlarý silinir
            _tagRepo.Delete<BlogTag>(bt => bt.TagId == id);

            // burada baþka bir yöntem olarak id yerine predicate (koþul) üzerinden kaydý siliyoruz
            _tagRepo.Delete(t => t.Id == id);
            //_tagRepo.Delete(id);

            return new SuccessResult();
        }

        public void Dispose()
        {
            _tagRepo.Dispose();
        }

        public Result Update(TagModel model)
        {
            // güncellenecek kayýt dýþýnda tag adý tabloda tekil olmalý
            if (_tagRepo.Exists(t => t.Name.ToUpper() == model.Name.ToUpper().Trim() && t.Id != model.Id))
                return new ErrorResult("Tag with the same name exists!");

            // burada baþka bir yöntem olarak yeni bir entity oluþturmak yerine id üzerinden mevcut
            // bir kaydý çekip özelliklerini model üzerinden güncelliyoruz
            Tag entity = _tagRepo.GetItem(model.Id);

            entity.Name = model.Name.Trim();
            entity.IsPopular = model.IsPopular;

            _tagRepo.Update(entity);

            return new SuccessResult();
        }

        public IQueryable<TagModel> Query()
        {
            return _tagRepo.Query().Select(t => new TagModel()
            {
                Guid = t.Guid,
                Id = t.Id,
                IsPopular = t.IsPopular,
                Name = t.Name,

                BlogCountDisplay = t.BlogTags.Count
            });
        }

        public List<TagModel> GetList()  // Listeleme
        {
            return Query().OrderBy(t => t.Name).Select(t => new TagModel()
            {
                Guid = t.Guid,
                Id = t.Id,
                IsPopular = t.IsPopular,
                Name = t.Name + (t.IsPopular ? " *" : ""),
                IsPopularDisplay = t.IsPopular ? "Yes" : "No",
                BlogCountDisplay = t.BlogCountDisplay
            }).ToList();
        }

        public TagModel GetItem(int id)
        {
            // 1. yöntem:
            //var entity = _tagRepo.Query().Include(t => t.BlogTags).SingleOrDefault(t => t.Id == id);
            //var model = new TagModel()
            //{
            //    Guid = entity.Guid,
            //    Id = entity.Id,
            //    IsPopular = entity.IsPopular,
            //    Name = entity.Name,
            //    IsPopularDisplay = entity.IsPopular ? "Yes" : "No",
            //    BlogCountDisplay = entity.BlogTags.Count 
            //};
            // 2. yöntem:
            var model = Query().Select(t => new TagModel()
            {
                Guid = t.Guid,
                Id = t.Id,
                IsPopular = t.IsPopular,
                Name = t.Name,
                IsPopularDisplay = t.IsPopular ? "Yes" : "No",
                BlogCountDisplay = t.BlogCountDisplay
            }).SingleOrDefault(t => t.Id == id);
            return model;
        }
    }
}
