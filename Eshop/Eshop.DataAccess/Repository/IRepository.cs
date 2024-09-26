using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository
{
	public interface IRepository<T> where T : IEntity
	{
		IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int offset = 0, int limit = 0);
		int Count(Expression<Func<T, bool>>? filter = null);
		T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
		T Get(int id, string? includeProperties = null, bool tracked = false);
		T Add(T entity, bool save = false);
		T Update(T entity, bool save = false);
		void Detach(T entity);
		void Remove(T entity);
		void RemoveRange(IEnumerable<T> entity);
		void Save();
		bool IsStored(int id);
		bool AreStored(List<T> entities);
	}
}
