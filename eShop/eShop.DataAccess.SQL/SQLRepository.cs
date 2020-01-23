﻿using eShop.Core.Contracts;
using eShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.DataAccess.SQL
{
	public class SQLRepository<T> : IRepository<T> where T : BaseEntity
	{
		internal DataContext context;
		internal DbSet<T> dbSet;

		public SQLRepository(DataContext context)
		{
			this.context = context;
			this.dbSet = context.Set<T>();
		}
		public IQueryable<T> Collection()
		{
			return dbSet;
		}

		public void Commit()
		{
			context.SaveChanges();
		}

		public void Delete(string Id)
		{
			//find what we need to delete
			var t = Find(Id);

			//we ask if t is already detached from dbContext by checking entity state
			if (context.Entry(t).State == EntityState.Detached)
			{
				//if it is attach it then
				dbSet.Attach(t);
			}

			//delete it.
			dbSet.Remove(t);
		}

		public T Find(string Id)
		{
			return dbSet.Find(Id);
		}

		public void Insert(T t)
		{
			dbSet.Add(t);
		}

		public void Update(T t)
		{
			//attach it to dbSet and change state of entity to modified
			//that way when update time comes, table will be updated
			dbSet.Attach(t);
			context.Entry(t).State=EntityState.Modified;
		}
	}
}
