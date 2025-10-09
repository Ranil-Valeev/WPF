using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Справочник_Книг.model;

namespace Справочник_Книг
{
    class RecommendationService
    {
        private readonly Справочник_книгEntities _context;

        public RecommendationService()
        {
            _context = GetContext();
        }

        // Контекст базы данных (Singleton)
        private static Справочник_книгEntities _contextInstance;
        public static Справочник_книгEntities GetContext()
        {
            if (_contextInstance == null)
                _contextInstance = new Справочник_книгEntities();
            return _contextInstance;
        }

        /// <summary>
        /// Получение списка рекомендованных книг для пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="maxResults">Максимум книг в выдаче</param>
        public List<Книги> GetRecommendations(int userId, int maxResults = 10)
        {
            try
            {
                // 1. Получаем жанры, которые чаще всего встречаются у пользователя
                var favoriteGenres = _context.Прочитанные_Книги
                    .Where(p => p.ID_Пользователя == userId)
                    .Join(_context.Книги,
                          pk => pk.ID_Книги,
                          k => k.ID_Книги,
                          (pk, k) => k.ID_Жанра)
                    .GroupBy(j => j)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .ToList();

                if (!favoriteGenres.Any())
                    return new List<Книги>();

                // 2. Получаем список книг, которые уже были прочитаны
                var readBooks = _context.Прочитанные_Книги
                    .Where(p => p.ID_Пользователя == userId)
                    .Select(p => p.ID_Книги)
                    .ToList();

                // 3. Рекомендованные книги (из любимых жанров, но ещё не читанные)
                var recommendations = _context.Книги
                    .Where(k => favoriteGenres.Contains(k.ID_Жанра) && !readBooks.Contains(k.ID_Книги))
                    .Take(maxResults)
                    .ToList();

                return recommendations;
            }
            catch (Exception ex)
            {
                // Если ошибка – возвращаем пустой список
                Console.WriteLine("Ошибка в RecommendationService: " + ex.Message);
                return new List<Книги>();
            }
        }
    }
}
