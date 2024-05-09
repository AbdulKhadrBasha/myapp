using System;
using System.Data.SqlClient;
using System.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace myapp
{
    public class LogInfo
    {
        public int iD { get; set; }
        public string name { get; set; }
        public string details { get; set; }
        public string location { get; set; }
        public int accessCount { get; set; }
        public DateTime accessDateTime { get; set; }


        private readonly IConfiguration _configuration;

        public LogInfo(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<LogInfo> LogData()
        {
            var dataList = new List<LogInfo>();
            try
            {
                string con = _configuration.GetConnectionString("DbConnection").ToString();

                LogInfo matchingLogInfo;
                
                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    string oString = "Select * from Loging";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            matchingLogInfo = new LogInfo(_configuration);
                            matchingLogInfo.iD = Convert.ToInt32(oReader["ID"]);
                            matchingLogInfo.name = oReader["Name"].ToString();
                            matchingLogInfo.details = oReader["Details"].ToString();
                            matchingLogInfo.location = oReader["Location"].ToString();
                            matchingLogInfo.accessCount = Convert.ToInt32(oReader["AccessCount"]);
                            matchingLogInfo.accessDateTime = Convert.ToDateTime(oReader["AccessDateTime"]);
                            dataList.Add(matchingLogInfo);
                        }

                        myConnection.Close();
                    }
                }


            }
            catch (Exception ex)
            {

                string msg = ex.Message;
            }
            return dataList;

        }
    }
}
