namespace DatabaseApps.Client.DbManagers
{
    using MySql;

    public class MysqlDBManager
    {
        private readonly MySQLContext context;

        public MysqlDBManager()
        {
            this.context = new MySQLContext();
        }

        public MySQLContext MySqlContext
        {
            get
            {
                return this.context;
            }
        }
    }
}
