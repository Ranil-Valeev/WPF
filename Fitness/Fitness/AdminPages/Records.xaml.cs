using Fitness.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Fitness.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для Records.xaml
    /// </summary>
    public partial class Records : Page
    {
        private Фитнес_ЗалEntities db = new Фитнес_ЗалEntities();
        public Records()
        {
            InitializeComponent();
            LoadTrainingActions();
        }
        private void LoadTrainingActions()
        {
            try
            {
                var rawData = db.Журнал
                    .Include("Клиенты") // Загружаем связанные данные клиентов
                    .Where(j => j.Действие == "Запись на тренировку" || j.Действие == "Отмена записи")
                    .OrderByDescending(j => j.Дата_и_время)
                    .ToList();

                // Теперь форматируем данные в памяти
                var formattedData = rawData.Select(j => new
                {
                    Id_журнала = j.Id_журнала, // Добавляем уникальный ID для отладки
                    Логин = j.Клиенты?.Логин ?? "Не указан",
                    Email = j.Клиенты?.Email ?? "Не указан",
                    Действие = j.Действие ?? "Неизвестно",
                    Дата_и_время = j.Дата_и_время.ToString("dd.MM.yyyy HH:mm:ss")
                }).ToList();

                ActionsDataGrid.ItemsSource = formattedData;

                // Для отладки - показываем количество записей
                Console.WriteLine($"Загружено записей: {formattedData.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }
        public void RefreshData()
        {
            LoadTrainingActions();
        }

    

    }
}
