﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using ZERO.Material.IDal;
using ZERO.Material.Model;

namespace ZERO.Material.Dal
{
    public class BasicDal<T> : IBasicDal<T> where T : class, new()
    {
        protected readonly ZeroMaterialContext ZeroMaterialEntities = new ZeroMaterialContext();

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool AddEntity(List<T> ts)
        {
            foreach (T t in ts)
            {
                ZeroMaterialEntities.Set<T>().Add(t);
            }

            return ZeroMaterialEntities.SaveChanges() == ts.Count;
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool DeleteEntity(List<T> ts)
        {
            foreach (T t in ts)
            {
                ZeroMaterialEntities.Set<T>().Remove(t);
            }

            return ZeroMaterialEntities.SaveChanges() == ts.Count;
        }

        /// <summary>
        /// 获取多个实体
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public List<T> GetEntities(Expression<Func<T, bool>> whereLambda)
        {
            return ZeroMaterialEntities.Set<T>().Where(whereLambda).ToList();
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T GetEntity(Expression<Func<T, bool>> whereLambda)
        {
            return ZeroMaterialEntities.Set<T>().FirstOrDefault(whereLambda);
        }

        public List<T> GetPageEntities<TKey>(int pageIndex, int pageCount, Expression<Func<T, TKey>> orderLambda, out int total)
        {
            total = pageCount;
            return ZeroMaterialEntities.Set<T>().OrderBy(orderLambda).Skip((pageIndex - 1) * pageCount).Take(pageCount).ToList();
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool UpdateEntity(List<T> ts)
        {
            foreach (T t in ts)
            {
                ZeroMaterialEntities.Set<T>().AddOrUpdate(t);
            }

            return ZeroMaterialEntities.SaveChanges() == ts.Count;
        }
    }
}