using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Data.Utility
{
    public static class SapB1QueryBuilder
    {
        private static string GetParametersToText(int quantity, string character = "?")
        {
            var param = "";
            for (var i = 0; i < quantity; i++)
            {
                if (!i.Equals(0))
                    param += ",";
                param += $" {character}";
            }
            return param;
        }

        private static List<int> GetParametersIndexFromQuery(string query)
        {
            var indexList = new List<int>();
            var fromIndex = 0;
            while (fromIndex < query.Length)
            {
                var index = query.IndexOf("?", fromIndex);
                if (index != -1)
                {
                    indexList.Add(index);
                    fromIndex = index + 1;
                }
                else
                    fromIndex = query.Length;
            }

            return indexList;
        }

        private static Dictionary<int, string> GetParameters(List<dynamic> parameters)
        {
            var sqlParameters = new Dictionary<int, string>();
            var i = 0;
            foreach (var p in parameters)
            {
                if (p is string)
                    sqlParameters.Add(i, $"'{p}'");
                else if (p is DateTime)
                    sqlParameters.Add(i, $"'{p:yyyy-MM-dd}'");
                else
                    sqlParameters.Add(i, p.ToString());
                i++;
            }

            return sqlParameters;
        }

        public static string BuildQuery(string spName, List<dynamic> parameters = null, Stream stream = null)
        {
            var sqlCommand = string.Empty;

            if (stream == null)
                sqlCommand = $"EXEC [dbo].[{spName}]{GetParametersToText(parameters?.Count ?? 0)}";
            else
            {
                var reader = new StreamReader(stream);
                while (!reader.EndOfStream)
                    sqlCommand += reader.ReadLine() + " ";
            }

            if (parameters == null)
                return sqlCommand;

            var parametersIndex = GetParametersIndexFromQuery(sqlCommand);
            var sqlParameters = GetParameters(parameters);

            for (var i = parametersIndex.Count - 1; i >= 0; i--)
            {
                var indStr = parametersIndex[i];
                if (!sqlParameters.ContainsKey(i))
                    throw new Exception("no se ha definido un valor para el parámetro " + i);
                var param = sqlParameters[i];
                sqlCommand = sqlCommand.Substring(0, indStr) + param + sqlCommand[(indStr + 1)..];
            }

            return sqlCommand;
        }
    }
}
