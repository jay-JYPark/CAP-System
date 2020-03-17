using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Adeng.Framework.Db;
using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    public class DBConnector
    {
        private string targetHostIP = null;
        private string targetSID = null;
        private string targetUserID = null;
        private string targetUserPWD = null;

        private AdengOracleDbEx oracleDB = null;

        /// <summary>
        /// 생성자
        /// <param name="hostIP">Host IP</param>
        /// <param name="serviceID">Service ID</param>
        /// <param name="userID">사용자 ID</param>
        /// <param name="userPWD">사용자 비밀번호</param>
        /// </summary>
        public DBConnector(string hostIP, string serviceID, string userID, string userPWD)
        {
            this.targetHostIP = hostIP;
            this.targetSID = serviceID;
            this.targetUserID = userID;
            this.targetUserPWD = userPWD;

            //if (this.oracleDB == null)
            //{
            //    this.oracleDB = new AdengOracleDbEx();
            //}
        }
        public DBConnector(ConfigDBData settingInfo)
        {
            if (settingInfo != null)
            {
                this.targetHostIP = settingInfo.HostIP;
                this.targetSID = settingInfo.ServiceID;
                this.targetUserID = settingInfo.UserID;
                this.targetUserPWD = settingInfo.UserPassword;
            }
        }

        /// <summary>
        /// DB 접속 정보 설정
        /// </summary>
        /// <param name="hostIP">Host IP</param>
        /// <param name="serviceID">Service ID</param>
        /// <param name="userID">사용자 ID</param>
        /// <param name="userPWD">사용자 비밀번호</param>
        /// <returns></returns>
        public void SetConnectionInfo(string hostIP, string serviceID, string userID, string userPWD)
        {
            this.targetHostIP = hostIP;
            this.targetSID = serviceID;
            this.targetUserID = userID;
            this.targetUserPWD = userPWD;
        }

        /// <summary>
        /// DB 접속
        /// </summary>
        /// <returns>접속 수행 결과</returns>
        public bool OpenDB()
        {
            bool isOpen = false;

            try
            {
                // 내부적으로 생성/연결된 컨넥션이 내부 멤버 오브젝트를 Dispose로 삭제해도
                // 실시간으로 삭제되지 않아 컨넥션이 계속해서 남아있는 문제가 있음.
                // 그래서 상위에서 매번 new 해서 새로운 오브젝트를 생성해야할 필요가 있다.
                this.oracleDB = new AdengOracleDbEx();
                this.oracleDB.OpenOracle(this.targetHostIP, this.targetSID, this.targetUserID, this.targetUserPWD);

                isOpen = this.oracleDB.IsOpen;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBConnector] OpenDB( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBConnector] OpenDB ( Exception=[" + ex.ToString() + "] )");

                isOpen = false;

                if (this.oracleDB.IsOpen)
                {
                    this.oracleDB.Close();
                }
                this.oracleDB = null;
            }

            return isOpen;
        }

        /// <summary>
        /// DB 접속 종료
        /// </summary>
        public void CloseDB()
        {
            try
            {
                this.oracleDB.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBConnector] CloseDB( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBConnector] CloseDB ( Exception=[" + ex.ToString() + "] )");
            }
            finally
            {
                if (this.oracleDB.IsOpen)
                {
                    this.oracleDB.Close();
                }
                this.oracleDB = null;
            }
        }

        /// <summary>
        /// DB Open 상태
        /// </summary>
        /// <returns></returns>
        public bool IsOpend()
        {
            if (this.oracleDB == null)
            {
                return false;
            }
            return this.oracleDB.IsOpen;
        }

        /// <summary>
        /// DB 접속
        /// </summary>
        /// <returns>접속 수행 결과</returns>
        public bool TestOpenDB(ConfigDBData dbInfo)
        {
            bool result = false;
            try
            {
                // 내부적으로 생성/연결된 컨넥션이 내부 멤버 오브젝트를 Dispose로 삭제해도
                // 실시간으로 삭제되지 않아 컨넥션이 계속해서 남아있는 문제가 있음.
                // 그래서 상위에서 매번 new 해서 새로운 오브젝트를 생성해야할 필요가 있다.
                this.oracleDB = new AdengOracleDbEx();

                this.oracleDB.OpenOracle(dbInfo.HostIP, dbInfo.ServiceID, dbInfo.UserID, dbInfo.UserPassword);
                result = this.oracleDB.IsOpen;

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBConnector] TestOpenDB( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBConnector] TestOpenDB ( Exception=[" + ex.ToString() + "] )");

                return false;
            }
            finally
            {
                if (this.oracleDB.IsOpen)
                {
                    this.oracleDB.Close();
                }
                this.oracleDB = null;
            }

            return result;
        }

        /// <summary>
        /// DB 쿼리 실행.
        /// BeginTransaction에 의한 관리 외의 경우, 자동으로 COMMIT 합니다.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>쿼리 실행 결과 데이터</returns>
        public QueryResultDataInfo ExecuteQuery(string query)
        {
            QueryResultDataInfo resultData = null;
            OracleDataReader reader = null;

            try
            {
                this.oracleDB.QueryData(query, out reader);
                if (reader == null)
                {
                    return null;
                }
                resultData = new QueryResultDataInfo();
                resultData.AffectedRecordCnt = reader.RecordsAffected;

                // 데이터 추출
                while (reader.Read())
                {
                    RowData rowData = new RowData();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        rowData.FieldDataList.Add(reader[i].ToString());
                    }
                    resultData.DataList.Add(rowData);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBConnector] ExecuteQuery( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBConnector] ExecuteQuery( Exception=[" + ex.ToString() + "] )");

                return null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
            }

            return resultData;
        }
        /// <summary>
        /// DB 쿼리 실행.(CLOB 데이터 입력 용)
        /// BeginTransaction에 의한 관리 외의 경우, 자동으로 COMMIT 합니다.
        /// </summary>
        /// <param name="querySelectForUpdate"></param>
        /// <param name="clobData"></param>
        /// <returns>쿼리 실행 결과 데이터</returns>
        public QueryResultDataInfo ExecuteQueryForClob(string querySelectForUpdate, string clobData)
        {
            QueryResultDataInfo resultData = null;
            OracleDataReader reader = null;

            try
            {
                this.oracleDB.QueryData(querySelectForUpdate, out reader);
                if (reader == null)
                {
                    return null;
                }
                resultData = new QueryResultDataInfo();
                resultData.AffectedRecordCnt = reader.RecordsAffected;

                reader.Read();

                OracleClob clob = reader.GetOracleClob(1);
                clob.Erase();
                Encoding utf16Encoding = Encoding.GetEncoding("utf-16");
                byte[] clobDataByteArray = utf16Encoding.GetBytes(clobData);
                clob.Write(clobDataByteArray, 0, clobDataByteArray.Length);

                clob.Close();
                clob.Dispose();
                clob = null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBConnector] ExecuteQueryForClob( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBConnector] ExecuteQueryForClob( Exception=[" + ex.ToString() + "] )");

                return null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
            }

            return resultData;
        }

        /// <summary>
        /// 트랜잭션 시작
        /// </summary>
        /// <returns></returns>
        public bool BeginTransaction()
        {
            try
            {
                this.oracleDB.BeginTransaction();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBConnector] BeginTransaction( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBConnector] BeginTransaction( Exception=[" + ex.ToString() + "] )");

                return false;
            }

            return true;
        }

        /// <summary>
        /// 트랜잭션 종료
        /// </summary>
        /// <param name="endState">트랜잭션 종료 형태 지정. <br></br> 
        /// true: Commit 으로 종료 <br></br>
        /// false: RollBack 으로 종료</param>
        /// <returns></returns>
        public bool EndTransaction(bool endState)
        {
            try
            {
                if (endState)
                {
                    // oracleDB.BeginTransaction() 를 호출하지 않는 경우에는, 자동으로 커밋된다.
                    // BeginTransaction을 호출하지 않는 상태에서 커밋을 실행하면 널익셉션 발생
                    oracleDB.Commit();
                }
                else
                {
                    this.oracleDB.Rollback();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBConnector] EndTransaction( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBConnector] EndTransaction( Exception=[" + ex.ToString() + "] )");

                //this.oracleDB.Rollback();
                return false;
            }

            return true;
        }

    }
}
