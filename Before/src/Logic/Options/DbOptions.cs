namespace Logic.Options
{
    /// <summary>
    /// Конфигурация работы с БД
    /// </summary>
    public sealed class DbOptions
    {
        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Количество попыток доступа к БД
        /// </summary>
        public int NumberOfDatabaseRetries { get; set; }
    }
}
