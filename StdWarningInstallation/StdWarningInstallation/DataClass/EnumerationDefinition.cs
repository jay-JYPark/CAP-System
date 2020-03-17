using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IEASProtocol;

namespace StdWarningInstallation.DataClass
{
    /// <summary>
    /// 프로그램 색상 정의
    /// </summary>
    public enum EColorStyle
    {
        WHITE,
        BLUE,
        RED,
        YELLOW,
        GREEN,
        BLACK,
        PINK,
        PALEGOLDENROD,  // 해일
        GOLD,           // 황사
        ORANGE,         // 건조
        BRIGHTRED,      // 태풍
        DEEPPINK,       // 대설
        PURPLE,         // 폭염
        ROYALBLUE,      // 호우
        DODGERBLUE,     // 한파
        MEDIUMTURQUOISE,    // 풍량
        LIMEGREEN,      // 강풍
    }

    /// <summary>
    /// 송신자 종류 (IEASProtocol과 내부처리를 분리하기 위해 별도 정의).
    /// 원본 데이터: IEASProtocol.IEASSenderType
    /// </summary>
    public enum SenderTypes
    {
        NONE = IEASProtocol.IEASSenderType.None,
        SWI = IEASProtocol.IEASSenderType.SWI,
        IAGW = IEASProtocol.IEASSenderType.IAGW,
        SAS = IEASProtocol.IEASSenderType.SAS,
        SASU,
    }
    /// <summary>
    /// 프로필 갱신 모드
    /// </summary>
    public enum ProfileUpdateMode
    {
        Regist = 1,
        Modify,
        Delete,
    }

    public enum OrderReferenceType
    {
        /// <summary>
        /// 참조 없음 (통상 발령)
        /// </summary>
        None = 0,
        /// <summary>
        /// 기상특보연계
        /// </summary>
        SWR,
        /// <summary>
        /// 발령 갱신(2차년도 현재 미사용)
        /// </summary>
        //Update,
        /// <summary>
        /// 발령 취소 (잘못 내린 발령의 취소)
        /// </summary>
        Cancel = 3,
        /// <summary>
        /// 경보 해제 (경보 상황의 해제에 대한 명시적인 알림)
        /// </summary>
        Clear,
    }

    public enum OrderLocationKind
    {
        /// <summary>
        /// 미등록 타입
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// 자체 발령
        /// </summary>
        Local = 0,
        /// <summary>
        /// 동일 지역 및 상위 지역 발령
        /// </summary>
        Other,
    }

    public enum IPType
    {
        IPv4 = 0,
        IPv6,
    }

    /// <summary>
    /// 경보 해제 상태
    /// </summary>
    public enum ClearAlertState
    {
        /// <summary>
        /// 해제 대기
        /// </summary>
        Waiting = 0,
        /// <summary>
        /// 해제 완료
        /// </summary>
        Clear,
        /// <summary>
        /// 해제 제외(해제 불필요)
        /// </summary>
        Exclude,
    }

    /// <summary>
    /// 기상특보 발령 연계 상태
    /// </summary>
    public enum SWRAssociationStateCode
    {
        /// <summary>
        /// 연계 대기
        /// </summary>
        Waiting = 0,
        /// <summary>
        /// 연계 발령 완료
        /// </summary>
        Order,
        /// <summary>
        /// 연계 제외 (비연계)
        /// </summary>
        Exclude,
    }

    /// <summary>
    /// 그룹 유형 (발령 그룹)
    /// </summary>
    public enum GroupTypeCodes
    {
        /// <summary>
        /// 지역 그룹
        /// </summary>
        Region = 0,
        /// <summary>
        /// 시스템 그룹
        /// </summary>
        System,
    }

    public enum OrderResponseCodes
    {
        /// <summary>
        /// 메시지 수신 확인.
        /// (통합경보게이트웨이 -> 표준발령대)
        /// </summary>
        IagwReceiveOK = 0,
        /// <summary>
        /// 메시지 수신 오류.
        /// (통합경보게이트웨이 -> 표준발령대)
        /// </summary>
        IagwReceiveError = 10,

        /// <summary>
        /// 통합경보게이트웨이의 메시지 유효성 확인.
        /// (통합경보게이트웨이 -> 표준발령대)
        /// </summary>
        IagwMsgValidationOK = 200,
        /// <summary>
        /// 통합경보게이트웨이의 메시지 유효성 오류.
        /// (통합경보게이트웨이 -> 표준발령대)
        /// </summary>
        IagwMsgValidationError = 210,
        /// <summary>
        /// 통합경보게이트웨이의 프로파일 해석 오류.
        /// (통합경보게이트웨이 -> 표준발령대)
        /// </summary>
        IagwProfileParsingError = 220,

        /// <summary>
        /// 메시지 중복 확인.
        /// (통합경보게이트웨이 -> 표준발령대)
        /// </summary>
        IagwMsgDuplication = 300,

        /// <summary>
        /// 통합경보게이트웨이의 메시지 인증 확인.
        /// (통합경보게이트웨이 -> 표준발령대)
        /// </summary>
        IagwMsgCertificationOK = 400,
        /// <summary>
        /// 통합경보게이트웨이의 메시지 인증 오류.
        /// (통합경보게이트웨이 -> 표준발령대)
        /// </summary>
        IagwMsgCertificationNG = 410,

        /// <summary>
        /// 표준경보시스템의 메시지 유효성 오류.
        /// (표준경보시스템 -> 통합경보게이트웨이)
        /// </summary>
        SasWMsgValidationError = 510,
        /// <summary>
        /// 표준경보시스템의 프로파일 해석 오류.
        /// (표준경보시스템 -> 통합경보게이트웨이)
        /// </summary>
        SasProfileParsingError = 520,

        /// <summary>
        /// 리소스 인증 확인 성공.
        /// (표준경보시스템 -> 통합경보게이트웨이)
        /// </summary>
        SasResourceCerciticationOK = 600,
        /// <summary>
        /// 리소스 인증 오류.
        /// (표준경보시스템 -> 통합경보게이트웨이)
        /// </summary>
        SasResourceCerciticationNG = 610,

        /// <summary>
        /// 경보 서비스 확인.
        /// (표준경보시스템 -> 통합경보게이트웨이)
        /// </summary>
        SasAlertServiceOK = 800,
        /// <summary>
        /// 경보 서비스 오류.
        /// (표준경보시스템 -> 통합경보게이트웨이)
        /// </summary>
        SasAlertServiceError = 810,

    }

    /// <summary>
    /// 지역 상대적 수준(상위/중위/하위)
    /// 중앙의 경우, 전국/시도/시군구.
    /// 시도의 경우, 시도/시군구/읍면동.
    /// 시군의 경우, 시군구/읍면동.
    /// </summary>
    public enum RelativeRegionLevel
    {
        Low = -1,
        Middle,
        High,
    }

    /// <summary>
    /// 수신 데이터 CRC16 체크 결과
    /// </summary>
    public enum DataValidationCheckResult
    {
        Success = 0,
        Crc16Error = 1,
        PacketLoss = 2,
    }




    public enum ResultCode
    {
        Success = 0,

    }
    public enum ProgramErrorCode
    {
        ExceptionOccured = -99,

        Success = 0,
        DBConnectorError = -1,
        DBOpenError = -2,
        DBSQLError = -3,
        DBResultError = -4,


    }
}
