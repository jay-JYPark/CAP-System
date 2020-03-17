using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Data.OleDb;

using Adeng.Framework.Db;
using CAPLib;
using Oracle.DataAccess.Client;
using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    public class DBManager
    {
        private readonly string NATIONAL_CODE = "0000000000";

        private static volatile DBManager instance = null;
        private static Mutex mutex = new Mutex();
        ConfigDBData currentDBSetting = new ConfigDBData();

        /// <summary>
        /// 싱글톤 처리
        /// 반드시 이 메소드를 호출하여 인스턴스를 사용해야 한다.
        /// </summary>
        /// <returns></returns>
        public static DBManager GetInstance()
        {
            mutex.WaitOne();
            if (instance == null)
            {
                instance = new DBManager();
            }

            mutex.ReleaseMutex();
            return instance;
        }

        /// <summary>
        /// 생성자
        /// 싱글톤 생성 메소드를 사용하여 인스턴스 생성할 것.
        /// </summary>
        private DBManager()
        {
        }

        /// <summary>
        /// DB 접속 정보 설정.
        /// DB로부터 정보를 조회하기 전에, 반드시 이 함수를 사용하여 접속 정보를 설정해야 한다.
        /// </summary>
        /// <param name="hostIP">Host IP</param>
        /// <param name="serviceID">Service ID</param>
        /// <param name="userID">사용자 ID</param>
        /// <param name="userPWD">사용자 비밀번호</param>
        /// <returns></returns>
        public void SetConnectionInfo(string hostIP, string serviceID, string userID, string userPWD)
        {
            Console.WriteLine("[DBManager] SetConnectionInfo4( start )");

            this.currentDBSetting.HostIP = hostIP;
            this.currentDBSetting.ServiceID = serviceID;
            this.currentDBSetting.UserID = userID;
            this.currentDBSetting.UserPassword = userPWD;

            Console.WriteLine("[DBManager] SetConnectionInfo( end )");
        }
        /// <summary>
        /// DB 접속 정보 설정.
        /// DB로부터 정보를 조회하기 전에, 반드시 이 함수를 사용하여 접속 정보를 설정해야 한다.
        /// </summary>
        /// <param name="settings">DB 접속 정보 데이터</param>
        /// <returns></returns>
        public void SetConnectionInfo(ConfigDBData settings)
        {
            Console.WriteLine("[DBManager] SetConnectionInfo1( start )");

            this.currentDBSetting.DeepCopyFrom(settings);

            Console.WriteLine("[DBManager] SetConnectionInfo1( end )");
        }

        /// <summary>
        /// DB 접속 가능 확인
        /// </summary>
        /// <returns></returns>
        public bool CheckOpenDB()
        {
            System.Console.WriteLine("[DBManager] CheckOpenDB( start )");

            bool result = false;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] CheckOpenDB( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] CheckOpenDB( DBConnector 생성 에러 )");

                    return false;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] CheckOpenDB( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] CheckOpenDB( DB Open ERROR )");

                    return false;
                }

                result = true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] CheckOpenDB( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] CheckOpenDB( " + ex.ToString() + " )");

                return false;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            Console.WriteLine("[DBManager] CheckOpenDB( end )");
            return result;
        }

        /// <summary>
        /// DB 접속 가능 확인
        /// </summary>
        /// <returns></returns>
        public bool TestOpenDB(ConfigDBData dbInfo)
        {
            Console.WriteLine("[DBManager] TestOpenDB( start )");

            bool result = false;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(dbInfo);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] TestOpenDB( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] TestOpenDB( DBConnector 생성 에러 )");

                    return false;
                }

                if (!dbConnector.OpenDB())
                {
                    Console.WriteLine("[DBManager] TestOpenDB(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] TestOpenDB( DB Open ERROR )");

                    return false;
                }

                result = true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] TestOpenDB( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] TestOpenDB( " + ex.ToString() + " )");

                return false;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            Console.WriteLine("[DBManager] CheckOpenDB( end )");
            return result;
        }


        #region 사용자
        /// <summary>
        /// 사용자 정보 취득
        /// </summary>
        /// <returns></returns>
        public List<UserAccount> QueryUserAccountInfo()
        {
            System.Console.WriteLine("[DBManager] QueryUserAccountInfo( start )");

            List<UserAccount> list = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryUserAccountInfo( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryUserAccountInfo( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    Console.WriteLine("[DBManager] QueryUserAccountInfo(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryUserAccountInfo( DB Open ERROR )");

                    return null;
                }

                string query = "SELECT ID, PASSWORD, NAME, DEPARTMENT, PHONE, DESCRIPTION FROM USERPROFILE ORDER BY ID";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryUserAccountInfo( SQL Error : " + query + " )");
                    return null;
                }

                list = new List<UserAccount>();

                if (resultData.DataCnt <= 0)
                {
                    System.Console.WriteLine("[DBManager] QueryUserAccountInfo( 레코드가 0건 )");
                    return list;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    UserAccount user = new UserAccount();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    user.ID = value;
                                }
                                break;
                            case 1:
                                {
                                    user.Password = value;
                                }
                                break;
                            case 2:
                                {
                                    user.Name = value;
                                }
                                break;
                            case 3:
                                {
                                    user.Departure = value;
                                }
                                break;
                            case 4:
                                {
                                    user.Telephone = value;
                                }
                                break;
                            case 5:
                                {
                                    user.Description = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    list.Add(user);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryUserAccountInfo( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryUserAccountInfo( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryUserAccountInfo( end )");
            return list;
        }
        /// <summary>
        /// 사용자 정보 등록
        /// </summary>
        /// <returns></returns>
        public int RegistUserAccountInfo(UserAccount accountInfo)
        {
            System.Console.WriteLine("[DBManager] RegistUserAccountInfo( start )");
            System.Diagnostics.Debug.Assert(accountInfo != null);

            if (accountInfo == null)
            {
                return -1;
            }

            int result = 0;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] RegistUserAccountInfo( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] RegistUserAccountInfo( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    Console.WriteLine("[DBManager] RegistUserAccountInfo(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] RegistUserAccountInfo( DB Open ERROR )");

                    return -1;
                }

                string query = "SELECT NAME FROM USERPROFILE WHERE ID = '" + accountInfo.ID + "'";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] RegistUserAccountInfo( SQL Error : " + query + " )");
                    return -2;
                }
                if (resultData.DataCnt > 0)
                {
                    System.Console.WriteLine("[DBManager] RegistUserAccountInfo( 이미 등록된 아이디 : " + query + " )");
                    return -3;
                }

                string department = "null";
                if (!string.IsNullOrEmpty(accountInfo.Departure))
                {
                    department = "'" + accountInfo.Departure + "'";
                }
                string phone = "null";
                if (!string.IsNullOrEmpty(accountInfo.Telephone))
                {
                    phone = "'" + accountInfo.Telephone + "'";
                }
                string description = "null";
                if (!string.IsNullOrEmpty(accountInfo.Description))
                {
                    description = "'" + accountInfo.Description + "'";
                }

                query = "INSERT INTO USERPROFILE (ID, PASSWORD, NAME, DEPARTMENT, PHONE, DESCRIPTION) "
                            + " VALUES ('" + accountInfo.ID + "', '" + accountInfo.Password + "', '" + accountInfo.Name + "'"
                            + ", " + department + ", " + phone + ", " + description + ")";
                resultData = dbConnector.ExecuteQuery(query);
                if (null == resultData)
                {
                    System.Console.WriteLine("[DBManager] RegistUserAccountInfo( SQL Error2 = [" + query + "])");
                    return -2;
                }
                if (resultData.AffectedRecordCnt != 1)
                {
                    System.Console.WriteLine("[DBManager] RegistUserAccountInfo( 갱신 결과 오류 : resultData.AffectedRecordCnt=[" + resultData.AffectedRecordCnt + "] )");
                    return 1;
                }

                result = 0;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] RegistUserAccountInfo( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] RegistUserAccountInfo( " + ex.ToString() + " )");

                result = -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] RegistUserAccountInfo( end )");
            return result;
        }
        /// <summary>
        /// 사용자 정보 갱신
        /// </summary>
        /// <returns></returns>
        public int UpdateUserAccountInfo(UserAccount accountInfo)
        {
            System.Console.WriteLine("[DBManager] UpdateUserAccountInfo( start )");
            System.Diagnostics.Debug.Assert(accountInfo != null);

            if (accountInfo == null)
            {
                return -1;
            }

            int result = 0;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] UpdateUserAccountInfo( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateUserAccountInfo( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    Console.WriteLine("[DBManager] UpdateUserAccountInfo(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateUserAccountInfo( DB Open ERROR )");

                    return -1;
                }

                string department = "null";
                if (!string.IsNullOrEmpty(accountInfo.Departure))
                {
                    department = "'" + accountInfo.Departure + "'";
                }
                string phone = "null";
                if (!string.IsNullOrEmpty(accountInfo.Telephone))
                {
                    phone = "'" + accountInfo.Telephone + "'";
                }
                string description = "null";
                if (!string.IsNullOrEmpty(accountInfo.Description))
                {
                    description = "'" + accountInfo.Description + "'";
                }
                string query = "UPDATE USERPROFILE SET PASSWORD = '" + accountInfo.Password + "', NAME = '" + accountInfo.Name + "' "
                                + ", DEPARTMENT = " + department + ", PHONE = " + phone + ", DESCRIPTION = " + description
                                + " WHERE ID = '" + accountInfo.ID + "'";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (null == resultData)
                {
                    System.Console.WriteLine("[DBManager] UpdateUserAccountInfo( SQL Error2 = [" + query + "])");
                    return -2;
                }
                if (resultData.AffectedRecordCnt != 1)
                {
                    System.Console.WriteLine("[DBManager] UpdateUserAccountInfo( 갱신 결과 오류 : resultData.AffectedRecordCnt=[" + resultData.AffectedRecordCnt + "] )");
                    return 1;
                }

                result = 0;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] UpdateUserAccountInfo( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] UpdateUserAccountInfo( " + ex.ToString() + " )");

                result = -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] UpdateUserAccountInfo( end )");
            return result;
        }
        /// <summary>
        /// 사용자 정보 삭제
        /// </summary>
        /// <returns></returns>
        public int DeleteUserAccountInfo(string accountID)
        {
            System.Console.WriteLine("[DBManager] DeleteUserAccountInfo( start )");
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(accountID));

            int result = 0;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] DeleteUserAccountInfo( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] DeleteUserAccountInfo( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] DeleteUserAccountInfo( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] DeleteUserAccountInfo( DB Open ERROR )");

                    return -1;
                }

                string query = "DELETE FROM USERPROFILE WHERE ID = '" + accountID + "'";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] DeleteUserAccountInfo( SQL Error = [" + query + " )");
                    return -2;
                }
                if (resultData.AffectedRecordCnt != 1)
                {
                    System.Console.WriteLine("[DBManager] DeleteUserAccountInfo( AffectedRecordCnt=[" + resultData.AffectedRecordCnt + "] )");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] DeleteUserAccountInfo( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] DeleteUserAccountInfo( " + ex.ToString() + " )");

                return -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] DeleteUserAccountInfo( end )");
            return result;
        }
        #endregion


        #region 재난종류
        /// <summary>
        /// 재난 카테고리에 대한 기초 정보 취득
        /// </summary>
        /// <returns></returns>
        public List<DisasterInfo> QueryDefinitionOfDisaster()
        {
            Console.WriteLine("[DBManager] QueryDefinitionOfDisaster( start )");

            List<DisasterInfo> disasterList = null;
            try
            {
                disasterList = new List<DisasterInfo>();
                int result = QueryDisasterCategory(ref disasterList);
                if (result >= 0)
                {
                    result = QueryDisasterKind(ref disasterList);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryDefinitionOfDisaster( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfDisaster( " + ex.ToString() + " )");
            }

            Console.WriteLine("[DBManager] QueryDefinitionOfDisaster( end )");

            return disasterList;
        }
        /// <summary>
        /// 재난 카테고리에 대한 기초 정보 취득
        /// </summary>
        /// <returns></returns>
        private int QueryDisasterCategory(ref List<DisasterInfo> disasterList)
        {
            System.Console.WriteLine("[DBManager] QueryDisasterCategory( start )");

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDisasterCategory( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDisasterCategory( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryDisasterCategory( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDisasterCategory( DB Open ERROR )");

                    return -1;
                }

                // 프로그램에서 사용하는 행정구역 코드 버전 아이디
                string query = "SELECT ID, CODE, NAME FROM DISASTER_CATEGORY ORDER BY NAME ASC";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDisasterCategory( SQL Error )");
                    return -2;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    DisasterInfo record = new DisasterInfo();
                    record.Category = new DisasterCategory();
                    record.KindList = new List<DisasterKind>();

                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    record.Category.ID = 0;
                                    int temp = 0;
                                    if (int.TryParse(value, out temp))
                                    {
                                        record.Category.ID = temp;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    record.Category.Code = value;
                                }
                                break;
                            case 2:
                                {
                                    record.Category.Name = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    disasterList.Add(record);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryDisasterCategory( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryDisasterCategory( " + ex.ToString() + " )");

                return -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryDisasterCategory( end )");
            return 0;
        }
        /// <summary>
        /// 재난 카테고리에 대한 기초 정보 취득
        /// </summary>
        /// <returns></returns>
        private int QueryDisasterKind(ref List<DisasterInfo> disasterList)
        {
            System.Console.WriteLine("[DBManager] QueryDisasterKind( start )");

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDisasterKind( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDisasterKind( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryDisasterKind( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDisasterKind( DB Open ERROR )");

                    return -1;
                }

                for (int index = 0; index < disasterList.Count; index++)
                {
                    DisasterCategory category = disasterList[index].Category;
                    // 카테고리 별 코드
                    string query = "SELECT CODE, NAME FROM DISASTER_KIND WHERE CATEGORY_ID = " + category.ID + "  ORDER BY NAME ASC";
                    QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                    if (resultData == null)
                    {
                        System.Console.WriteLine("[DBManager] QueryDisasterKind( SQL Error : " + query + " )");
                        return -2;
                    }

                    foreach (RowData rowData in resultData.DataList)
                    {
                        DisasterKind record = new DisasterKind();
                        record.CategoryID = category.ID;

                        for (int field = 0; field < rowData.FieldCnt; field++)
                        {
                            string value = rowData.FieldDataList[field];
                            switch (field)
                            {
                                case 0:
                                    {
                                        record.Code = value;
                                    }
                                    break;
                                case 1:
                                    {
                                        record.Name = value;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        disasterList[index].KindList.Add(record);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryDisasterKind( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryDisasterKind( " + ex.ToString() + " )");

                return -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryDisasterKind( end )");
            return 0;
        }
        #endregion

        #region 표준경보시스템(기초정보)
        /// <summary>
        /// 표준경보시스템 종류에 대한 기초 정보 취득.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, SASKind> QueryDefinitionOfSASKind()
        {
            System.Console.WriteLine("[DBManager] QueryDefinitionOfSASKind( start )");

            Dictionary<string, SASKind> list = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSASKind( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSASKind( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSASKind(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSASKind( DB Open ERROR )");

                    return null;
                }

                string query = "SELECT CODE, NAME FROM SASKIND ORDER BY NAME ASC";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    Console.WriteLine("[DBManager] QueryDefinitionOfSASKind( DB Open ERROR )");
                    return null;
                }

                list = new Dictionary<string, SASKind>();

                if (resultData.DataCnt < 1)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSASKind( 레코드가 0건 )");

                    return list;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    SASKind record = new SASKind();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    record.Code = value;
                                }
                                break;
                            case 1:
                                {
                                    record.Name = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    list.Add(record.Code, record);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryDefinitionOfSASKind( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSASKind( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryDefinitionOfSASKind( end )");
            return list;
        }
        #endregion

        #region 표준경보시스템
        /// <summary>
        /// 표준경보시스템에 대한 전체 정보 조회.
        /// 입력 파라미터 systemIDs 가 널인 경우에는, 모든 표준경보시스템에 대한 정보를 조회한다.
        /// </summary>
        /// <returns></returns>
        public List<SASProfile> QuerySASInfo(List<string> systemIDs = null)
        {
            System.Console.WriteLine("[DBManager] QuerySASInfo( start )");

            List<SASProfile> list = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QuerySASInfo( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QuerySASInfo( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QuerySASInfo(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QuerySASInfo( DB Open ERROR )");

                    return null;
                }

                string query = "SELECT ID, NAME, AUTHCODE, SYSTEMKIND, IPTYPE, IPADDRESS, LATITUDE, LONGITUDE," +
                                " ASSIGNEDIAGWREGIONCODE, ADDRESS, MANAGERNAME, MANAGERDEPARTMENT, MANAGERPHONE" +
                                " FROM SASPROFILE " +
                                " ORDER BY NAME ASC";

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QuerySASInfo( SQL Error : " + query + " )");
                    return null;
                }

                // 반환할 리스트
                list = new List<SASProfile>();

                if (resultData.DataCnt <= 0)
                {
                    System.Console.WriteLine("[DBManager] QuerySASInfo( 레코드가 0건 )");

                    return list;
                }

                List<SASProfile> tempList = new List<SASProfile>();

                foreach (RowData rowData in resultData.DataList)
                {
                    SASProfile record = new SASProfile();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    record.ID = value;
                                }
                                break;
                            case 1:
                                {
                                    record.Name = value;
                                }
                                break;
                            case 2:
                                {
                                    record.AuthCode = value;
                                }
                                break;
                            case 3:
                                {
                                    record.KindCode = value;
                                }
                                break;
                            case 4:
                                {
                                    record.IpType = IPType.IPv4;
                                    IPType temp = IPType.IPv4;
                                    if (Enum.TryParse<IPType>(value, out temp))
                                    {
                                        record.IpType = temp;
                                    }
                                }
                                break;
                            case 5:
                                {
                                    record.IpAddress = value;
                                }
                                break;
                            case 6:
                                {
                                    record.Latitude = value;
                                }
                                break;
                            case 7:
                                {
                                    record.Longitude = value;
                                }
                                break;
                            case 8:
                                {
                                    record.AssignedIAGWRegionCode = value;
                                }
                                break;
                            case 9:
                                {
                                    record.Address = value;
                                }
                                break;
                            case 10:
                                {
                                    record.ManagerName = value;
                                }
                                break;
                            case 11:
                                {
                                    record.ManagerDepartment = value;
                                }
                                break;
                            case 12:
                                {
                                    record.ManagerPhone = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    tempList.Add(record);
                }

                if (systemIDs == null)
                {
                    list = tempList;
                }
                else
                {
                    foreach (string identifier in systemIDs)
                    {
                        for (int index = 0; index < tempList.Count; index++)
                        {
                            if (identifier == tempList[index].ID)
                            {
                                list.Add(tempList[index]);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QuerySASInfo( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QuerySASInfo( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QuerySASInfo( end )");
            return list;
        }
        /// <summary>
        /// 표준경보시스템 정보 갱신.
        /// </summary>
        /// <returns></returns>
        public int UpdateSASProfile(List<SASProfile> systemProfiles)
        {
            System.Console.WriteLine("[DBManager] UpdateSASProfile( start )");

            int result = 0;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] UpdateSASProfile( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateSASProfile( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] UpdateSASProfile( DB Open Error )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateSASProfile( DB Open ERROR )");

                    return -1;
                }

                // 트랜잭션 시작
                if (!dbConnector.BeginTransaction())
                {
                    System.Console.WriteLine("[DBManager] UpdateSASProfile( 트랜잭션 시작 실패 )");
                    return -2;
                }

                foreach (SASProfile profile in systemProfiles)
                {
                    string ipType = ((int)profile.IpType).ToString();

                    string query = "UPDATE SASPROFILE SET NAME = '" + profile.Name + "', AUTHCODE = '" + profile.AuthCode + "', SYSTEMKIND = '" + profile.KindCode + "', " +
                                    "IPTYPE = " + ipType + ", IPADDRESS = '" + profile.IpAddress + "', LATITUDE = '" + profile.Latitude + "', LONGITUDE = '" + profile.Longitude + "', " +
                                    "ASSIGNEDIAGWREGIONCODE = '" + profile.AssignedIAGWRegionCode + "', ADDRESS = '" + profile.Address + "', " +
                                    "MANAGERNAME = '" + profile.ManagerName + "', MANAGERDEPARTMENT = '" + profile.ManagerDepartment + "', MANAGERPHONE = '" + profile.ManagerPhone + "' " +
                                    "WHERE ID = '" + profile.ID + "'";

                    QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                    if (resultData == null)
                    {
                        // error
                        dbConnector.EndTransaction(false);
                        return -3;
                    }

                    if (resultData.AffectedRecordCnt <= 0)
                    {
                        // 신규 레코드: 등록
                        query = "INSERT INTO SASPROFILE (ID, NAME, AUTHCODE, SYSTEMKIND, IPTYPE, IPADDRESS, LATITUDE, LONGITUDE, " +
                                "ASSIGNEDIAGWREGIONCODE, ADDRESS, MANAGERNAME, MANAGERDEPARTMENT, MANAGERPHONE) " +
                                "VALUES ('" + profile.ID + "', '" + profile.Name + "', '" + profile.AuthCode + "', '" + profile.KindCode + "'" +
                                ", " + ipType + ", '" + profile.IpAddress + "', '" + profile.Latitude + "', '" + profile.Longitude + "'" +
                                ", '" + profile.AssignedIAGWRegionCode + "', '" + profile.Address + "'" +
                                ", '" + profile.ManagerName + "', '" + profile.ManagerDepartment + "', '" + profile.ManagerPhone + "')";

                        resultData = dbConnector.ExecuteQuery(query);
                        if (resultData == null)
                        {
                            dbConnector.EndTransaction(false);
                            return -4;
                        }
                        if (resultData.AffectedRecordCnt <= 0)
                        {
                            dbConnector.EndTransaction(false);
                            return -5;
                        }
                    }
                }

                dbConnector.EndTransaction(true);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] UpdateSASProfile( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] UpdateSASProfile( " + ex.ToString() + " )");

                return -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] UpdateSASProfile( end )");
            return result;
        }
        /// <summary>
        /// 표준경보시스템 프로필 삭제.
        /// </summary>
        /// <param name="systemProfiles"></param>
        /// <returns></returns>
        public int DeleteSASProfile(List<SASProfile> systemProfiles)
        {
            System.Console.WriteLine("[DBManager] DeleteSASProfile( start )");

            int result = 0;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] DeleteSASProfile( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] DeleteSASProfile( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] DeleteSASProfile(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] DeleteSASProfile( DB Open ERROR )");

                    return -1;
                }

                // 트랜잭션 시작
                if (!dbConnector.BeginTransaction())
                {
                    System.Console.WriteLine("[DBManager] DeleteSASProfile( 트랜잭션 시작 실패 )");
                    return -2;
                }

                foreach (SASProfile profile in systemProfiles)
                {
                    string query = "DELETE FROM SASPROFILE WHERE ID = '" + profile.ID + "'";
                    QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                    if (resultData == null)
                    {
                        // 트랜잭션 롤백
                        dbConnector.EndTransaction(false);
                        System.Console.WriteLine("[DBManager] DeleteSASProfile( 삭제 실패, 트랜잭션 종료 )");
                        return -3;
                    }
                }

                // 트랜잭션 종료
                if (!dbConnector.EndTransaction(true))
                {
                    System.Console.WriteLine("[DBManager] DeleteSASProfile( 트랜잭션 종료 실패 )");
                    return -4;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] DeleteSASProfile( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] DeleteSASProfile( " + ex.ToString() + " )");

                return -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] DeleteSASProfile( end )");
            return result;
        }
        #endregion

        #region 발령문안
        /// <summary>
        /// <para>
        /// 문안 표출 매체 형태에 대한 기초 정보 취득;
        /// </para>
        /// </summary>
        /// <returns></returns>
        public List<MsgTextDisplayMediaType> QueryDefinitionOfMsgTextDisplayMediaType()
        {
            System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextDisplayMediaType( start )");

            List<MsgTextDisplayMediaType> list = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextDisplayMediaType( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfMsgTextDisplayMediaType( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextDisplayMediaType(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfMsgTextDisplayMediaType( DB Open ERROR )");

                    return null;
                }

                string query = "SELECT ID, CODE, NAME, LETTERCOUNTLIMIT" +
                                " FROM MSGTEXT_DISPLAYMEDIATYPE ORDER BY NAME ASC";

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextDisplayMediaType( SQL Error : " + query + " )");
                    return null;
                }

                // 반환할 리스트
                list = new List<MsgTextDisplayMediaType>();

                if (resultData.DataCnt <= 0)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextDisplayMediaType( 레코드가 0건 )");
                    return list;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    MsgTextDisplayMediaType record = new MsgTextDisplayMediaType();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    uint temp = 0;
                                    if (uint.TryParse(value, out temp))
                                    {
                                        record.ID = temp;
                                    }
                                    else
                                    {
                                        record.ID = 0;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    record.Code = value;
                                }
                                break;
                            case 2:
                                {
                                    record.TypeName = value;
                                }
                                break;
                            case 3:
                                {
                                    uint temp = 0;
                                    if (uint.TryParse(value, out temp))
                                    {
                                        record.LetterCountLimit = temp;
                                    }
                                    else
                                    {
                                        record.LetterCountLimit = 0;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    list.Add(record);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextDisplayMediaType( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfMsgTextDisplayMediaType( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextDisplayMediaType( end )");
            return list;
        }
        /// <summary>
        /// 문안 언어 종류에 대한 기초 정보 취득
        /// </summary>
        /// <returns></returns>
        public List<MsgTextDisplayLanguageKind> QueryDefinitionOfMsgTextDisplayLanguageKind()
        {
            System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextDisplayLanguageKind( start )");

            List<MsgTextDisplayLanguageKind> list = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextDisplayLanguageKind( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfMsgTextDisplayLanguageKind( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextDisplayLanguageKind(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfMsgTextDisplayLanguageKind( DB Open ERROR )");

                    return null;
                }

                string query = "SELECT ID, CODE, NAME, IS_DEFAULT" +
                                " FROM MSGTEXT_LANGUAGE" +
                                " WHERE AVAILABLE = 1";

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextDisplayLanguageKind( SQL Error : " + query + " )");
                    return null;
                }

                // 반환할 리스트
                list = new List<MsgTextDisplayLanguageKind>();

                if (resultData.DataCnt <= 0)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextDisplayLanguageKind( 레코드가 0건 )");
                    return list;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    MsgTextDisplayLanguageKind record = new MsgTextDisplayLanguageKind();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    int temp = 0;
                                    if (int.TryParse(value, out temp))
                                    {
                                        record.ID = temp;
                                    }
                                    else
                                    {
                                        record.ID = 0;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    record.LanguageCode = value;
                                }
                                break;
                            case 2:
                                {
                                    record.LanguageName = value;
                                }
                                break;
                            case 3:
                                {
                                    int temp = 0;
                                    if (int.TryParse(value, out temp))
                                    {
                                        if (temp != 0)
                                        {
                                            record.IsDefault = true;
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    list.Add(record);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextDisplayLanguageKind( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfMsgTextDisplayLanguageKind( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextDisplayLanguageKind( end )");
            return list;
        }
        public int UpdateLanguageSetting(Dictionary<int, MsgTextDisplayLanguageKind> settingList)
        {
            System.Console.WriteLine("[DBManager] UpdateLanguageSetting( start )");

            int result = 0;
            bool transactionStarted = false;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] UpdateLanguageSetting( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateLanguageSetting( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] UpdateLanguageSetting( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateLanguageSetting( DB Open ERROR )");

                    return -1;
                }

                // 트랜잭션 시작
                transactionStarted = dbConnector.BeginTransaction();
                if (!transactionStarted)
                {
                    result = -10;
                    return result;
                }

                foreach (MsgTextDisplayLanguageKind language in settingList.Values)
                {
                    int isDefault = (language.IsDefault) ? 1 : 0;
                    string query = "UPDATE MSGTEXT_LANGUAGE SET IS_DEFAULT = " + isDefault + " WHERE ID = " + language.ID;
                    QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                    if (null == resultData)
                    {
                        System.Console.WriteLine("[DBManager] UpdateLanguageSetting( SQL Error = [" + query + "])");
                        return -2;
                    }
                    if (resultData.AffectedRecordCnt != 1)
                    {
                        System.Console.WriteLine("[DBManager] UpdateLanguageSetting( 갱신 결과 오류 : resultData.AffectedRecordCnt=[" + resultData.AffectedRecordCnt + "] )");
                        return 1;
                    }
                }

                result = 0;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] UpdateLanguageSetting( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] UpdateLanguageSetting( " + ex.ToString() + " )");

                return -99;
            }
            finally
            {
                if (transactionStarted)
                {
                    if (result == 0)
                    {
                        dbConnector.EndTransaction(true);
                    }
                    else
                    {
                        dbConnector.EndTransaction(false);
                    }
                }

                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] UpdateLanguageSetting( end )");
            return result;
        }

        /// <summary>
        /// 문안 도시 유형에 대한 기초 정보 취득
        /// </summary>
        /// <returns></returns>
        public List<MsgTextCityType> QueryDefinitionOfMsgTextCityType()
        {
            System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextCityType( start )");

            List<MsgTextCityType> list = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextCityType( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfMsgTextCityType( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextCityType(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfMsgTextCityType( DB Open ERROR )");

                    return null;
                }

                string query = "SELECT ID, CODE, NAME" +
                                " FROM MSGTEXT_SUPPORTEDCITYTYPE";

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextCityType( SQL Error : " + query + " )");
                    return null;
                }

                // 반환할 리스트
                list = new List<MsgTextCityType>();

                if (resultData.DataCnt <= 0)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextCityType( 레코드가 0건 )");
                    return list;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    MsgTextCityType record = new MsgTextCityType();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    int temp = 0;
                                    if (int.TryParse(value, out temp))
                                    {
                                        record.ID = temp;
                                    }
                                    else
                                    {
                                        record.ID = 0;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    record.TypeCode = value;
                                }
                                break;
                            case 2:
                                {
                                    record.TypeName = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    list.Add(record);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextCityType( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfMsgTextCityType( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryDefinitionOfMsgTextCityType( end )");
            return list;
        }
        /// <summary>
        /// 문안 도시 유형에 대한 기초 정보 취득
        /// </summary>
        /// <returns></returns>
        public List<MsgText> QueryDefinitionOfBasicMsgText()
        {
            System.Console.WriteLine("[DBManager] QueryDefinitionOfBasicMsgText( start )");

            List<MsgText> list = null;

            System.Console.WriteLine("[DBManager] QueryDefinitionOfBasicMsgText( end )");
            return list;
        }
        #endregion

        #region 행정구역
        /// <summary>
        /// 행정 구역 정보 취득(전체)
        /// </summary>
        /// <returns></returns>
        public RegionInfo QueryRegionProfileInfo(string currentRegion)
        {
            System.Console.WriteLine("[DBManager] QueryRegionProfileInfo( start )");

            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(currentRegion));

            RegionInfo regionInfo = new RegionInfo();
            // 프로그램에서 사용하는 행정구역 코드 버전을 취득
            regionInfo.Version = QueryRegionCodeVersion();
            if (regionInfo.Version == null)
            {
                System.Console.WriteLine("[DBManager] QueryRegionProfileInfo( 행정구역 코드 버전 취득 실패 )");
                return null;
            }

            RegionCode regionCode = new RegionCode(currentRegion);
            // 현재 사용하는 코드 버전에 맞는 행정구역 코드 취득
            QueryRegionCodes(regionCode, ref regionInfo);

            // 각 행정구역 코드에 해당하는 경계 데이터 취득
            QueryRegionOuterBoundary(ref regionInfo);

            System.Console.WriteLine("[DBManager] QueryRegionProfileInfo( end )");
            return regionInfo;
        }
        /// <summary>
        /// 행정 구역 정보 취득(행정구역 코드 버전 정보 - 프로그램)
        /// </summary>
        /// <returns></returns>
        private RegionCodeVersion QueryRegionCodeVersion()
        {
            System.Console.WriteLine("[DBManager] QueryRegionCodeVersion( start )");

            // 각 행정 구역에 해당하는 구역 경계 정보 취득
            RegionCodeVersion versionInfo = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryRegionCodeVersion( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryRegionCodeVersion( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryRegionCodeVersion(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryRegionCodeVersion( DB Open ERROR )");

                    return null;
                }

                // 프로그램에서 사용하는 행정구역 코드 버전 아이디
                string query = "SELECT USINGVERSIONID FROM PROGRAM_REGIONCODEVERSION";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null || resultData.DataCnt != 1)
                {
                    System.Console.WriteLine("[DBManager] QueryRegionCodeVersion( 행정동 코드 버전 정보 취득 실패(1) )");
                    return null;
                }
                versionInfo = new RegionCodeVersion();
                versionInfo.ID = resultData.DataList[0].FieldDataList[0];
                resultData = null;

                // 프로그램에서 사용하는 행정구역 코드 버전 아이디
                query = "SELECT VERSION FROM REGION_CODEVERSION WHERE ID = " + versionInfo.ID;
                resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null || resultData.DataCnt != 1)
                {
                    System.Console.WriteLine("[DBManager] QueryRegionCodeVersion( 행정동 코드 버전 정보 취득 실패(2) )");
                    return null;
                }
                versionInfo.Information = resultData.DataList[0].FieldDataList[0];
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryRegionCodeVersion( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryRegionCodeVersion( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryRegionCodeVersion( end )");
            return versionInfo;
        }
        /// <summary>
        /// 행정 구역 정보 취득(행정구역 코드 부분)
        /// </summary>
        /// <returns></returns>
        private void QueryRegionCodes(RegionCode regionCode, ref RegionInfo regionInfo)
        {
            System.Console.WriteLine("[DBManager] QueryRegionCodes( start )");

            try
            {
                QueryLocalHighLevelRegionCode(regionCode, ref regionInfo);
                QueryLocalMiddleLevelRegionCode(ref regionInfo);
                QueryLocalLowLevelRegionCode(ref regionInfo);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryRegionCodes( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryRegionCodes( " + ex.ToString() + " )");
            }

            System.Console.WriteLine("[DBManager] QueryRegionCodes( end )");
        }
        /// <summary>
        /// 행정 구역 정보 취득(행정구역 코드 부분)
        /// </summary>
        /// <returns></returns>
        private int QueryLocalHighLevelRegionCode(RegionCode regionCode, ref RegionInfo regionInfo)
        {
            System.Console.WriteLine("[DBManager] QueryLocalHighLevelRegionCode( start )");

            System.Diagnostics.Debug.Assert(regionCode != null);
            System.Diagnostics.Debug.Assert(regionCode.Level1 != null);
            System.Diagnostics.Debug.Assert(regionCode.Level2 != null);
            System.Diagnostics.Debug.Assert(regionCode.Level3 != null);
            System.Diagnostics.Debug.Assert(regionInfo != null);

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryLocalHighLevelRegionCode( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryLocalHighLevelRegionCode( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryLocalHighLevelRegionCode(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryLocalHighLevelRegionCode( DB Open ERROR )");

                    return -1;
                }

                string query = "SELECT CODEPART1, CODEPART2, CODEPART3, NAME FROM REGIONPROFILE WHERE VERSIONID = " + regionInfo.Version.ID
                               + " AND CODEPART1 = '" + regionCode.Level1 + "' AND CODEPART2 = '" + regionCode.Level2 + "' AND CODEPART3 = '" + regionCode.Level3 + "'"
                               + " ORDER BY NAME ASC";

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryLocalHighLevelRegionCode( SQL Error : " + query + " )");
                    return -2;
                }
                if (resultData.DataCnt <= 0)
                {
                    System.Console.WriteLine("[DBManager] QueryLocalHighLevelRegionCode( 레코드가 0건 )");

                    return 1;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    RegionProfile record = new RegionProfile();
                    RegionCode code = new RegionCode();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    code.Level1 = value;
                                }
                                break;
                            case 1:
                                {
                                    code.Level2 = value;
                                }
                                break;
                            case 2:
                                {
                                    code.Level3 = value;
                                }
                                break;
                            case 3:
                                {
                                    record.Name = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    record.Code = code.Level1 + code.Level2 + code.Level3;
                    record.RegionLevel = RelativeRegionLevel.High;

                    regionInfo.LstRegion.Add(record.Code, record);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryLocalHighLevelRegionCode( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryLocalHighLevelRegionCode( " + ex.ToString() + " )");
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryLocalHighLevelRegionCode( end )");

            return 0;
        }
        /// <summary>
        /// 행정 구역 정보 취득(행정구역 코드 부분)
        /// </summary>
        /// <returns></returns>
        private int QueryLocalMiddleLevelRegionCode(ref RegionInfo regionInfo)
        {
            System.Console.WriteLine("[DBManager] QueryLocalMiddleLevelRegionCode( start )");

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryLocalMiddleLevelRegionCode( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryLocalMiddleLevelRegionCode( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryLocalMiddleLevelRegionCode(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryLocalMiddleLevelRegionCode( DB Open ERROR )");

                    return -1;
                }

                foreach (RegionProfile profile1 in regionInfo.LstRegion.Values)
                {
                    string query = "";
                    if (profile1.Code == NATIONAL_CODE)
                    {
                        // 중앙의 경우, 시도 레벨 검색
                        query = "SELECT CODEPART1, CODEPART2, CODEPART3, NAME FROM REGIONPROFILE WHERE VERSIONID = " + regionInfo.Version.ID +
                                        " AND CODEPART2 = '000' AND CODEPART3 = '00000'";
                    }
                    else if (profile1.CodePart2 == "000" && profile1.CodePart3 == "00000")
                    {
                        // 시도의 경우, 시군구 레벨 검색
                        query = "SELECT CODEPART1, CODEPART2, CODEPART3, NAME FROM REGIONPROFILE WHERE VERSIONID = " + regionInfo.Version.ID +
                                        " AND CODEPART1 = " + profile1.CodePart1 + " AND CODEPART3 = '00000'";
                    }
                    else if (profile1.CodePart3 == "00000")
                    {
                        // 시군구의 경우, 읍면동 레벨 검색
                        query = "SELECT CODEPART1, CODEPART2, CODEPART3, NAME FROM REGIONPROFILE WHERE VERSIONID = " + regionInfo.Version.ID +
                                        " AND CODEPART1 = " + profile1.CodePart1 + " AND CODEPART2 = '" + profile1.CodePart2 + "'";
                    }
                    else
                    {
                        // 존재하지 않는 케이스
                        return -10;
                    }
                    query = query + " ORDER BY NAME ASC";

                    QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                    if (resultData == null)
                    {
                        System.Console.WriteLine("[DBManager] QueryLocalMiddleLevelRegionCode( SQL Error : " + query + " )");
                        return -2;
                    }
                    if (resultData.DataCnt <= 0)
                    {
                        System.Console.WriteLine("[DBManager] QueryLocalMiddleLevelRegionCode( 레코드가 0건 )");

                        return 1;
                    }

                    foreach (RowData rowData in resultData.DataList)
                    {
                        RegionProfile record = new RegionProfile();
                        RegionCode code = new RegionCode();
                        for (int field = 0; field < rowData.FieldCnt; field++)
                        {
                            string value = rowData.FieldDataList[field];
                            switch (field)
                            {
                                case 0:
                                    {
                                        code.Level1 = value;
                                    }
                                    break;
                                case 1:
                                    {
                                        code.Level2 = value;
                                    }
                                    break;
                                case 2:
                                    {
                                        code.Level3 = value;
                                    }
                                    break;
                                case 3:
                                    {
                                        record.Name = value;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        record.Code = code.Level1 + code.Level2 + code.Level3;
                        if (record.Code == profile1.Code)
                        {
                            continue;
                        }
                        record.RegionLevel = RelativeRegionLevel.Middle;

                        profile1.LstSubRegion.Add(record.Code, record);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryLocalMiddleLevelRegionCode( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryLocalMiddleLevelRegionCode( " + ex.ToString() + " )");
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryLocalMiddleLevelRegionCode( end )");

            return 0;
        }
        /// <summary>
        /// 행정 구역 정보 취득(행정구역 코드 부분)
        /// </summary>
        /// <returns></returns>
        private int QueryLocalLowLevelRegionCode(ref RegionInfo regionInfo)
        {
            System.Console.WriteLine("[DBManager] QueryLocalLowLevelRegionCode( start )");

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryLocalLowLevelRegionCode( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryLocalLowLevelRegionCode( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryLocalLowLevelRegionCode(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryLocalLowLevelRegionCode( DB Open ERROR )");

                    return -1;
                }

                foreach (RegionProfile profile1 in regionInfo.LstRegion.Values)
                {
                    foreach (RegionProfile profile2 in profile1.LstSubRegion.Values)
                    {
                        string query = "";
                        if (profile1.CodePart1 == "00" && profile1.CodePart2 == "000" && profile1.CodePart3 == "00000")
                        {
                            // 중앙의 경우, 시군구 레벨 검색
                            query = "SELECT CODEPART1, CODEPART2, CODEPART3, NAME FROM REGIONPROFILE WHERE VERSIONID = " + regionInfo.Version.ID +
                                            " AND CODEPART1 = " + profile2.CodePart1 + " AND CODEPART3 = '00000'";
                        }
                        else if (profile1.CodePart2 == "000" && profile1.CodePart3 == "00000")
                        {
                            // 시도의 경우, 읍면동 레벨 검색
                            query = "SELECT CODEPART1, CODEPART2, CODEPART3, NAME FROM REGIONPROFILE WHERE VERSIONID = " + regionInfo.Version.ID +
                                            " AND CODEPART1 = " + profile2.CodePart1 + " AND CODEPART2 = '" + profile2.CodePart2 + "'";
                        }
                        else if (profile1.CodePart3 == "00000")
                        {
                            // 시군구의 경우, 더이상 검색 없음
                            return 0;
                        }
                        else
                        {
                            // 존재하지 않는 케이스
                            return -10;
                        }
                        query = query + " ORDER BY NAME ASC";

                        QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                        if (resultData == null)
                        {
                            System.Console.WriteLine("[DBManager] QueryLocalLowLevelRegionCode( SQL Error : " + query + " )");
                            return -2;
                        }
                        if (resultData.DataCnt <= 0)
                        {
                            System.Console.WriteLine("[DBManager] QueryLocalLowLevelRegionCode( 레코드가 0건 )");
                            return 1;
                        }

                        foreach (RowData rowData in resultData.DataList)
                        {
                            RegionProfile record = new RegionProfile();
                            RegionCode code = new RegionCode();
                            for (int field = 0; field < rowData.FieldCnt; field++)
                            {
                                string value = rowData.FieldDataList[field];
                                switch (field)
                                {
                                    case 0:
                                        {
                                            code.Level1 = value;
                                        }
                                        break;
                                    case 1:
                                        {
                                            code.Level2 = value;
                                        }
                                        break;
                                    case 2:
                                        {
                                            code.Level3 = value;
                                        }
                                        break;
                                    case 3:
                                        {
                                            record.Name = value;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            record.Code = code.Level1 + code.Level2 + code.Level3;
                            if (record.Code == profile2.Code)
                            {
                                continue;
                            }
                            record.RegionLevel = RelativeRegionLevel.Low;

                            profile2.LstSubRegion.Add(record.Code, record);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryLocalLowLevelRegionCode( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryLocalLowLevelRegionCode( " + ex.ToString() + " )");
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryLocalLowLevelRegionCode( end )");

            return 0;
        }
        /// <summary>
        /// 행정 구역 경계 데이터 취득 (미사용 가능성이 크다. 확인 후 삭제 요망)
        /// </summary>
        /// <returns></returns>
        private void QueryRegionOuterBoundary(ref RegionInfo districtInfo)
        {
            System.Console.WriteLine("[DBManager] QueryRegionOuterBoundary( start )");

            if (districtInfo == null || districtInfo.LstRegion == null)
            {
                System.Console.WriteLine("[DBManager] QueryRegionOuterBoundary( parameter error )");
                return;
            }

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryRegionOuterBoundary( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryRegionOuterBoundary( DBConnector 생성 에러 )");

                    return;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryRegionOuterBoundary(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryRegionOuterBoundary( DB Open ERROR )");

                    return;
                }

                // 최상위 지역
                foreach (RegionProfile profile1 in districtInfo.LstRegion.Values)
                {
                    string boundaryData = null;
                    if (QueryRegionOuterBoundaryData(districtInfo.Version.ID, profile1.Code, out boundaryData))
                    {
                        profile1.KmlText = boundaryData;
                    }

                    // 중간 지역
                    if (profile1.LstSubRegion == null || profile1.LstSubRegion.Count <= 0)
                    {
                        continue;
                    }
                    foreach (RegionProfile profile2 in profile1.LstSubRegion.Values)
                    {
                        if (QueryRegionOuterBoundaryData(districtInfo.Version.ID, profile2.Code, out boundaryData))
                        {
                            profile2.KmlText = boundaryData;
                        }

                        // 최하위 지역
                        if (profile2.LstSubRegion == null || profile2.LstSubRegion.Count <= 0)
                        {
                            continue;
                        }
                        foreach (RegionProfile profile3 in profile2.LstSubRegion.Values)
                        {
                            if (QueryRegionOuterBoundaryData(districtInfo.Version.ID, profile3.Code, out boundaryData))
                            {
                                profile3.KmlText = boundaryData;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryRegionOuterBoundary( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryRegionOuterBoundary( " + ex.ToString() + " )");
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryRegionOuterBoundary( end )");
        }
        private bool QueryRegionOuterBoundaryData(string versionID, string regionCode, out string boundaryData)
        {
//            System.Console.WriteLine("[DBManager] QueryRegionOuterBoundaryData( start )");

            boundaryData = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryRegionOuterBoundaryData( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryRegionOuterBoundaryData( DBConnector 생성 에러 )");

                    return false;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryRegionOuterBoundaryData(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryRegionOuterBoundaryData( DB Open ERROR )");

                    return false;
                }

                if (string.IsNullOrEmpty(regionCode))
                {
                    System.Console.WriteLine("[DBManager] QueryRegionOuterBoundaryData( parameter error )");
                    return false;
                }
                string query = "SELECT OUTERBOUNDARYDATA FROM REGION_OUTERBOUNDARY "
                                    + " WHERE REGIONCODE = '" + regionCode + "' AND VERSIONID = " + versionID;
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (null == resultData)
                {
                    System.Console.WriteLine("[DBManager] QueryRegionOuterBoundaryData( SQL Error )");
                    return false;
                }
                if (resultData.DataCnt < 1)
                {
                    System.Console.WriteLine("[DBManager] QueryRegionOuterBoundaryData( 해당 지역의 데이터가 없습니다. regionCode=["+regionCode+"])");
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        switch (field)
                        {
                            case 0:
                                {
                                    boundaryData = rowData.FieldDataList[field];
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryRegionOuterBoundaryData( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryRegionOuterBoundaryData( " + ex.ToString() + " )");
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

//            System.Console.WriteLine("[DBManager] QueryRegionOuterBoundaryData( end )");
            return true;
        }
        #endregion

        #region 기상특보(기초정보)
        /// <summary>
        /// 기상 특보 구역에 대한 기초 정보 취득
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, SWRAnnounceArea> QueryDefinitionOfSWRAnnounceArea()
        {
            System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRAnnounceArea( start )");

            Dictionary<string, SWRAnnounceArea> list = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRAnnounceArea( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSWRAnnounceArea( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRAnnounceArea(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSWRAnnounceArea( DB Open ERROR )");

                    return null;
                }

                string query = "SELECT CODE, NAME, TARGETREGIONS FROM SWR_ANNOUNCEAREA";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRAnnounceArea( SQL Error : " + query + " )");
                    return null;
                }

                list = new Dictionary<string, SWRAnnounceArea>();

                if (resultData.DataCnt <= 0)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRAnnounceArea( 레코드가 0건 )");
                    return list;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    SWRAnnounceArea record = new SWRAnnounceArea();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    record.AreaCode = value;
                                }
                                break;
                            case 1:
                                {
                                    record.AreaName = value;
                                }
                                break;
                            case 2:
                                {
                                    record.TargetRegions = ConvertToList(value, ",");
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    list.Add(record.AreaCode, record);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRAnnounceArea( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSWRAnnounceArea( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRAnnounceArea( end )");
            return list;
        }
        /// <summary>
        /// 기상 특보 발표 코드에 대한 기초 정보 취득
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> QueryDefinitionOfSWRCommand()
        {
            System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRCommand( start )");

            Dictionary<string, string> list = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRCommand( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSWRCommand( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRCommand(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSWRCommand( DB Open ERROR )");

                    return null;
                }

                string query = "SELECT CODE, DESCRIPTION FROM SWR_COMMAND";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRCommand( SQL Error : " + query + " )");
                    return null;
                }

                list = new Dictionary<string, string>();

                if (resultData.DataCnt <= 0)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRCommand( 레코드가 0건 )");
                    return list;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    SWRCommand record = new SWRCommand();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    record.Code = value;
                                }
                                break;
                            case 1:
                                {
                                    record.Description = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    if (list.ContainsKey(record.Code))
                    {
                        System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRCommand( 이미 등록된 코드입니다. code=[" + record.Code + "] )");
                        continue;
                    }
                    list.Add(record.Code, record.Description);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRCommand( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSWRCommand( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRCommand( end )");
            return list;
        }
        /// <summary>
        /// 기상 특보 강도에 대한 기초 정보 취득
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> QueryDefinitionOfSWRStress()
        {
            System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRStress( start )");

            Dictionary<string, string> list = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRStress( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSWRStress( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRStress(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSWRStress( DB Open ERROR )");

                    return null;
                }

                string query = "SELECT CODE, DESCRIPTION FROM SWR_STRESS";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRStress( SQL Error : " + query + " )");
                    return null;
                }

                list = new Dictionary<string, string>();

                if (resultData.DataCnt <= 0)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRStress( 레코드가 0건 )");
                    return list;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    SWRStress record = new SWRStress();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    record.Code = value;
                                }
                                break;
                            case 1:
                                {
                                    record.Description = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    if (list.ContainsKey(record.Code))
                    {
                        System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRStress( 이미 등록된 코드입니다. code=[" + record.Code + "] )");
                        continue;
                    }
                    list.Add(record.Code, record.Description);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRStress( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSWRStress( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRStress( end )");
            return list;
        }
        /// <summary>
        /// 기상 특보 종류에 대한 기초 정보 취득
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> QueryDefinitionOfSWRKind()
        {
            System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRKind( start )");

            Dictionary<string, string> list = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRKind( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSWRKind( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRKind(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSWRKind( DB Open ERROR )");

                    return null;
                }

                string query = "SELECT CODE, DESCRIPTION FROM SWR_KIND";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRKind( SQL Error : " + query + " )");
                    return null;
                }

                list = new Dictionary<string, string>();

                if (resultData.DataCnt <= 0)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRKind( 레코드가 0건 )");
                    return list;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    if (rowData == null || rowData.FieldCnt < 1)
                    {
                        continue;
                    }

                    SWRKind record = new SWRKind();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    record.Code = value;
                                }
                                break;
                            case 1:
                                {
                                    record.Description = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    if (list.ContainsKey(record.Code))
                    {
                        System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRKind( 이미 등록된 코드입니다. code=[" + record.Code + "] )");
                        continue;
                    }
                    list.Add(record.Code, record.Description);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRKind( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSWRKind( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRKind( end )");
            return list;
        }
        /// <summary>
        /// 기상특보에 매칭되는 재난 종류 매칭 정보를 조회한다.
        /// 기상 특보 종류+강도에 대한 재난 종류 코드 정보를 포함하고 있다.
        /// </summary>
        /// <returns></returns>
        public List<SWRDisasterMatching> QueryDefinitionOfSWRDisaster()
        {
            System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRDisaster( start )");

            List<SWRDisasterMatching> list = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRDisaster( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSWRDisaster( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRDisaster(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSWRDisaster( DB Open ERROR )");

                    return null;
                }

                string query = "SELECT ID, WARNKIND_CODE, WARNSTRESS_CODE, DISASTERKIND_CODE FROM SWR_MATCHING_INFO";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRDisaster( SQL Error : " + query + " )");
                    return null;
                }

                list = new List<SWRDisasterMatching>();

                if (resultData.DataCnt <= 0)
                {
                    System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRDisaster( 레코드가 0건 )");
                    return list;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    SWRDisasterMatching record = new SWRDisasterMatching();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    record.ID = value;
                                }
                                break;
                            case 1:
                                {
                                    record.SwrKindCode = value;
                                }
                                break;
                            case 2:
                                {
                                    record.SwrStressCode = value;
                                }
                                break;
                            case 3:
                                {
                                    record.DisasterKindCode = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    list.Add(record);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRDisaster( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryDefinitionOfSWRDisaster( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryDefinitionOfSWRDisaster( end )");
            return list;
        }
        #endregion

        #region 발령이력
        /// <summary>
        /// 발령 정보 취득
        /// </summary>
        /// <returns></returns>
        public OrderRecord QueryOrderInfo(string orderId)
        {
            System.Console.WriteLine("[DBManager] QueryOrderInfo( start )");
            OrderRecord orderInfo = null;

            System.Console.WriteLine("[DBManager] QueryOrderInfo( end )");
            return orderInfo;
        }

        /// <summary>
        /// 발령 정보 등록.
        /// 경보를 수행할 것으로 예상되는 표준경보시스템 목록으로 응답 정보를 가등록 한다.
        /// </summary>
        /// <returns></returns>
        public int RegistOrderRecord(OrderRecord orderInfo)
        {
            System.Console.WriteLine("[DBManager] RegistOrderRecord2( start )");

            int result = 0;
            bool transactionStarted = false;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] RegistOrderRecord2( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] RegistOrderRecord2( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] RegistOrderRecord2(DB Open ERROR)");
                    FileLogManager.GetInstance().WriteLog("[DBManager] RegistOrderRecord2( DB Open ERROR )");

                    result = -1;
                    return result;
                }

                // 트랜잭션 시작
                transactionStarted = dbConnector.BeginTransaction();
                if (!transactionStarted)
                {
                    result = -10;
                    return result;
                }

                string refRecordID = "NULL";
                if (!string.IsNullOrEmpty(orderInfo.RefRecordID))
                {
                    refRecordID = "'" + orderInfo.RefRecordID + "'";
                }
                string clearState = ((int)ClearAlertState.Waiting).ToString();
                if (orderInfo.ClearState != null)
                {
                    clearState = ((int)orderInfo.ClearState.Code).ToString();
                }

                string query = "INSERT INTO ORDER_RECORD (CAP_ID, ORDERED_TIME, ORDER_MODE, DISASTERKIND_CODE, " +
                                "REF_TYPE, REF_RECORDID, LOCATION_KIND, CLEAR_STATUS, CAP_TEXT) "
                            + " VALUES ('" + orderInfo.CAPID + "', TO_DATE(" + this.ConvertToLocalDateType(orderInfo.OrderedTime) + ", 'YYYYMMDDHH24MISS'), "
                            + (int)orderInfo.OrderMode + ", " + "'" + orderInfo.DisasterKindCode + "', "
                            + (int)orderInfo.RefType + ", " + refRecordID + ", " + (int)orderInfo.LocationKind + ", " + clearState + ", "
                            + "EMPTY_CLOB())";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (null == resultData)
                {
                    System.Console.WriteLine("[DBManager] RegistOrderRecord2( SQL Error=[" + query + "] )");
                    result = -2;
                    return result;
                }
                if (resultData.AffectedRecordCnt != 1)
                {
                    System.Console.WriteLine("[DBManager] RegistOrderRecord2( 갱신 결과 오류 : resultData.AffectedRecordCnt=[" + resultData.AffectedRecordCnt + "] )");
                    result = 1;
                    return result;
                }

                // 캡 원문 등록
                string updateQuery = string.Format("SELECT CAP_ID, CAP_TEXT FROM ORDER_RECORD WHERE CAP_ID = '{0}' FOR UPDATE", orderInfo.CAPID);
                resultData = dbConnector.ExecuteQueryForClob(updateQuery, orderInfo.CapText);
                if (null == resultData)
                {
                    System.Console.WriteLine("[DBManager] RegistOrderRecord2( SQL Error2 = [" + updateQuery + "])");
                    result = -3;
                    return result;
                }

                result = 0;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] RegistOrderRecord2( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] RegistOrderRecord2( " + ex.ToString() + " )");

                result = -99;
                return result;
            }
            finally
            {
                if (transactionStarted)
                {
                    if (result == 0)
                    {
                        dbConnector.EndTransaction(true);
                    }
                    else
                    {
                        dbConnector.EndTransaction(false);
                    }
                }

                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] RegistOrderRecord2( end )");
            return result;
        }
        /// <summary>
        /// 최근 발령 기록 조회
        /// </summary>
        /// <returns></returns>
        public int QueryRecentlyOrderHistory(out List<OrderRecord> history)
        {
            System.Console.WriteLine("[DBManager] QueryRecentlyOrderHistory( start )");

            int result = 0;
            history = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryRecentlyOrderHistory( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryRecentlyOrderHistory( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryRecentlyOrderHistory( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryRecentlyOrderHistory( DB Open ERROR )");

                    return -1;
                }

                string selectionColumns = "CAP_ID, ORDERED_TIME, ORDER_MODE, DISASTERKIND_CODE, REF_TYPE, REF_RECORDID, LOCATION_KIND, CLEAR_STATUS, CAP_TEXT";
                string innerQuery = "SELECT " + selectionColumns + ", ROWNUM" +
                                " FROM ORDER_RECORD " +
                                " ORDER BY ORDERED_TIME DESC";

                string query = "SELECT " + selectionColumns +
                                " FROM ( " + innerQuery + " )" +
                                " WHERE ROWNUM <= 30";

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryRecentlyOrderHistory( SQL Error = [" + query + "])");
                    return -2;
                }

                history = new List<OrderRecord>();
                foreach (RowData rowData in resultData.DataList)
                {
                    OrderRecord record = new OrderRecord();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    record.CAPID = value;
                                }
                                break;
                            case 1:
                                {
                                    DateTime temp = new DateTime();
                                    if (DateTime.TryParse(value, out temp))
                                    {
                                        record.OrderedTime = temp;
                                    }
                                    else
                                    {
                                        record.OrderedTime = new DateTime();
                                    }
                                }
                                break;
                            case 2:
                                {
                                    record.OrderMode = StatusType.Actual;
                                    StatusType temp = StatusType.Actual;
                                    if (Enum.TryParse<StatusType>(value, out temp))
                                    {
                                        record.OrderMode = temp;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    record.DisasterKindCode = value;
                                }
                                break;
                            case 4:
                                {
                                    record.RefType = OrderReferenceType.None;
                                    OrderReferenceType temp = OrderReferenceType.None;
                                    if (Enum.TryParse<OrderReferenceType>(value, out temp))
                                    {
                                        record.RefType = temp;
                                    }
                                }
                                break;
                            case 5:
                                {
                                    record.RefRecordID = value;
                                }
                                break;
                            case 6:
                                {
                                    record.LocationKind = OrderLocationKind.Local;
                                    OrderLocationKind temp = OrderLocationKind.Local;
                                    if (Enum.TryParse<OrderLocationKind>(value, true, out temp))
                                    {
                                        record.LocationKind = temp;
                                    }
                                }
                                break;
                            case 7:
                                {
                                    record.ClearState = null;
                                    ClearAlertState temp = ClearAlertState.Waiting;
                                    if (Enum.TryParse<ClearAlertState>(value, true, out temp))
                                    {
                                        record.ClearState = new AlertingClearState();
                                        record.ClearState.DeepCopyFrom(BasisData.AlertingClearStateInfo[temp]);
                                    }
                                }
                                break;
                            case 8:
                                {
                                    record.CapText = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    history.Add(record);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryRecentlyOrderHistory( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryRecentlyOrderHistory( " + ex.ToString() + " )");

                result = -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryRecentlyOrderHistory( end )");
            return result;
        }
        /// <summary>
        /// 최근 발령 기록 조회
        /// </summary>
        /// <returns></returns>
        public int QueryWaitToClearAlertList(out List<OrderRecord> waitingList)
        {
            System.Console.WriteLine("[DBManager] QueryWaitToClearAlertList( start )");

            int result = 0;
            waitingList = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryWaitToClearAlertList( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryWaitToClearAlertList( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryWaitToClearAlertList( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryWaitToClearAlertList( DB Open ERROR )");

                    return -1;
                }

                string selectionColumns = "CAP_ID, ORDERED_TIME, ORDER_MODE, DISASTERKIND_CODE, REF_TYPE, REF_RECORDID, LOCATION_KIND, CLEAR_STATUS, CAP_TEXT";
                string innerQuery = "SELECT " + selectionColumns + ", ROWNUM" +
                                " FROM ORDER_RECORD " +
                                " WHERE CLEAR_STATUS = 0 AND DISASTERKIND_CODE != 'DWC'" +
                                " ORDER BY ORDERED_TIME DESC";

                string query = "SELECT " + selectionColumns +
                                " FROM ( " + innerQuery + " )" +
                                " WHERE ROWNUM <= 30";

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryWaitToClearAlertList( SQL Error = [" + query + "])");
                    return -2;
                }

                waitingList = new List<OrderRecord>();
                foreach (RowData rowData in resultData.DataList)
                {
                    OrderRecord record = new OrderRecord();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    record.CAPID = value;
                                }
                                break;
                            case 1:
                                {
                                    DateTime temp = new DateTime();
                                    if (DateTime.TryParse(value, out temp))
                                    {
                                        record.OrderedTime = temp;
                                    }
                                    else
                                    {
                                        record.OrderedTime = new DateTime();
                                    }
                                }
                                break;
                            case 2:
                                {
                                    record.OrderMode = StatusType.Actual;
                                    StatusType temp = StatusType.Actual;
                                    if (Enum.TryParse<StatusType>(value, out temp))
                                    {
                                        record.OrderMode = temp;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    record.DisasterKindCode = value;
                                }
                                break;
                            case 4:
                                {
                                    record.RefType = OrderReferenceType.None;
                                    OrderReferenceType temp = OrderReferenceType.None;
                                    if (Enum.TryParse<OrderReferenceType>(value, out temp))
                                    {
                                        record.RefType = temp;
                                    }
                                }
                                break;
                            case 5:
                                {
                                    record.RefRecordID = value;
                                }
                                break;
                            case 6:
                                {
                                    record.LocationKind = OrderLocationKind.Local;
                                    OrderLocationKind temp = OrderLocationKind.Local;
                                    if (Enum.TryParse<OrderLocationKind>(value, true, out temp))
                                    {
                                        record.LocationKind = temp;
                                    }
                                }
                                break;
                            case 7:
                                {
                                    record.ClearState = null;
                                    ClearAlertState temp = ClearAlertState.Waiting;
                                    if (Enum.TryParse<ClearAlertState>(value, true, out temp))
                                    {
                                        record.ClearState = new AlertingClearState();
                                        record.ClearState.DeepCopyFrom(BasisData.AlertingClearStateInfo[temp]);
                                    }
                                }
                                break;
                            case 8:
                                {
                                    record.CapText = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    waitingList.Add(record);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryWaitToClearAlertList( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryWaitToClearAlertList( " + ex.ToString() + " )");

                result = -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryWaitToClearAlertList( end )");
            return result;
        }
        /// <summary>
        /// 발령 기록 조회
        /// </summary>
        /// <returns></returns>
        public int QueryOrderHistory(OrderInquiryCondition condition, out List<OrderRecord> historyList)
        {
            System.Console.WriteLine("[DBManager] QueryOrderHistory( start )");

            int result = 0;
            historyList = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryOrderHistory( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryOrderHistory( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryOrderHistory( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryOrderHistory( DB Open ERROR )");

                    return -1;
                }

                bool needAnd = false;
                StringBuilder builder = new StringBuilder();
                if (condition.StartTime.Ticks > 0 && condition.EndTime.Ticks > 0)
                {
                    if (needAnd)
                    {
                        builder.Append(" AND");
                    }
                    string start = string.Format("{0:yyyy}{0:MM}{0:dd}", condition.StartTime);
                    string end = string.Format("{0:yyyy}{0:MM}{0:dd}", condition.EndTime.AddDays(1));
                    builder.Append(" ORDERED_TIME > '" + start + "' AND ORDERED_TIME < '" + end + "'");
                    needAnd = true;
                }
                if (condition.Disaster != null && condition.Disaster.Kind != null && !string.IsNullOrEmpty(condition.Disaster.Kind.Code))
                {
                    if (needAnd)
                    {
                        builder.Append(" AND");
                    }
                    builder.Append(" DISASTERKIND_CODE = '" + condition.Disaster.Kind.Code + "'");
                    needAnd = true;
                }
                else if (condition.Disaster != null && condition.Disaster.Category != null && condition.Disaster.Kind == null)
                {
                    // 카테고리만 선택한 경우에는 카테고리 이하의 모든 재난 종류를 조건으로 걸어야 한다.
                }
                else
                {
                }
                if (condition.OrderMode != null)
                {
                    if (needAnd)
                    {
                        builder.Append(" AND");
                    }
                    builder.Append(" ORDER_MODE = " + (int)condition.OrderMode.Code);
                    needAnd = true;
                }
                if (condition.OrderLocationKind != null)
                {
                    if (needAnd)
                    {
                        builder.Append(" AND");
                    }
                    builder.Append(" LOCATION_KIND = " + (int)condition.OrderLocationKind.Code);
                    needAnd = true;
                }
                string queryCondition = string.Empty;
                if (builder.Length > 0)
                {
                    queryCondition = " WHERE " + builder.ToString();
                }

                string selectionColumns = "CAP_ID, ORDERED_TIME, ORDER_MODE, DISASTERKIND_CODE, REF_TYPE, REF_RECORDID, LOCATION_KIND, CLEAR_STATUS, CAP_TEXT";
                string innerQuery = "SELECT " + selectionColumns + ", ROWNUM" +
                                " FROM ORDER_RECORD " +
                                queryCondition +
                                " ORDER BY ORDERED_TIME DESC";

                string query = "SELECT " + selectionColumns +
                                " FROM ( " + innerQuery + " )" +
                                " WHERE ROWNUM <= " + condition.Count;

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryOrderHistory( SQL Error = [" + query + "])");
                    return -2;
                }
                if (resultData.DataCnt < 1)
                {
                    System.Console.WriteLine("[DBManager] QueryOrderHistory( 레코드가 0건 )");
                    return 1;
                }

                bool needCheckDisasterCategory = false;
                if (condition.Disaster != null && (condition.Disaster.Category != null && condition.Disaster.Kind == null))
                {
                    needCheckDisasterCategory = true;
                }

                historyList = new List<OrderRecord>();
                foreach (RowData rowData in resultData.DataList)
                {
                    OrderRecord record = new OrderRecord();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    record.CAPID = value;
                                }
                                break;
                            case 1:
                                {
                                    DateTime temp = new DateTime();
                                    if (DateTime.TryParse(value, out temp))
                                    {
                                        record.OrderedTime = temp;
                                    }
                                    else
                                    {
                                        record.OrderedTime = new DateTime();
                                    }
                                }
                                break;
                            case 2:
                                {
                                    record.OrderMode = StatusType.Actual;
                                    StatusType temp = StatusType.Actual;
                                    if (Enum.TryParse<StatusType>(value, out temp))
                                    {
                                        record.OrderMode = temp;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    record.DisasterKindCode = value;
                                }
                                break;
                            case 4:
                                {
                                    record.RefType = OrderReferenceType.None;
                                    OrderReferenceType temp = OrderReferenceType.None;
                                    if (Enum.TryParse<OrderReferenceType>(value, out temp))
                                    {
                                        record.RefType = temp;
                                    }
                                }
                                break;
                            case 5:
                                {
                                    record.RefRecordID = value;
                                }
                                break;
                            case 6:
                                {
                                    record.LocationKind = OrderLocationKind.Local;
                                    OrderLocationKind temp = OrderLocationKind.Local;
                                    if (Enum.TryParse<OrderLocationKind>(value, true, out temp))
                                    {
                                        record.LocationKind = temp;
                                    }
                                }
                                break;
                            case 7:
                                {
                                    record.ClearState = null;
                                    ClearAlertState temp = ClearAlertState.Waiting;
                                    if (Enum.TryParse<ClearAlertState>(value, true, out temp))
                                    {
                                        record.ClearState = new AlertingClearState();
                                        record.ClearState.DeepCopyFrom(BasisData.AlertingClearStateInfo[temp]);
                                    }
                                }
                                break;
                            case 8:
                                {
                                    record.CapText = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    if (needCheckDisasterCategory)
                    {
                        DisasterKind kindInfo = BasisData.FindDisasterKindByCode(record.DisasterKindCode);
                        if (kindInfo != null && kindInfo.CategoryID == condition.Disaster.Category.ID)
                        {
                            historyList.Add(record);
                        }
                    }
                    else
                    {
                        historyList.Add(record);
                    }
                }

                result = 0;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryOrderHistory( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryOrderHistory( " + ex.ToString() + " )");

                result = -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryOrderHistory( end )");
            return result;
        }
        /// <summary>
        /// 발령 응답 기록 조회
        /// </summary>
        /// <returns></returns>
        public int QueryOrderResponse(string orderID, out List<OrderResponseProfile> responseList)
        {
            System.Console.WriteLine("[DBManager] QueryOrderResponse( start )");
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(orderID));

            responseList = null;

            if (string.IsNullOrEmpty(orderID))
            {
                System.Console.WriteLine("QueryOrderResponse: 입력 파라미터 오류");
                return -1;
            }

            int result = 0;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryOrderResponse( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryOrderResponse( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryOrderResponse( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryOrderResponse( DB Open ERROR )");

                    return -1;
                }

                string query = "SELECT ID, REFCAP_ID, SENDER_ID, SENDER_TYPE, CAP_MSG" +
                                ", SYSTEM_MANAGER_NAME, SYSTEM_MANAGER_DEPARTMENT, SYSTEM_MANAGER_PHONE" +
                                ", RECV_TIME" +
                                " FROM ORDER_RESPONSE" + 
                                " WHERE REFCAP_ID = '" + orderID + "' AND SENDER_TYPE = " + (int)SenderTypes.SAS +
                                " ORDER BY SENDER_ID ASC";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryOrderResponse( SQL Error = [" + query + "])");
                    return -2;
                }
                if (resultData.DataCnt < 1)
                {
                    System.Console.WriteLine("[DBManager] QueryOrderResponse( 레코드가 0건 )");
                    return 1;
                }

                responseList = new List<OrderResponseProfile>();
                foreach (RowData rowData in resultData.DataList)
                {
                    OrderResponseProfile record = new OrderResponseProfile();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    record.ID = value;
                                }
                                break;
                            case 1:
                                {
                                    record.ReferenceCapID = value;
                                }
                                break;
                            case 2:
                                {
                                    record.SenderID = value;
                                }
                                break;
                            case 3:
                                {
                                    record.SenderType = SenderTypes.NONE;
                                    SenderTypes temp = SenderTypes.NONE;
                                    if (Enum.TryParse<SenderTypes>(value, out temp))
                                    {
                                        record.SenderType = temp;
                                    }
                                }
                                break;
                            case 4:
                                {
                                    record.CapMsg = value;
                                }
                                break;
                            case 5:
                                {
                                    record.SystemManagerName = value;
                                }
                                break;
                            case 6:
                                {
                                    record.SystemManagerDepartment = value;
                                }
                                break;
                            case 7:
                                {
                                    record.SystemManagerPhone = value;
                                }
                                break;
                            case 8:
                                {
                                    DateTime temp = new DateTime();
                                    if (DateTime.TryParse(value, out temp))
                                    {
                                        record.RecvTime = temp;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    responseList.Add(record);
                }

                result = 0;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryOrderResponse( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryOrderResponse( " + ex.ToString() + " )");

                result = -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryOrderResponse( end )");
            return result;
        }
        public int ExpectedTargetSystemList(List<RegionDefinition> targetRegions, List<SASKind> systemKinds, out List<string> expectedTargetSystems)
        {
            System.Console.WriteLine("[DBManager] ExpectedTargetSystemList( start )");

            int ret = 0;

            expectedTargetSystems = new List<string>();

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] ExpectedTargetSystemList( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] ExpectedTargetSystemList( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] ExpectedTargetSystemList( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] ExpectedTargetSystemList( DB Open ERROR )");

                    return -1;
                }

                // 경보 표출 예상 표준발령대 목록 추출
                foreach (RegionDefinition region in targetRegions)
                {
                    string code = region.Code.Level1;
                    if (region.Code.Level2 != "000" && region.Code.Level3 == "00000")
                    {
                        code += region.Code.Level2;
                    }
                    else if (region.Code.Level2 != "000" && region.Code.Level3 != "00000")
                    {
                        code += region.Code.Level3;
                    }
                    else
                    {
                    }

                    string query = string.Empty;
                    if (region.Code.ToString() == NATIONAL_CODE)
                    {
                        query = "SELECT ID, SYSTEMKIND FROM SASPROFILE";
                    }
                    else
                    {
                        query = "SELECT ID, SYSTEMKIND FROM SASPROFILE WHERE ASSIGNEDIAGWREGIONCODE like '" + code + "%'";
                    }

                    QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                    if (resultData == null || resultData.DataCnt < 1)
                    {
                        System.Console.WriteLine("[DBManager] ExpectedTargetSystemList(지역코드[" + code + " (" + region.Code + ")]에 위치한 표준경보시스템이 없습니다.)");
                        continue;
                    }

                    foreach (RowData rowData in resultData.DataList)
                    {
                        string systemID = rowData.FieldDataList[0];
                        string currentKind = rowData.FieldDataList[1];

                        if (systemKinds != null && systemKinds.Count > 0)
                        {
                            foreach (SASKind kind in systemKinds)
                            {
                                if (kind.Code == currentKind)
                                {
                                    expectedTargetSystems.Add(systemID);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            // 모든 경보 시스템 종류
                            expectedTargetSystems.Add(systemID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] ExpectedTargetSystemList( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] ExpectedTargetSystemList( " + ex.ToString() + " )");

                return -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] ExpectedTargetSystemList( end )");
            return ret;
        }
        /// <summary>
        /// 발령 응답 기록 갱신
        /// </summary>
        /// <returns></returns>
        public int UpdateOrderResponse(OrderResponseProfile response)
        {
            System.Console.WriteLine("[DBManager] UpdateOrderResponse( start )");

            System.Diagnostics.Debug.Assert(response != null);

            int result = 0;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] UpdateOrderResponse( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateOrderResponse( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] UpdateOrderResponse( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateOrderResponse( DB Open ERROR )");

                    return -1;
                }

                string query = "";

                string identifier = "'"+ response.ID + "'";
                if (string.IsNullOrEmpty(response.ID))
                {
                    identifier = "null";
                }

                string refCapID = "'" + response.ReferenceCapID + "'";
                if (string.IsNullOrEmpty(response.ReferenceCapID))
                {
                    refCapID = "null";
                }

                string senderID = "'" + response.SenderID + "'";
                if (string.IsNullOrEmpty(response.SenderID))
                {
                    senderID = "null";
                }

                string senderType = ((int)response.SenderType).ToString();

                string managerName = "'" + response.SystemManagerName + "'";
                if (string.IsNullOrEmpty(response.SystemManagerName))
                {
                    managerName = "null";
                }
                string managerDepartment = "'" + response.SystemManagerDepartment + "'";
                if (string.IsNullOrEmpty(response.SystemManagerDepartment))
                {
                    managerDepartment = "null";
                }
                string managerPhone = "'" + response.SystemManagerPhone + "'";
                if (string.IsNullOrEmpty(response.SystemManagerPhone))
                {
                    managerPhone = "null";
                }
                DateTime recvTime = DateTime.Now;
                if (response.RecvTime.Ticks > 0)
                {
                    recvTime = response.RecvTime;
                }

                bool isNew = true;
                query = "SELECT ID FROM ORDER_RESPONSE WHERE REFCAP_ID = " + refCapID + " AND SENDER_ID = " + senderID;
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null || resultData.DataCnt < 1)
                {
                    // 존재하지 않는 레코드 => 신규 레코드 등록
                    System.Console.WriteLine("[DBManager] UpdateOrderResponse( 신규=[" + refCapID + " ] )");
                }
                else
                {
                    // 레코드 갱신
                    isNew = false;
                    System.Console.WriteLine("[DBManager] UpdateOrderResponse( 갱신=[" + refCapID + " ] )");
                }

                if (isNew)
                {
                    // 신규 레코드: 등록
                    query = "INSERT INTO ORDER_RESPONSE (ID, REFCAP_ID, SENDER_ID, SENDER_TYPE, CAP_MSG, " +
                            "SYSTEM_MANAGER_NAME, SYSTEM_MANAGER_DEPARTMENT, SYSTEM_MANAGER_PHONE, RECV_TIME) " +
                            "VALUES (" + identifier + ", " + refCapID + ", " + senderID + ", " + senderType + ", '" + response.CapMsg + "'" +
                            ", " + managerName + ", " + managerDepartment + ", " + managerPhone +
                            ", TO_DATE( '" + recvTime.ToString("yyyyMMddHHmmss") + "', 'YYYYMMDDHH24MISS'))";

                }
                else
                {
                    query = "UPDATE ORDER_RESPONSE SET ID = " + identifier + 
                                    ", RECV_TIME = TO_DATE( '" + recvTime.ToString("yyyyMMddHHmmss") + "', 'YYYYMMDDHH24MISS')" + 
                                    ", CAP_MSG = '" + response.CapMsg + "'" +
                                "WHERE REFCAP_ID = " + refCapID + " AND SENDER_ID = " + senderID;
                }

                resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] UpdateOrderResponse( 발령 응답 정보 갱신 실패 : query=[" + query + "] )");
                    return -2;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] UpdateOrderResponse( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] UpdateOrderResponse( " + ex.ToString() + " )");

                result = -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] UpdateOrderResponse( end )");
            return result;
        }
        /// <summary>
        /// 발령 기록의 경보 해제 상태 정보 변경
        /// </summary>
        /// <returns></returns>
        public int UpdateAlertingClearStateOfOrderRecord(string targetID, ClearAlertState clearState)
        {
            System.Console.WriteLine("[DBManager] UpdateAlertingClearStateOfOrderRecord( start )");

            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(targetID));

            int result = 0;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] UpdateAlertingClearStateOfOrderRecord( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateAlertingClearStateOfOrderRecord( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] UpdateAlertingClearStateOfOrderRecord( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateAlertingClearStateOfOrderRecord( DB Open ERROR )");

                    return -1;
                }

                string query = "UPDATE ORDER_RECORD SET CLEAR_STATUS = " + (int)clearState + " WHERE CAP_ID = '" + targetID + "'";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (null == resultData)
                {
                    System.Console.WriteLine("[DBManager] UpdateAlertingClearStateOfOrderRecord( SQL Error = [" + query + "])");
                    return -2;
                }
                if (resultData.AffectedRecordCnt != 1)
                {
                    System.Console.WriteLine("[DBManager] UpdateAlertingClearStateOfOrderRecord( 갱신 결과 오류 : resultData.AffectedRecordCnt=[" + resultData.AffectedRecordCnt + "] )");
                    return 1;
                }

                result = 0;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] UpdateAlertingClearStateOfOrderRecord( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] UpdateAlertingClearStateOfOrderRecord( " + ex.ToString() + " )");

                return -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] UpdateAlertingClearStateOfOrderRecord( end )");
            return result;
        }
        #endregion


        #region 기상특보이력
        /// <summary>
        /// 기상 특보 정보 등록
        /// </summary>
        /// <returns></returns>
        public int RegistSWRInfo(List<SWRProfile> profileList)
        {
            System.Console.WriteLine("[DBManager] RegistSWRInfo( start )");
            System.Diagnostics.Debug.Assert(profileList != null);

            int result = -1;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] RegistSWRInfo( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] RegistSWRInfo( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] RegistSWRInfo( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] RegistSWRInfo( DB Open ERROR )");

                    result = -2;
                    return result;
                }

                // 트랜잭션 시작
                bool isStarted = dbConnector.BeginTransaction();
                if (!isStarted)
                {
                    result = -20;
                    return result;
                }

                // ID순으로 정렬.
                profileList.Sort(CompareSWRProfileByID);

                foreach (SWRProfile newProfile in profileList)
                {
                    string query = "SELECT ID" +
                                    " FROM ( SELECT ID, ROWNUM FROM SWR_PROFILE ORDER BY ID DESC )" +
                                    " WHERE ROWNUM <= 1";
                    QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                    if (null == resultData)
                    {
                        System.Console.WriteLine("[DBManager] RegistSWRInfo( SQL Error1 = [" + query + "])");
                        return -2;
                    }

                    bool needCheckID = false;
                    int lastReportID = 0;
                    if (resultData.DataCnt > 0 && resultData.DataList[0].FieldCnt > 0)
                    {
                        needCheckID = true;
                        string value = resultData.DataList[0].FieldDataList[0];
                        int temp = 0;
                        if (int.TryParse(value, out temp))
                        {
                            lastReportID = temp;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (needCheckID)
                    {
                        int currentReportID = 0;
                        int temp = 0;
                        if (int.TryParse(newProfile.ID, out temp))
                        {
                            currentReportID = temp;
                        }
                        else
                        {
                            continue;
                        }

                        if (currentReportID <= lastReportID)
                        {
                            continue;
                        }
                    }


                    string receivedTime = string.Format("{0:yyyy}{0:MM}{0:dd}", newProfile.ReceivedTime);

                    query = "INSERT INTO SWR_PROFILE (ID, TARGET_AREAS, WARN_KINDCODE, WARN_STRESSCODE, WARN_COMMANDCODE, ORIGINAL_REPORT, RECEIVED_TIME, ASSOCIATION_STATE) "
                            + " VALUES ('" + newProfile.ID + "', EMPTY_CLOB()"
                                + ", " + newProfile.WarnKindCode + ", " + newProfile.WarnStressCode + ", " + newProfile.CommandCode + " "
                                + ", EMPTY_CLOB()"
                                + ", TO_DATE( '" + newProfile.ReceivedTime.ToString("yyyyMMddHHmmss") + "', 'YYYYMMDDHH24MISS')"
                                + ", " + (int)newProfile.AssociationState + ")";
                    resultData = dbConnector.ExecuteQuery(query);
                    if (null == resultData)
                    {
                        System.Console.WriteLine("[DBManager] RegistSWRInfo( SQL Error 2 )");
                        result = -2;
                        return result;
                    }

                    if (resultData.AffectedRecordCnt != 1)
                    {
                        System.Console.WriteLine("[DBManager] RegistSWRInfo( 갱신 결과 오류 : resultData.AffectedRecordCnt=[" + resultData.AffectedRecordCnt + "] )");
                        result = 1;
                        return result;
                    }

                    // 대상구역 정보 등록
                    string updateQuery = string.Format("SELECT ID, TARGET_AREAS FROM SWR_PROFILE WHERE ID = '{0}' FOR UPDATE", newProfile.ID);
                    resultData = dbConnector.ExecuteQueryForClob(updateQuery, newProfile.TargetAreas);
                    if (null == resultData)
                    {
                        System.Console.WriteLine("[DBManager] RegistSWRInfo( SQL(CLOB) Error1 = [" + updateQuery + "])");
                        result = -3;
                        return result;
                    }
                    updateQuery = string.Format("SELECT ID, ORIGINAL_REPORT FROM SWR_PROFILE WHERE ID = '{0}' FOR UPDATE", newProfile.ID);
                    resultData = dbConnector.ExecuteQueryForClob(updateQuery, newProfile.OriginalWarningItemReport);
                    if (null == resultData)
                    {
                        System.Console.WriteLine("[DBManager] RegistSWRInfo( SQL(CLOB) Error2 = [" + updateQuery + "])");
                        result = -4;
                        return result;
                    }

                    result = 0;
                }

                // 트랜잭션 종료
                dbConnector.EndTransaction(true);

                result = 0;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] RegistSWRInfo( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] RegistSWRInfo( " + ex.ToString() + " )");

                return -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    if (result != 0)
                    {
                        dbConnector.EndTransaction(false);
                    }
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] RegistSWRInfo( end )");
            return result;
        }
        /// <summary>
        /// 기상 특보 기록 조회
        /// </summary>
        /// <returns></returns>
        public SWRProfile QuerySWRProfile(string reportID)
        {
            System.Console.WriteLine("[DBManager] QuerySWRProfile( start )");
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(reportID));

            SWRProfile profile = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QuerySWRProfile( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QuerySWRProfile( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QuerySWRProfile( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QuerySWRProfile( DB Open ERROR )");

                    return null;
                }

                string query = "SELECT ID, RECEIVED_TIME, ORIGINAL_REPORT, ASSOCIATION_STATE" +
                                " FROM SWR_PROFILE" +
                                " WHERE ASSOCIATION_STATE = 0" +
                                " ORDER BY ID DESC";

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QuerySWRProfile( 기상특보 이력 데이터 취득 실패 )");
                    return null;
                }
                if (resultData.DataCnt < 1)
                {
                    System.Console.WriteLine("[DBManager] QuerySWRProfile( 조회된 레코드가 0건. )");
                    return null;
                }

                RowData rowData = resultData.DataList[0];
                profile = ConvertRowDataToSWRProfile(rowData);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QuerySWRProfile( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QuerySWRProfile( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QuerySWRProfile( end )");
            return profile;
        }
        /// <summary>
        /// 기상 특보 가장 최근 아이디 조회.
        /// </summary>
        /// <returns></returns>
        public int QueryLatestSWRProfileID(out string reportSeq)
        {
            System.Console.WriteLine("[DBManager] QueryLatestSWRProfileID( start )");

            int result = 0;
            reportSeq = string.Empty;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryLatestSWRProfileID( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryLatestSWRProfileID( DBConnector 생성 에러 )");

                    return -1;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryLatestSWRProfileID( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryLatestSWRProfileID( DB Open ERROR )");

                    return -2;
                }

                string query = "SELECT ID" +
                                " FROM ( SELECT ID, ROWNUM FROM SWR_PROFILE ORDER BY ID DESC )" +
                                " WHERE ROWNUM <= 1";

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryLatestSWRProfileID( DB Open ERROR )");
                    return -3;
                }
                if (resultData.DataCnt != 1)
                {
                    System.Console.WriteLine("[DBManager] QueryLatestSWRProfileID( 아직 이력이 없음. )");
                    return 0;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    reportSeq = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryLatestSWRProfileID( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryLatestSWRProfileID( " + ex.ToString() + " )");

                return -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryLatestSWRProfileID( end )");
            return result;
        }
        /// <summary>
        /// 기상 특보의 발령 연계 상태 변경
        /// 상태: 0(대기), 1(발령완료), 2(발령제외)
        /// </summary>
        /// <returns></returns>
        public int UpdateSWRAssociationState(string reportID, SWRAssociationStateCode state)
        {
            System.Console.WriteLine("[DBManager] UpdateSWRAssociationState( start )");

            int result = 0;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] UpdateSWRAssociationState( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateSWRAssociationState( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] UpdateSWRAssociationState( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateSWRAssociationState( DB Open ERROR )");

                    return -1;
                }

                string query = "UPDATE SWR_PROFILE SET ASSOCIATION_STATE = " + (int)state + " WHERE ID = '" + reportID + "'";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (null == resultData)
                {
                    System.Console.WriteLine("[DBManager] UpdateSWRAssociationState( SQL Error = [" + query + "])");
                    return -2;
                }
                if (resultData.AffectedRecordCnt != 1)
                {
                    System.Console.WriteLine("[DBManager] UpdateSWRAssociationState( 갱신 결과 오류 : resultData.AffectedRecordCnt=[" + resultData.AffectedRecordCnt + "] )");
                    return 1;
                }

                result = 0;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] UpdateSWRAssociationState( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] UpdateSWRAssociationState( " + ex.ToString() + " )");

                return -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] UpdateSWRAssociationState( end )");
            return result;
        }
        /// <summary>
        /// 기상 특보 기록 중 발령 대기 중인 특보 조회.
        /// </summary>
        /// <returns></returns>
        public int QueryWaitingToOrderSWRList(out List<SWRProfile> waitingList)
        {
            System.Console.WriteLine("[DBManager] QueryWaitingToOrderSWRList( start )");

            int result = 0;
            waitingList = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryWaitingToOrderSWRList( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryWaitingToOrderSWRList( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryWaitingToOrderSWRList( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryWaitingToOrderSWRList( DB Open ERROR )");

                    return -1;
                }

                string query = "SELECT ID, TARGET_AREAS, WARN_KINDCODE, WARN_STRESSCODE, WARN_COMMANDCODE, ORIGINAL_REPORT, RECEIVED_TIME, ASSOCIATION_STATE" +
                                " FROM SWR_PROFILE " +
                                " WHERE ASSOCIATION_STATE = " + ((int)SWRAssociationStateCode.Waiting) +
                                " ORDER BY ID DESC";

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryWaitingToOrderSWRList( DB Open ERROR )");
                    return -2;
                }

                waitingList = new List<SWRProfile>();
                foreach (RowData rowData in resultData.DataList)
                {
                    SWRProfile record = ConvertRowDataToSWRProfile(rowData);
                    if (record != null)
                    {
                        waitingList.Add(record);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryWaitingToOrderSWRList( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryWaitingToOrderSWRList( " + ex.ToString() + " )");

                result = -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryWaitingToOrderSWRList( end )");
            return result;
        }
        private static int CompareSWRProfileByID(SWRProfile profile1, SWRProfile profile2)
        {
            if (profile1 == null)
            {
                if (profile2 == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (profile2 == null)
                {
                    return 1;
                }
                else
                {
                    int id1 = 0;
                    int id2 = 0;
                    if (int.TryParse(profile1.ID, out id1))
                    {
                    }
                    if (int.TryParse(profile2.ID, out id2))
                    {
                    }

                    if (id1 < id2)
                    {
                        return -1;
                    }
                    else if (id1 > id2)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        #endregion

        #region 기상특보연계설정
        /// <summary>
        /// 기상 특보 연계 발령의 연계 설정 정보 조회
        /// </summary>
        /// <returns></returns>
        public List<SWRAssociationCondition> QuerySWRAssociationCondition()
        {
            System.Console.WriteLine("[DBManager] QuerySWRAssociationCondition( start )");

            List<SWRAssociationCondition> conditions = new List<SWRAssociationCondition>();

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QuerySWRAssociationCondition( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QuerySWRAssociationCondition( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QuerySWRAssociationCondition( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QuerySWRAssociationCondition( DB Open ERROR )");

                    return null;
                }

                string query = "SELECT WARNKIND_CODE, WARNSTRESS_CODE, USING_STATE" +
                                " FROM SWR_ASSOCIATION_SETTING";

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QuerySWRAssociationCondition( DB Open ERROR )");
                    return null;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    SWRAssociationCondition record = new SWRAssociationCondition();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    int temp = -1;
                                    if (int.TryParse(value, out temp))
                                    {
                                        record.WarnKindCode = temp;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    int temp = -1;
                                    if (int.TryParse(value, out temp))
                                    {
                                        record.WarnStressCode = temp;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    int temp = -1;
                                    if (int.TryParse(value, out temp))
                                    {
                                        if (temp != 0)
                                        {
                                            record.IsUse = true;
                                        }
                                        else
                                        {
                                            record.IsUse = false;
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    conditions.Add(record);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QuerySWRAssociationCondition( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QuerySWRAssociationCondition( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QuerySWRAssociationCondition( end )");
            return conditions;
        }
        /// <summary>
        /// 기상 특보 연계 발령의 연계 설정 정보 변경
        /// </summary>
        /// <returns></returns>
        public int UpdateSWRAssociationCondition(List<SWRAssociationCondition> newCondition)
        {
            System.Console.WriteLine("[DBManager] UpdateSWRAssociationCondition( start )");

            int result = 0;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] UpdateSWRAssociationCondition( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateSWRAssociationCondition( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] UpdateSWRAssociationCondition( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateSWRAssociationCondition( DB Open ERROR )");

                    return -1;
                }

                foreach (SWRAssociationCondition condition in newCondition)
                {
                    int usingState = (condition.IsUse ? 1 : 0);
                    string query = "UPDATE SWR_ASSOCIATION_SETTING "
                                    + " SET USING_STATE = " + usingState
                                    + " WHERE WARNKIND_CODE = " + condition.WarnKindCode + " AND WARNSTRESS_CODE = " + condition.WarnStressCode;
                    QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                    if (null == resultData)
                    {
                        System.Console.WriteLine("[DBManager] UpdateSWRAssociationCondition( SQL Error = [" + query + "])");
                        return -2;
                    }

                    if (resultData.AffectedRecordCnt == 0)
                    {
                        query = "INSERT INTO SWR_ASSOCIATION_SETTING (WARNKIND_CODE, WARNSTRESS_CODE, USING_STATE) "
                            + " VALUES (" + condition.WarnKindCode + ", " + condition.WarnStressCode + ", " + usingState + " )";
                        resultData = dbConnector.ExecuteQuery(query);
                        if (null == resultData)
                        {
                            System.Console.WriteLine("[DBManager] UpdateSWRAssociationCondition( SQL Error2 = [" + query + "])");
                            return -3;
                        }
                    }

                    if (resultData.AffectedRecordCnt != 1)
                    {
                        System.Console.WriteLine("[DBManager] UpdateSWRAssociationCondition( 갱신 결과 오류 : resultData.AffectedRecordCnt=[" + resultData.AffectedRecordCnt + "] )");
                    }
                }
                result = 0;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] UpdateSWRAssociationCondition( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] UpdateSWRAssociationCondition( " + ex.ToString() + " )");

                return -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] UpdateSWRAssociationCondition( end )");
            return result;
        }
        #endregion


        #region 그룹정보 조회/생성/수정/삭제
        /// <summary>
        /// 지역 그룹 정보 조회.
        /// 입력 파라미터 groupIDs 가 널인 경우에는, 모든 그룹의 정보를 조회
        /// </summary>
        /// <returns></returns>
        public List<GroupProfile> QueryGroupInfo(string groupTypeCode)
        {
            System.Console.WriteLine("[DBManager] QueryGroupInfo( groupTypeCode - start )");

            System.Diagnostics.Debug.Assert(groupTypeCode != null);
            System.Diagnostics.Debug.Assert(groupTypeCode.Length == 1);

            List<GroupProfile> list = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryGroupInfo( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryGroupInfo( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryGroupInfo( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryGroupInfo( DB Open ERROR )");

                    return list;
                }

                string query = "SELECT ID, NAME, TYPECODE, DISASTERCATEGORY_ID, DISASTERKIND_CODE, TARGET, TARGET_SYSTEMKIND" +
                                " FROM GROUPPROFILE WHERE TYPECODE = '" + groupTypeCode + "' ORDER BY ID";

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null || resultData.DataCnt < 1)
                {
                    System.Console.WriteLine("[DBManager] QueryGroupInfo( 그룹 정보 취득 실패 )");

                    return list;
                }

                list = new List<GroupProfile>();
                foreach (RowData rowData in resultData.DataList)
                {
                    GroupProfile record = new GroupProfile();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    record.GroupID = value;
                                }
                                break;
                            case 1:
                                {
                                    record.Name = value;
                                }
                                break;
                            case 2:
                                {
                                    if (value.ToUpper() == "R")
                                    {
                                        record.GroupType = GroupTypeCodes.Region;
                                    }
                                    else if (value.ToUpper() == "S")
                                    {
                                        record.GroupType = GroupTypeCodes.System;
                                    }
                                    else
                                    {
                                        //error
                                    }
                                }
                                break;
                            case 3:
                                {
                                    record.DisasterCategoryID = 0;
                                    int temp = -1;
                                    if (int.TryParse(value, out temp))
                                    {
                                        record.DisasterCategoryID = temp;
                                    }
                                }
                                break;
                            case 4:
                                {
                                    record.DisasterKindCode = value;
                                }
                                break;
                            case 5:
                                {
                                    if (!string.IsNullOrEmpty(value) && value.Length > 0)
                                    {
                                        if (record.Targets == null)
                                        {
                                            record.Targets = new List<string>();
                                        }

                                        string[] spliter = { ",", };
                                        string[] targetRegions = value.Split(spliter, StringSplitOptions.RemoveEmptyEntries);
                                        if (targetRegions != null)
                                        {
                                            record.Targets = targetRegions.ToList<string>();
                                        }
                                    }
                                }
                                break;
                            case 6:
                                {
                                    if (!string.IsNullOrEmpty(value) && value.Length > 0)
                                    {
                                        if (record.TargetSystemKinds == null)
                                        {
                                            record.TargetSystemKinds = new List<string>();
                                        }

                                        string[] spliter = { ",", };
                                        string[] targetKinds = value.Split(spliter, StringSplitOptions.RemoveEmptyEntries);
                                        if (targetKinds != null)
                                        {
                                            record.TargetSystemKinds = targetKinds.ToList<string>();
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    list.Add(record);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryGroupInfo( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryGroupInfo( " + ex.ToString() + " )");

                list = null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryGroupInfo( groupTypeCode - end )");
            return list;
        }
        /// <summary>
        /// 시스템 그룹 정보 조회
        /// 입력 파라미터 groupIDs 가 널인 경우에는, 모든 그룹의 정보를 조회
        /// </summary>
        /// <returns></returns>
        public List<GroupProfile> QuerySASGroupInfo()
        {
            System.Console.WriteLine("[DBManager] QuerySASGroupInfo( start )");

            List<GroupProfile> list = null;

            System.Console.WriteLine("[DBManager] QuerySASGroupInfo( end )");
            return list;
        }
        /// <summary>
        /// 그룹 등록
        /// </summary>
        /// <returns></returns>
        public int RegistGroup(GroupProfile groupInfo)
        {
            System.Console.WriteLine("[DBManager] RegistGroup( start )");

            int result = 0;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] RegistGroup( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] RegistGroup( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] RegistGroup( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] RegistGroup( DB Open ERROR )");

                    return -1;
                }

                // 신규 그룹 프로필의 아이디 취득
                int profileID = 0;

                string query = "SELECT ID" +
                                " FROM ( SELECT ID, ROWNUM FROM GROUPPROFILE ORDER BY ID DESC)" +
                                " WHERE ROWNUM <= 1";
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (null == resultData)
                {
                    System.Console.WriteLine("[DBManager] RegistGroup( SQL Error1 = [" + query + "])");
                    return -2;
                }
                if (resultData.DataCnt == 1)
                {
                    if (resultData.DataList[0].FieldCnt > 0)
                    {
                        bool parse = int.TryParse(resultData.DataList[0].FieldDataList[0], out profileID);
                    }
                }
                profileID++;

                // 신규 등록
                string disasterKindCode = "null";
                if (!string.IsNullOrEmpty(groupInfo.DisasterKindCode))
                {
                    disasterKindCode = "'" + groupInfo.DisasterKindCode + "'";
                }
                string typeCode = groupInfo.GroupType.ToString().Substring(0, 1);
                string systemKindsStr = "null";
                if (groupInfo.GroupType == GroupTypeCodes.Region)
                {
                    if (groupInfo.TargetSystemKinds != null && groupInfo.TargetSystemKinds.Count > 0)
                    {
                        systemKindsStr = "'" + groupInfo.GetTargetSystemKindsString() + "'";
                    }
                }
                query = "INSERT INTO GROUPPROFILE (ID, NAME, TYPECODE, DISASTERCATEGORY_ID, DISASTERKIND_CODE, TARGET, TARGET_SYSTEMKIND) "
                            + " VALUES (" + profileID + ", '" + groupInfo.Name + "', '" + typeCode + "'"
                            + ", " + groupInfo.DisasterCategoryID + ", " + disasterKindCode
                            + ", '" + groupInfo.GetTargetsString() + "', " + systemKindsStr + " )";
                resultData = dbConnector.ExecuteQuery(query);
                if (null == resultData)
                {
                    System.Console.WriteLine("[DBManager] RegistGroup( SQL Error2 = [" + query + "])");
                    return -2;
                }
                if (resultData.AffectedRecordCnt != 1)
                {
                    System.Console.WriteLine("[DBManager] RegistGroup( 갱신 결과 오류 : resultData.AffectedRecordCnt=[" + resultData.AffectedRecordCnt + "] )");
                    return 1;
                }

                result = 0;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] RegistGroup( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] RegistGroup( " + ex.ToString() + " )");

                return -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] RegistGroup( end )");
            return result;
        }
        /// <summary>
        /// 그룹 정보 변경
        /// </summary>
        /// <returns></returns>
        public int UpdateGroupInfo(GroupProfile groupInfo)
        {
            System.Console.WriteLine("[DBManager] UpdateGroupInfo( start )");

            System.Diagnostics.Debug.Assert(groupInfo != null);
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(groupInfo.GroupID));

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] UpdateGroupInfo( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateGroupInfo( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] UpdateGroupInfo( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateGroupInfo( DB Open ERROR )");

                    return -1;
                }

                string disasterKindCode = "null";
                if (!string.IsNullOrEmpty(groupInfo.DisasterKindCode))
                {
                    disasterKindCode = "'" + groupInfo.DisasterKindCode + "'";
                }
                string systemKindsStr = "null";
                if (groupInfo.GroupType == GroupTypeCodes.Region)
                {
                    if (groupInfo.TargetSystemKinds != null && groupInfo.TargetSystemKinds.Count > 0)
                    {
                        systemKindsStr = "'" + groupInfo.GetTargetSystemKindsString() + "'";
                    }
                }

                string query = "UPDATE GROUPPROFILE SET NAME = '" + groupInfo.Name + "'"
                                + ", DISASTERCATEGORY_ID = " + groupInfo.DisasterCategoryID + ", DISASTERKIND_CODE = " + disasterKindCode
                                + ", TARGET = '" + groupInfo.GetTargetsString() + "', TARGET_SYSTEMKIND = " + systemKindsStr
                                + " WHERE ID = " + groupInfo.GroupID;
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    Console.WriteLine("[DBManager] UpdateGroupInfo( DB Open ERROR )");
                    return -2;
                }
                if (resultData.AffectedRecordCnt != 1)
                {
                    System.Console.WriteLine("[DBManager] UpdateGroupInfo( AffectedRecordCnt=[" + resultData.AffectedRecordCnt + "] )");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] UpdateGroupInfo( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] UpdateGroupInfo( " + ex.ToString() + " )");
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] UpdateGroupInfo( end )");
            return 0;
        }
        /// <summary>
        /// 그룹 정보 삭제
        /// </summary>
        /// <returns></returns>
        public int DeleteGroupInfo(string groupID)
        {
            System.Console.WriteLine("[DBManager] DeleteGroupInfo( start )");
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(groupID));

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] DeleteGroupInfo( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] DeleteGroupInfo( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] DeleteGroupInfo( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] DeleteGroupInfo( DB Open ERROR )");

                    return -1;
                }

                string query = "DELETE FROM GROUPPROFILE WHERE ID = " + groupID;
                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] DeleteGroupInfo( SQL Error = [" + query + " )");
                    return -2;
                }
                if (resultData.AffectedRecordCnt != 1)
                {
                    System.Console.WriteLine("[DBManager] DeleteGroupInfo( AffectedRecordCnt=[" + resultData.AffectedRecordCnt + "] )");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] DeleteGroupInfo( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] DeleteGroupInfo( " + ex.ToString() + " )");

                return -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] DeleteGroupInfo( end )");
            return 0;
        }
        #endregion


        #region 기본문안/전송문안
        /// <summary>
        /// 기본 문안 정보 조회
        /// 입력 파라미터가 널인 경우에는, 모든 전송 문안의 정보를 조회
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, DisasterMsgText> QueryBasicMsgTextInfo()
        {
            System.Console.WriteLine("[DBManager] QueryBasicMsgTextInfo( start )");

            Dictionary<string, DisasterMsgText> list = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryBasicMsgTextInfo( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryBasicMsgTextInfo( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryBasicMsgTextInfo( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryBasicMsgTextInfo( DB Open ERROR )");

                    return null;
                }

                string query = "SELECT ID, DISASTERKIND_CODE, DISPLAYMEDIATYPE_ID, LANGUAGEKIND_ID, SUPPORTEDCITYTYPE_ID, MSGTEXT " +
                                "FROM BASIC_MSGTEXTPROFILE ORDER BY ID";

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryBasicMsgTextInfo( SQL Error )");
                    return null;
                }

                list = new Dictionary<string, DisasterMsgText>();

                if (resultData.DataCnt < 1)
                {
                    System.Console.WriteLine("[DBManager] QueryBasicMsgTextInfo( 레코드가 0건 )");
                    return list;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    DisasterMsgText msgInfo = new DisasterMsgText();
                    msgInfo.Initialize();

                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field];
                        switch (field)
                        {
                            case 0:
                                {
                                    if (msgInfo.MsgTxt == null)
                                    {
                                        msgInfo.MsgTxt = new MsgText();
                                    }
                                    msgInfo.MsgTxt.ID = value;
                                }
                                break;
                            case 1:
                                {
                                    if (msgInfo.Disaster == null)
                                    {
                                        msgInfo.Disaster = new Disaster();
                                    }
                                    if (msgInfo.Disaster.Kind == null)
                                    {
                                        msgInfo.Disaster.Kind = new DisasterKind();
                                    }
                                    msgInfo.Disaster.Kind.Code = value;
                                }
                                break;
                            case 2:
                                {
                                    int temp = 0;
                                    if (int.TryParse(value, out temp))
                                    {
                                        msgInfo.MsgTxt.MediaTypeID = temp;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    int temp = 0;
                                    if (int.TryParse(value, out temp))
                                    {
                                        msgInfo.MsgTxt.LanguageKindID = temp;
                                    }
                                    else
                                    {
                                        msgInfo.MsgTxt.LanguageKindID = 0;   // ko-KR
                                    }
                                }
                                break;
                            case 4:
                                {
                                    int temp = 0;
                                    if (int.TryParse(value, out temp))
                                    {
                                        msgInfo.MsgTxt.CityTypeID = temp;
                                    }
                                    else
                                    {
                                        msgInfo.MsgTxt.CityTypeID = 0;   // GENERAL
                                    }
                                }
                                break;
                            case 5:
                                {
                                    msgInfo.MsgTxt.Text = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    list.Add(msgInfo.MsgTxt.ID, msgInfo);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryBasicMsgTextInfo( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryBasicMsgTextInfo( " + ex.ToString() + " )");

                list = null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryBasicMsgTextInfo( end )");

            return list;
        }
        /// <summary>
        /// 전송 문안 정보 조회.
        /// 입력 파라미터가 널인 경우에는, 모든 전송 문안의 정보를 조회.
        /// 단일과 복수, 전체를 구하는 처리를 분리 대응해야 한다.(함수 분리 및 조합 필요)
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, DisasterMsgText> QueryTransmitMsgTextInfo(string msgTextID)
        {
            System.Console.WriteLine("[DBManager] QueryTransmitMsgTextInfo( start )");

            Dictionary<string, DisasterMsgText> list = null;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] QueryTransmitMsgTextInfo( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryTransmitMsgTextInfo( DBConnector 생성 에러 )");

                    return null;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] QueryTransmitMsgTextInfo( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] QueryTransmitMsgTextInfo( DB Open ERROR )");

                    return null;
                }

                string query = null;
                if (msgTextID == null)
                {
                    query = "SELECT ID, DISASTERKIND_CODE, DISPLAYMEDIATYPE_ID, LANGUAGEKIND_ID, SUPPORTEDCITYTYPE_ID, MSGTEXT " +
                                 "FROM TRANSMIT_MSGTEXTPROFILE ORDER BY ID";
                }
                else
                {
                    query = "SELECT ID, DISASTERKIND_CODE, DISPLAYMEDIATYPE_ID, LANGUAGEKIND_ID, SUPPORTEDCITYTYPE_ID, MSGTEXT " +
                                 "FROM TRANSMIT_MSGTEXTPROFILE WHERE ID = '" + msgTextID + "' ORDER BY ID";
                }

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] QueryTransmitMsgTextInfo( SQL Error )");
                    return null;
                }

                list = new Dictionary<string, DisasterMsgText>();

                if (resultData.DataCnt < 1)
                {
                    System.Console.WriteLine("[DBManager] QueryTransmitMsgTextInfo( 레코드가 0건 )");
                    return list;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    DisasterMsgText msgInfo = new DisasterMsgText();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        switch (field)
                        {
                            case 0:
                                {
                                    if (msgInfo.MsgTxt == null)
                                    {
                                        msgInfo.MsgTxt = new MsgText();
                                    }
                                    msgInfo.MsgTxt.ID = rowData.FieldDataList[field];
                                }
                                break;
                            case 1:
                                {
                                    if (msgInfo.Disaster == null)
                                    {
                                        msgInfo.Disaster = new Disaster();
                                    }
                                    if (msgInfo.Disaster.Kind == null)
                                    {
                                        msgInfo.Disaster.Kind = new DisasterKind();
                                    }
                                    msgInfo.Disaster.Kind.Code = rowData.FieldDataList[field];
                                }
                                break;
                            case 2:
                                {
                                    int value = 0;
                                    if (int.TryParse(rowData.FieldDataList[field], out value))
                                    {
                                        msgInfo.MsgTxt.MediaTypeID = value;
                                    }
                                    else
                                    {
                                        msgInfo.MsgTxt.MediaTypeID = -1;   // ?
                                    }
                                }
                                break;
                            case 3:
                                {
                                    int value = 0;
                                    if (int.TryParse(rowData.FieldDataList[field], out value))
                                    {
                                        msgInfo.MsgTxt.LanguageKindID = value;
                                    }
                                    else
                                    {
                                        msgInfo.MsgTxt.LanguageKindID = 0;   // ko-KR
                                    }
                                }
                                break;
                            case 4:
                                {
                                    int value = 0;
                                    if (int.TryParse(rowData.FieldDataList[field], out value))
                                    {
                                        msgInfo.MsgTxt.CityTypeID = value;
                                    }
                                    else
                                    {
                                        msgInfo.MsgTxt.CityTypeID = 0;   // GENERAL
                                    }
                                }
                                break;
                            case 5:
                                {
                                    msgInfo.MsgTxt.Text = rowData.FieldDataList[field];
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    list.Add(msgInfo.MsgTxt.ID, msgInfo);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] QueryTransmitMsgTextInfo( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] QueryTransmitMsgTextInfo( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] QueryTransmitMsgTextInfo( end )");
            return list;
        }
        /// <summary>
        /// 전송 문안 정보 변경
        /// </summary>
        /// <returns></returns>
        public int UpdateTransmitMsgText(List<MsgText> msgTextInfo)
        {
            System.Console.WriteLine("[DBManager] UpdateTransmitMsgText( start )");

            System.Diagnostics.Debug.Assert(msgTextInfo != null);
            if (msgTextInfo == null)
            {
                return -1;
            }

            int result = 0;
            bool isTransaction = false;

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] UpdateTransmitMsgText( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateTransmitMsgText( DBConnector 생성 에러 )");

                    return -80;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] UpdateTransmitMsgText( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] UpdateTransmitMsgText( DB Open ERROR )");

                    result = -1;
                    return result;
                }

                isTransaction = dbConnector.BeginTransaction();
                if (!isTransaction)
                {
                    Console.WriteLine("[DBManager] UpdateTransmitMsgText( 트랜잭션 시작 오류 )");
                    result = -10;
                    return result;
                }

                foreach (MsgText msg in msgTextInfo)
                {
                    string query = "UPDATE TRANSMIT_MSGTEXTPROFILE "
                                    + " SET MSGTEXT = '" + msg.Text + "' "
                                    + " WHERE ID = '" + msg.ID + "'";

                    QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                    if (resultData == null)
                    {
                        Console.WriteLine("[DBManager] UpdateTransmitMsgText( DB Open ERROR )");
                        result = -2;
                        return result;
                    }
                    if (resultData.AffectedRecordCnt != 1)
                    {
                        System.Console.WriteLine("[DBManager] UpdateTransmitMsgText( AffectedRecordCnt=[" + resultData.AffectedRecordCnt + "] )");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] UpdateTransmitMsgText( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] UpdateTransmitMsgText( " + ex.ToString() + " )");

                result = -99;
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    if (isTransaction)
                    {
                        if (result == 0)
                        {
                            dbConnector.EndTransaction(false);
                        }
                        else
                        {
                            dbConnector.EndTransaction(true);
                        }
                    }

                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] UpdateTransmitMsgText( end )");
            return result;
        }
        #endregion

        /// <summary>
        /// 모든 표준경보시스템 프로필의 아이디와 해쉬코드를 취득.
        /// 이 함수는 내부 전용 함수로 데이터베이스 오픈/클로즈는 상위에서 수행해야 함.
        /// </summary>
        /// <param name="profiles"></param>
        /// <returns></returns>
        private bool GetAllSASProfileHashKey(out List<SASProfileHash> profiles)
        {
            System.Console.WriteLine("[DBManager] GetAllSASProfileHashKey( start )");

            bool result = false;
            profiles = new List<SASProfileHash>();

            DBConnector dbConnector = null;
            try
            {
                dbConnector = new DBConnector(this.currentDBSetting);
                if (dbConnector == null)
                {
                    System.Console.WriteLine("[DBManager] GetAllSASProfileHashKey( DBConnector 생성 에러 )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] GetAllSASProfileHashKey( DBConnector 생성 에러 )");

                    return false;
                }

                if (!dbConnector.OpenDB())
                {
                    System.Console.WriteLine("[DBManager] GetAllSASProfileHashKey( DB Open ERROR )");
                    FileLogManager.GetInstance().WriteLog("[DBManager] GetAllSASProfileHashKey( DB Open ERROR )");

                    return false;
                }

                string query = "SELECT ID, NAME, AUTHCODE, SYSTEMKIND, IPTYPE, IPADDRESS, LATITUDE, LONGITUDE, " +
                                "ASSIGNEDIAGWREGIONCODE, ADDRESS, MANAGERNAME, MANAGERDEPARTMENT, MANAGERPHONE " +
                                "FROM SASPROFILE ORDER BY ID";

                QueryResultDataInfo resultData = dbConnector.ExecuteQuery(query);
                if (resultData == null)
                {
                    System.Console.WriteLine("[DBManager] GetAllSASProfileHashKey( 표준경보시스템 정보 취득 실패 )");
                    return false;
                }
                if (resultData.DataCnt < 1)
                {
                    System.Console.WriteLine("[DBManager] GetAllSASProfileHashKey( 표준경보시스템 프로필 레코드 0건 )");
                    return true;
                }

                foreach (RowData rowData in resultData.DataList)
                {
                    SASProfile systemInfo = new SASProfile();
                    for (int field = 0; field < rowData.FieldCnt; field++)
                    {
                        string value = rowData.FieldDataList[field].Trim();
                        switch (field)
                        {
                            case 0:
                                {
                                    systemInfo.ID = value;
                                }
                                break;
                            case 1:
                                {
                                    systemInfo.Name = value;
                                }
                                break;
                            case 2:
                                {
                                    systemInfo.AuthCode = value;
                                }
                                break;
                            case 3:
                                {
                                    systemInfo.KindCode = value;
                                }
                                break;
                            case 4:
                                {
                                    systemInfo.IpType = IPType.IPv4;
                                    IPType temp = IPType.IPv4;
                                    if (Enum.TryParse<IPType>(value, out temp))
                                    {
                                        systemInfo.IpType = temp;
                                    }
                                }
                                break;
                            case 5:
                                {
                                    systemInfo.IpAddress = value;
                                }
                                break;
                            case 6:
                                {
                                    systemInfo.Latitude = value;
                                }
                                break;
                            case 7:
                                {
                                    systemInfo.Longitude = value;
                                }
                                break;
                            case 8:
                                {
                                    systemInfo.AssignedIAGWRegionCode = value;
                                }
                                break;
                            case 9:
                                {
                                    systemInfo.Address = value;
                                }
                                break;
                            case 10:
                                {
                                    systemInfo.ManagerName = value;
                                }
                                break;
                            case 11:
                                {
                                    systemInfo.ManagerDepartment = value;
                                }
                                break;
                            case 12:
                                {
                                    systemInfo.ManagerPhone = value;
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    SASProfileHash profileInfo = new SASProfileHash();
                    profileInfo.ProfileID = systemInfo.ID;
                    profileInfo.HashKey = systemInfo.ComputeHashKey();

                    profiles.Add(profileInfo);
                }

                result = true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] GetAllSASProfileHashKey( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] GetAllSASProfileHashKey( " + ex.ToString() + " )");
            }
            finally
            {
                if (dbConnector != null && dbConnector.IsOpend())
                {
                    dbConnector.CloseDB();
                }
                dbConnector = null;
            }

            System.Console.WriteLine("[DBManager] GetAllSASProfileHashKey( end )");
            return result;
        }
        /// <summary>
        /// 전체 해쉬키 취득
        /// </summary>
        /// <param name="hashCode"></param>
        /// <returns></returns>
        public int GetEntireHashKey(out byte[] hashCode)
        {
            System.Console.WriteLine("[DBManager] GetEntireHashKey (전체 해쉬키 계산 시작)");

            int result = 0;
            hashCode = null;
            List<SASProfileHash> profileList = null;

            try
            {
                bool getResult = GetAllSASProfileHashKey(out profileList);
                if (!getResult)
                {
                    System.Console.WriteLine("[DBManager] GetEntireHashKey( 프로필 정보 취득 실패 )");
                    return -2;
                }

                StringBuilder builder = new StringBuilder();
                foreach (SASProfileHash profile in profileList)
                {
                    for (int index = 0; index < profile.HashKey.Length; index++)
                    {
                        builder.Append(string.Format("{0:X2}", profile.HashKey[index]));
                    }
                }

                byte[] data = Encoding.Default.GetBytes(builder.ToString());
                SHA1CryptoServiceProvider sh1 = new SHA1CryptoServiceProvider();
                hashCode = sh1.ComputeHash(data);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[DBManager] GetEntireHashKey( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[DBManager] GetEntireHashKey( " + ex.ToString() + " )");

                return -99;
            }

            System.Console.WriteLine("[DBManager] GetEntireHashKey (전체 해쉬키=[" + hashCode + "])");
            return result;
        }


        /// <summary>
        /// DateTime 형 데이터의 변환
        /// DateTime 형식의 데이터를 YYYYMMDDhhmmdd (14byte)형식으로 변환한 문자열을 반환한다.
        /// </summary>
        /// <param name="dateTime">DateTime 형 데이터</param>
        /// <returns></returns>
        private string ConvertToLocalDateType(DateTime dateTime)
        {
            System.Console.WriteLine("[DBManager] ConvertToLocalDateType ( start )");

            string result = null;

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(dateTime.Year);

            if (dateTime.Month < 10)
            {
                strBuilder.Append(string.Format("0{0}", dateTime.Month));
            }
            else
            {
                strBuilder.Append(dateTime.Month);
            }

            if (dateTime.Day < 10)
            {
                strBuilder.Append(string.Format("0{0}", dateTime.Day));
            }
            else
            {
                strBuilder.Append(dateTime.Day);
            }

            if (dateTime.Hour < 10)
            {
                strBuilder.Append(string.Format("0{0}", dateTime.Hour));
            }
            else
            {
                strBuilder.Append(dateTime.Hour);
            }

            if (dateTime.Minute < 10)
            {
                strBuilder.Append(string.Format("0{0}", dateTime.Minute));
            }
            else
            {
                strBuilder.Append(dateTime.Minute);
            }

            if (dateTime.Second < 10)
            {
                strBuilder.Append(string.Format("0{0}", dateTime.Second));
            }
            else
            {
                strBuilder.Append(dateTime.Second);
            }

            result = strBuilder.ToString();

            System.Console.WriteLine("[DBManager] ConvertToLocalDateType ( end )");
            return result;
        }
        /// <summary>
        /// 구분 문자열을 포함한 문자열을 스트링 리스트 형식으로 변환.
        /// </summary>
        /// <param name="listString"></param>
        /// <returns></returns>
        private List<string> ConvertToList(string listString, string divisionString)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(listString));
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(divisionString));

            string[] stringSeperators = new string[] { divisionString };
            string[] dividedArray = listString.Split(stringSeperators, StringSplitOptions.RemoveEmptyEntries);
            if (dividedArray == null || dividedArray.Count() <= 0)
            {
                return null;
            }

            List<string> result = dividedArray.ToList();

            return result;
        }

        private SWRProfile ConvertRowDataToSWRProfile(RowData rowData)
        {
            if (rowData == null || rowData.FieldCnt < 1)
            {
                return null;
            }

            SWRProfile record = new SWRProfile();

            for (int field = 0; field < rowData.FieldCnt; field++)
            {
                string value = rowData.FieldDataList[field];
                switch (field)
                {
                    case 0:
                        {
                            record.ID = value;
                        }
                        break;
                    case 1:
                        {
                            record.TargetAreas = value;
                        }
                        break;

                    case 2:
                        {
                            record.WarnKindCode = value;
                        }
                        break;
                    case 3:
                        {
                            record.WarnStressCode = value;
                        }
                        break;
                    case 4:
                        {
                            record.CommandCode = value;
                        }
                        break;
                    case 5:
                        {
                            record.OriginalWarningItemReport = value;
                        }
                        break;
                    case 6:
                        {
                            DateTime temp = new DateTime();
                            if (DateTime.TryParse(value, out temp))
                            {
                                record.ReceivedTime = temp;
                            }
                            else
                            {
                                record.ReceivedTime = new DateTime();
                            }
                        }
                        break;
                    case 7:
                        {
                            record.AssociationState = SWRAssociationStateCode.Waiting;
                            SWRAssociationStateCode temp = SWRAssociationStateCode.Waiting;
                            if (Enum.TryParse<SWRAssociationStateCode>(value, out temp))
                            {
                                record.AssociationState = temp;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return record;
        }
    }

}