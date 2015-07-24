namespace DatabaseApps.Client.DbManagers
{
    using DatabasseApps.SQLite;

    public class SQLiteDBManager
    {
        private readonly SQLiteContext context;

        public SQLiteDBManager()
        {
            this.context = new SQLiteContext();
        }

        public SQLiteContext SQLiteContext
        {
            get
            {
                return this.context;
            }
        }
    }
}
