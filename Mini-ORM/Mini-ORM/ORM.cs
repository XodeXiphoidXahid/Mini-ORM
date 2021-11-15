using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    public class ORM<T> where T : IEntity
    {
        private SqlConnection _sqlConnection;
        public ORM(SqlConnection connection)
        {
            _sqlConnection = connection;
        }
        public ORM(string connnectionString)
            : this(new SqlConnection(connnectionString))
        {

        }
        public void Insert(T item)
        {
            _sqlConnection.Open();

            var sql = new StringBuilder("Insert into ");
            var type = item.GetType();
            var properties = type.GetProperties();

            sql.Append(type.Name);
            sql.Append('(');

            foreach (var property in properties)
            {
                if (property.Name == "Id")
                    continue;
                sql.Append(' ').Append(property.Name).Append(',');
            }

            sql.Remove(sql.Length - 1, 1);
            sql.Append(") values(");

            foreach (var property in properties)
            {
                if (property.Name == "Id")
                    continue;
                sql.Append('@').Append(property.Name).Append(',');
            }

            sql.Remove(sql.Length - 1, 1);
            sql.Append(");");

            var query = sql.ToString();

            var command = new SqlCommand(query, _sqlConnection);

            foreach (var property in properties)
            {
                if (property.Name == "Id")
                    continue;
                var peram = new StringBuilder("@");
                peram.Append(property.Name);
                var peramList = peram.ToString();
                command.Parameters.AddWithValue(peramList, property.GetValue(item));
            }
            command.ExecuteNonQuery();
        }
        public void Update(T item)
        {
            _sqlConnection.Open();

            var tableName = TableName();
  
            var properties = item.GetType().GetProperties();
            var sql = new StringBuilder("Update ").Append(tableName).Append(" SET ");

            foreach (var property in properties)
            {
                if (property.Name == "Id")
                    continue;

                sql.Append(property.Name).Append("=@").Append(property.Name).Append(",");
            }
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" Where Id=@Id;");

            var query = sql.ToString();
            var command = new SqlCommand(query, _sqlConnection);

            foreach (var property in properties)
            {
                if (property.Name == "Id")
                    continue;
                var peram = new StringBuilder("@");
                peram.Append(property.Name);
                var peramList = peram.ToString();

                command.Parameters.AddWithValue(peramList, property.GetValue(item));
            }
            command.Parameters.AddWithValue("@Id", item.Id);
            command.ExecuteNonQuery();
        }
        public void Delete(T item)
        {
            Delete(item.Id);
        }
        public void Delete(int Id)
        {
            _sqlConnection.Open();

            var tableName = TableName();

            var sql = new StringBuilder("Delete From ").Append(tableName).Append(" Where Id=@Id");
            var query = sql.ToString();

            var command = new SqlCommand(query, _sqlConnection);
            command.Parameters.AddWithValue("@Id", Id);
            command.ExecuteNonQuery();
        }
        public T GetById(int id)
        {
            _sqlConnection.Open();

            var tableName = TableName();

            var sql = new StringBuilder("Select * From ").Append(tableName).Append(" Where Id=@Id");
            var query = sql.ToString();

            var command = new SqlCommand(query, _sqlConnection);
            command.Parameters.AddWithValue("@Id", id);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var item = ChangeType(reader);
                return item;
            }
            return default(T);
        }
        public IList<T> GetAll()
        {
            _sqlConnection.Open();

            var tableName = TableName();

            var itemList = new List<T>();
            var query = new StringBuilder("Select * From ").Append(tableName).ToString();

            var command = new SqlCommand(query, _sqlConnection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var item = ChangeType(reader);
                itemList.Add(item);
            }
            return itemList;
        }
        public string TableName()
        {
            var fullName = typeof(T).ToString().Split(".");
            var tableName = fullName[fullName.Length - 1];
            return tableName;
        }
        public T ChangeType(SqlDataReader reader)
        {
            var item = Activator.CreateInstance<T>();
            foreach (var property in typeof(T).GetProperties())
            {
                if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                {
                    var convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    property.SetValue(item, Convert.ChangeType(reader[property.Name], convertTo), null);
                }
            }
            return item;
        }
    }
}
