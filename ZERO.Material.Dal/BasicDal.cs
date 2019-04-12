using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Linq.Expressions;
using ZERO.Material.IDal;
using ZERO.Material.Model;

namespace ZERO.Material.Dal
{
    public class BasicDal<T> : IBasicDal<T> where T : class, new()
    {

        private static readonly DbContext context = DbContextFactory.GetDbContext();

        /// <summary>
        /// 添加多个实体
        /// </summary>
        /// <param name="ts">实体集合</param>
        /// <returns></returns>
        public bool AddEntities(List<T> ts)
        {

            using (DbContext context = DbContextFactory.GetDbContext())
            {
                try
                {
                    for (int i = 0; i < ts.Count; i++)
                    {
                        if (ts[i] == null)
                        {
                            ts.Remove(ts[i]);
                        }
                    }
                    if (ts.Count == 0)
                    {
                        return true;
                    }

                    context.Set<T>().AddRange(ts);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool UpdateEntities(List<T> ts)
        {
            using (DbContext context = DbContextFactory.GetDbContext())
            {
                try
                {
                    foreach (T t in ts)
                    {
                        context.Entry(t).State = EntityState.Modified;
                    }

                    context.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                } 
            }

            return true;
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool DeleteEntity(List<T> ts)
        {
            try
            {
                using (DbContext context = DbContextFactory.GetDbContext())
                {
                    foreach (var t in ts)
                    {
                        context.Set<T>().Attach(t);
                        context.Set<T>().Remove(t);
                    }
                    int count = context.SaveChanges();
                    return count == ts.Count ? true : false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public T Find(params object[] keyValues)
        {
            using (DbContext context = DbContextFactory.GetDbContext())
            {
                return context.Set<T>().Find(keyValues); 
            }
        }

        /// <summary>
        /// 获取多个实体
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public List<T> GetEntities(Expression<Func<T, bool>> whereLambda)
        {

            using (DbContext context = DbContextFactory.GetDbContext())
            {
                return DbContextFactory.GetDbContext().Set<T>().Where(whereLambda).ToList(); 
            }
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T GetEntity(Expression<Func<T, bool>> whereLambda)
        {
            using (DbContext context = DbContextFactory.GetDbContext())
            {
                return context.Set<T>().FirstOrDefault(whereLambda); 
            }
        }

        /// <summary>
        /// 分页功能
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="orderLambda"></param>
        /// <param name="whereLambda"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<T> GetPageEntities<TKey>(int pageIndex, int pageCount, Expression<Func<T, TKey>> orderLambda, Expression<Func<T, bool>> whereLambda, out int total)
        {
            using (DbContext context = DbContextFactory.GetDbContext())
            {
                var queryable = context.Set<T>().Where(whereLambda);
                total = queryable.Count();
                return queryable.OrderBy(orderLambda).Skip((pageIndex - 1) * pageCount).Take(pageCount).ToList(); 
            }
        }

        public List<TB> ExecuteSqlCommand<TB>(string sql)
        {
            using (DbContext context = DbContextFactory.GetDbContext())
            {
                return context.Database.SqlQuery<TB>(sql).ToList(); 
            }
        }
    }
}