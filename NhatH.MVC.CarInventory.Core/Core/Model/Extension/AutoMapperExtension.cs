using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatH.MVC.CarInventory.Core.Core.Model.Extension
{
    public static class AutoMapperExtension
    {
        /*public static TModel ToModel<TEntity, TModel>(this TEntity entity) where TEntity : BaseEntity
        {
            AutoMapper.Mapper.CreateMap<TEntity, TModel>();
            return AutoMapper.Mapper.Map<TEntity, TModel>(entity);
        }*/

        /*  public static TEntity ToEntity<TModel, TEntity>(this TModel model) where TEntity : BaseEntity
          {
              AutoMapper.Mapper.CreateMap<TModel, TEntity>();
              return AutoMapper.Mapper.Map<TModel, TEntity>(model);
          }

          public static TEntity ToEntity<TModel, TEntity>(this TModel model, TEntity entity) where TEntity : BaseEntity
          {
              AutoMapper.Mapper.CreateMap<TModel, TEntity>();
              return AutoMapper.Mapper.Map(model, entity);
          }

          public static TEntity ToEntity<TModel, TEntity>(this TModel model, TEntity entity,
                                                          Expression<Func<TEntity, object>> destinationMember,
                                                          Action<IMemberConfigurationExpression<TModel>> memberOptions)
              where TEntity : BaseEntity
          {
              AutoMapper.Mapper.CreateMap<TModel, TEntity>().ForMember(destinationMember, memberOptions);
              return AutoMapper.Mapper.Map(model, entity);
          }*/

        //public static void IgnoreMemberOption<T>(Action<IMemberConfigurationExpression<T>> memberOptions,
        //                                         IMemberConfigurationExpression<T> para)
        //{
        //    // ignore something.

        //    // 
        //    memberOptions(para);
        //}

        public static List<TModel> ToModel<TEntity, TModel>(this List<TEntity> entity) where TEntity : class
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TEntity, TModel>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<List<TEntity>, List<TModel>>(entity);
        }
    }
}
