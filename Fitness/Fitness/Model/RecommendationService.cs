using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Model
{
    
    class RecommendationService
    {
        private readonly Фитнес_ЗалEntities _context;

        public RecommendationService()
        {
            _context = new Фитнес_ЗалEntities();
        }

        /// <summary>
        /// Возвращает список рекомендаций для клиента на основе посещенных тренировок.
        /// </summary>
        public List<Рекомендации> GetClientRecommendations(int clientId)
        {
            var тренировкиКлиента = _context.Посещения
                .Where(p => p.Id_клиента == clientId)
                .Select(p => p.Id_тренировки)
                .Distinct()
                .ToList();

            return _context.Рекомендации
                .Where(r => тренировкиКлиента.Contains(r.Id_тренировки))
                .ToList();
        }
    }
}
