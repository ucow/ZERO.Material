using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ZERO.Material.IDal;
using ZERO.Material.Model;

namespace ZERO.Material.Dal
{
    public class BasicDal<T> : IBasicDal<T> where T : class, new()
    {
        private readonly DbContext _context = DbContextFactory.GetDbContext();

        /// <summary>
        /// 添加多个实体
        /// </summary>
        /// <param name="ts">实体集合</param>
        /// <returns></returns>
        public bool AddEntities(List<T> ts)
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

            _context.Set<T>().AddRange(ts);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool UpdateEntities(List<T> ts)
        {
            foreach (T t in ts)
            {
                _context.Entry(t).State = EntityState.Modified;
            }

            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool DeleteEntity(List<T> ts)
        {
            _context.Set<T>().RemoveRange(ts);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public T Find(params object[] keyValues)
        {
            return _context.Set<T>().Find(keyValues);
        }

        /// <summary>
        /// 获取多个实体
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public List<T> GetEntities(Expression<Func<T, bool>> whereLambda)
        {
            return _context.Set<T>().Where(whereLambda).ToList();
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T GetEntity(Expression<Func<T, bool>> whereLambda)
        {
            return _context.Set<T>().FirstOrDefault(whereLambda);
        }

        /// <summary>
        /// 分页功能
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="orderLambda"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<T> GetPageEntities<TKey>(int pageIndex, int pageCount, Expression<Func<T, TKey>> orderLambda, out int total)
        {
            total = _context.Set<T>().Count();
            return _context.Set<T>().OrderBy(orderLambda).Skip((pageIndex - 1) * pageCount).Take(pageCount).ToList();
        }
    }
}