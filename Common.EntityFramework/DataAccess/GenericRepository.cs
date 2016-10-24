using System;
using System.Collections;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Design.PluralizationServices;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Common.Exception;


namespace Common.EntityFramework.DataAccess
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
         where TEntity : class
    {
        private readonly PluralizationService _pluralizer = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));

        public TEntity GetByKey(object keyValue)
        {
            EntityKey key = GetEntityKey(keyValue);

            object originalItem;
            if (((IObjectContextAdapter)Context).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                return (TEntity)originalItem;
            }
            return default(TEntity);
        }

        public TEntity GetOrThrow(object keyValue)
        {
            var entityName = GetEntityName();
            var result = GetByKey(keyValue);
            if (result == null)
                throw new NotFoundException(string.Format("Can't find {0} {1}", entityName, keyValue));
            return result;
        }

        public IQueryable<TEntity> GetQuery()
        {
            var entityName = GetEntityName();

            return ((IObjectContextAdapter)Context).ObjectContext.CreateQuery<TEntity>(entityName);
        }

        public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate)
        {
            return GetQuery().Where(predicate);
        }


        public IQueryable<TEntity> Get<TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending)
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQuery().OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            return GetQuery().OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<TEntity> Get<TOrderBy>(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending)
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQuery(criteria).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            return GetQuery(criteria).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }


        public TEntity Single(Expression<Func<TEntity, bool>> criteria)
        {
            return GetQuery().Single(criteria);
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return GetQuery().FirstOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            Context.Set(entity.GetType()).Add(entity);
        }

        public void Attach(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Context.Set(entity.GetType()).Attach(entity);
        }

        public void AddRange(IEnumerable entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }
            Context.Set(typeof(TEntity)).AddRange(entities);
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            Context.Set(entity.GetType()).Remove(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> criteria)
        {
            IEnumerable records = Find(criteria);

            foreach (TEntity record in records)
            {
                Delete(record);
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            return GetQuery();
        }

        public TEntity Save(TEntity entity)
        {
            Add(entity);
            return entity;
        }

        public void Update(TEntity entity)
        {
            var fqen = GetEntityName();

            object originalItem;
            EntityKey key = ((IObjectContextAdapter)Context).ObjectContext.CreateEntityKey(fqen, entity);
            if (((IObjectContextAdapter)Context).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                ((IObjectContextAdapter)Context).ObjectContext.ApplyCurrentValues<TEntity>(key.EntitySetName, entity);
            }
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> criteria)
        {
            return GetQuery().Where(criteria);
        }

        public TEntity FindOne(Expression<Func<TEntity, bool>> criteria)
        {
            return GetQuery().Where(criteria).FirstOrDefault();
        }

        public int Count()
        {
            return GetQuery().Count();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return GetQuery().Any(predicate);
        }

        public int Count(Expression<Func<TEntity, bool>> criteria)
        {
            return GetQuery().Count(criteria);
        }

        public IUnitOfWork UnitOfWork
        {
            get
            {
                if (_unitOfWork == null)
                {
                    _unitOfWork = new UnitOfWork(this.Context);
                    //_unitOfWork.TokenExtractor = TokenExtractor;
                }
                return _unitOfWork;
            }
        }

        private EntityKey GetEntityKey(object keyValue)
        {
            var entitySetName = GetEntityName();
            var objectSet = ((IObjectContextAdapter)Context).ObjectContext.CreateObjectSet<TEntity>();
            var keyPropertyName = objectSet.EntitySet.ElementType.KeyMembers[0].ToString();
            var entityKey = new EntityKey(entitySetName, new[] { new EntityKeyMember(keyPropertyName, keyValue) });
            return entityKey;
        }

        private string GetEntityName()
        {
            return string.Format("{0}.{1}", ((IObjectContextAdapter)Context).ObjectContext.DefaultContainerName, _pluralizer.Pluralize(typeof(TEntity).Name));
            //return string.Format("{0}.{1}", ((IObjectContextAdapter)DbContext).ObjectContext.DefaultContainerName, typeof(TEntity).Name);
        }

        public DbContext Context { get; set; }

        private IUnitOfWork _unitOfWork;
    }
}
