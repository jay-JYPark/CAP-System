using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using GEPlugin;

using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    [ComVisibleAttribute(true)]
    public class GEController
    {
        #region Fields
        private IGEPlugin ge = null;
        private Dictionary<EColorStyle, IKmlStyleMap> dicStyleMap = new Dictionary<EColorStyle, IKmlStyleMap>();
        #endregion
        #region Properties
        public IGEPlugin Ge
        {
            get { return ge; }
            set { ge = value; }
        }
        #endregion
        #region Delegate
        #endregion
        #region Event
        #endregion

        #region PlugIn Method
        /// <summary>
        /// GEPlugin Visibility 변경
        /// </summary>
        /// <param name="isVisible"></param>
        public void SetPluginVisibility(bool isVisible)
        {
            try
            {
                if (this.ge == null)
                {
                    FileLogManager.GetInstance().WriteLog("[GEController] SetPluginVisibility( GEPlugin is null. )");

                    throw new Exception("External Exception : GEPlugin is null.");
                }

                if (isVisible)
                {
                    ge.getWindow().setVisibility(ge.VISIBILITY_SHOW);
                }
                else
                {
                    ge.getWindow().setVisibility(ge.VISIBILITY_HIDE);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("ShowPlugin Exception : " + ex.ToString());
                FileLogManager.GetInstance().WriteLog("[GEController] SetPluginVisibility( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// NavigationControl Visibility 변경
        /// </summary>
        /// <param name="isVisible">true : Auto, false : Hide</param>
        public void SetNavigationControlVisibility(bool isVisible)
        {
            try
            {
                if (this.ge == null)
                {
                    FileLogManager.GetInstance().WriteLog("[GEController] SetNavigationControlVisibility( GEPlugin is null. )");

                    throw new Exception("External Exception : GEPlugin is null.");
                }

                if (isVisible)
                {
                    ge.getNavigationControl().setVisibility(ge.VISIBILITY_AUTO);
                }
                else
                {
                    ge.getNavigationControl().setVisibility(ge.VISIBILITY_HIDE);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("SetNavigationControlVisibility Exception : " + ex.ToString());
                FileLogManager.GetInstance().WriteLog("[GEController] SetNavigationControlVisibility( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// 영역 구분선 Visibility 변경
        /// </summary>
        /// <param name="isVisible"></param>
        public void SetBordersVisibility(bool isVisible)
        {
            try
            {
                if (this.ge == null)
                {
                    FileLogManager.GetInstance().WriteLog("[GEController] SetBordersVisibility( GEPlugin is null. )");

                    throw new Exception("External Exception : GEPlugin is null.");
                }

                if (isVisible)
                {
                    ge.getLayerRoot().enableLayerById(ge.LAYER_BORDERS, 1);
                }
                else
                {
                    ge.getLayerRoot().enableLayerById(ge.LAYER_BORDERS, 0);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("SetBordersVisibility Exception : " + ex.ToString());
                FileLogManager.GetInstance().WriteLog("[GEController] SetBordersVisibility( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// 도로 Visibility 변경
        /// </summary>
        /// <param name="isVisible"></param>
        public void SetRoadsVisibility(bool isVisible)
        {
            try
            {
                if (this.ge == null)
                {
                    FileLogManager.GetInstance().WriteLog("[GEController] SetRoadsVisibility( GEPlugin is null. )");

                    throw new Exception("External Exception : GEPlugin is null.");
                }

                if (isVisible)
                {
                    ge.getLayerRoot().enableLayerById(ge.LAYER_ROADS, 1);
                }
                else
                {
                    ge.getLayerRoot().enableLayerById(ge.LAYER_ROADS, 0);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("SetRoadsVisibility Exception : " + ex.ToString());
                FileLogManager.GetInstance().WriteLog("[GEController] SetRoadsVisibility( " + ex.ToString() + " )");
            }
        }

        /// <summary>
        /// Icon을 생성하여 GEPlugin 에 Append
        /// </summary>
        /// <param name="placemarkID"></param>
        /// <param name="iconName"></param>
        /// <param name="iconURL"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public void CreateSystemIcon(string placemarkID, string iconName, string iconUrl, double latitude, double longitude)
        {
            try
            {
                if (this.ge == null)
                {
                    FileLogManager.GetInstance().WriteLog("[GEController] CreateSystemIcon( GEPlugin is null. )");

                    throw new Exception("External Exception : GEPlugin is null.");
                }

                //아이콘 생성---------------------------------------시작
                IKmlPlacemark placemark = ge.createPlacemark("icon" + placemarkID);
                placemark.setDescription(iconName);
                if (!string.IsNullOrEmpty(iconUrl))
                {
                    //아이콘 스타일 변경----------------------------시작
                    IKmlIcon icon = ge.createIcon("");
                    icon.setHref(iconUrl);
                    IKmlStyle style = ge.createStyle("");
                    style.getIconStyle().setIcon(icon);
                    placemark.setStyleSelector(style);
                    //아이콘 스타일 변경------------------------------끝
                }
                else
                {
                    //아이콘 스타일 변경----------------------------시작
                    IKmlIcon icon = ge.createIcon("");
                    icon.setHref("http://maps.google.com/mapfiles/kml/paddle/red-circle.png");
                    IKmlStyle style = ge.createStyle("");
                    style.getIconStyle().setIcon(icon);
                    placemark.setStyleSelector(style);
                    //아이콘 스타일 변경------------------------------끝
                }
                IKmlPoint point = ge.createPoint("");
                point.setLatitude(latitude);
                point.setLongitude(longitude);
                placemark.setGeometry(point);
                ge.getFeatures().appendChild(placemark);
                //아이콘 생성-----------------------------------------끝
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("CreateSystemIcon Exception : " + ex.ToString());
                FileLogManager.GetInstance().WriteLog("[GEController] CreateSystemIcon( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// Icon을 생성하여 GEPlugin 에 Append
        /// </summary>
        /// <param name="iconInfo"></param>
        public IKmlObject CreateSystemIcon(IconInfo iconInfo)
        {
            try
            {
                if (this.ge == null)
                {
                    FileLogManager.GetInstance().WriteLog("[GEController] CreateSystemIcon2( GEPlugin is null. )");

                    throw new Exception("External Exception : GEPlugin is null.");
                }

                StringBuilder builder = new StringBuilder();
                builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                builder.Append("<kml xmlns=\"http://www.opengis.net/kml/2.2\" xmlns:gx=\"http://www.google.com/kml/ext/2.2\"");
                builder.Append(" xmlns:kml=\"http://www.opengis.net/kml/2.2\" xmlns:atom=\"http://www.w3.org/2005/Atom\">");
                builder.Append("<Placemark id=\"");
                builder.Append("icon" + iconInfo.IconName);
                builder.Append("\">");
                builder.Append("<name></name>");
                if (iconInfo.LstExtendedData.Count > 0)
                {
                    builder.Append("<ExtendedData>");
                    foreach (KmlExtendedData extendData in iconInfo.LstExtendedData)
                    {
                        builder.Append("<Data name=\"");
                        builder.Append(extendData.DataName);
                        builder.Append("\">");
                        builder.Append("<value>");
                        builder.Append(extendData.Data);
                        builder.Append("</value>");
                        builder.Append("</Data>");
                    }
                    builder.Append("</ExtendedData>");
                }

                builder.Append("</Placemark>");
                builder.Append("</kml>");
                IKmlObject obj = ge.parseKml(builder.ToString());
                //아이콘 생성---------------------------------------시작
                IKmlPlacemark placemark = obj as IKmlPlacemark;
                placemark.setDescription(iconInfo.IconName);
                if (!string.IsNullOrEmpty(iconInfo.IconURL))
                {
                    //아이콘 스타일 변경----------------------------시작
                    IKmlIcon icon = ge.createIcon("");
                    icon.setHref(iconInfo.IconURL);
                    IKmlStyle style = ge.createStyle("");
                    style.getIconStyle().setIcon(icon);
                    placemark.setStyleSelector(style);
                    //아이콘 스타일 변경------------------------------끝
                }
                else
                {
                    //아이콘 스타일 변경----------------------------시작
                    IKmlIcon icon = ge.createIcon("");
                    icon.setHref("http://maps.google.com/mapfiles/kml/paddle/red-circle.png");
                    IKmlStyle style = ge.createStyle("");
                    style.getIconStyle().setIcon(icon);
                    placemark.setStyleSelector(style);
                    //아이콘 스타일 변경------------------------------끝
                }
                IKmlPoint point = ge.createPoint("");
                point.setLatitude(iconInfo.Latitude);
                point.setLongitude(iconInfo.Longitude);
                placemark.setGeometry(point);

                return ge.getFeatures().appendChild(placemark);
                //아이콘 생성-----------------------------------------끝
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("CreateSystemIcon2 Exception : " + ex.ToString());
                FileLogManager.GetInstance().WriteLog("[GEController] CreateSystemIcon2( GEPlugin is null. )");

                return null;
            }
        }
        public void SetIconUrl(IKmlPlacemark placemark,string iconUrl, bool isSelect)
        {
            try
            {
                if (this.ge == null)
                {
                    FileLogManager.GetInstance().WriteLog("[GEController] SetIconUrl( GEPlugin is null. )");

                    throw new Exception("External Exception : GEPlugin is null.");
                }

                string url = iconUrl;
                //아이콘 스타일 변경----------------------------시작
                IKmlIcon icon = ge.createIcon("");
                if (isSelect)
                {
                    if (!url.Contains("_preferences"))
                    {
                        url = url.Split('.')[0] + "_preferences." + url.Split('.')[1];
                    }
                }
                else
                {
                    if (url.Contains("_preferences"))
                    {
                        url = url.Replace("_preferences", "");
                    }
                }
                icon.setHref(url);
                IKmlStyle style = ge.createStyle("");
                style.getIconStyle().setIcon(icon);
                placemark.setStyleSelector(style);
                //아이콘 스타일 변경------------------------------끝
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("SetIconUrl Exception : " + ex.ToString());
                FileLogManager.GetInstance().WriteLog("[GEController] SetIconUrl( " + ex.ToString() + " )");
            }
            
        }
        /// <summary>
        /// 지정한 위 경도로 이동
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="altitude"></param>
        /// <param name="speed"></param>
        public void MoveFlyTo(double latitude, double longitude, double altitude, double speed)
        {
            try
            {
                if (this.ge == null)
                {
                    FileLogManager.GetInstance().WriteLog("[GEController] MoveFlyTo( GEPlugin is null. )");

                    throw new Exception("External Exception : GEPlugin is null.");
                }

                //맵 이동------------------------------------------시작
                double prevFlyToSpeed = ge.getOptions().getFlyToSpeed();
                ge.getOptions().setFlyToSpeed(speed);
                IKmlCamera camera = ge.getView().copyAsCamera(ge.ALTITUDE_RELATIVE_TO_GROUND);
                camera.setAltitude(altitude);
                camera.setLatitude(latitude);
                camera.setLongitude(longitude);
                camera.setHeading(360);
                ge.getView().setAbstractView(camera);
                ge.getOptions().setFlyToSpeed(prevFlyToSpeed);
                //맵 이동--------------------------------------------끝
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("MoveFlyTo Exception : " + ex.ToString());
                FileLogManager.GetInstance().WriteLog("[GEController] MoveFlyTo( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// 지정한 위 경도로 이동
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="altitude"></param>
        /// <param name="tilt"></param>
        /// <param name="speed"></param>
        public void MoveFlyTo(double latitude, double longitude, double altitude, double tilt, double range, double speed)
        {
            try
            {
                if (this.ge == null)
                {
                    FileLogManager.GetInstance().WriteLog("[GEController] MoveFlyTo2( GEPlugin is null. )");

                    throw new Exception("External Exception : GEPlugin is null.");
                }

                //맵 이동------------------------------------------시작
                double prevFlyToSpeed = ge.getOptions().getFlyToSpeed();
                ge.getOptions().setFlyToSpeed(speed);

                //IKmlCamera camera = ge.getView().copyAsCamera(ge.ALTITUDE_RELATIVE_TO_GROUND);
                //camera.setAltitude(altitude);
                //camera.setLatitude(latitude);
                //camera.setLongitude(longitude);
                //camera.setHeading(360);
                //camera.setTilt(tilt);
                //ge.getView().setAbstractView(camera);

                IKmlLookAt lookAt = ge.getView().copyAsLookAt(ge.ALTITUDE_RELATIVE_TO_GROUND);
                lookAt.setLatitude(latitude);
                lookAt.setLongitude(longitude);
                lookAt.setAltitude(altitude);
                lookAt.setHeading(0.0);
                lookAt.setTilt(tilt);
                lookAt.setRange(range);
                ge.getView().setAbstractView(lookAt);

                ge.getOptions().setFlyToSpeed(prevFlyToSpeed);
                //맵 이동--------------------------------------------끝
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("MoveFlyTo Exception : " + ex.ToString());
                FileLogManager.GetInstance().WriteLog("[GEController] MoveFlyTo2( " + ex.ToString() + " )");
            }
        }

        /// <summary>
        /// Kml 을 parse 하여 GEPlugin 에 로드
        /// </summary>
        /// <param name="kml"></param>
        /// <returns></returns>
        public IKmlObject AppendKml(string kml)
        {
            try
            {
                if (this.ge == null)
                {
                    FileLogManager.GetInstance().WriteLog("[GEController] AppendKml( GEPlugin is null )");

                    throw new Exception("External Exception : GEPlugin is null.");
                }

                IKmlObject obj = ge.parseKml(kml.Trim());
                return ge.getFeatures().appendChild(obj);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("AppendKml Exception : " + ex.ToString());
                FileLogManager.GetInstance().WriteLog("[GEController] AppendKml( " + ex.ToString() + " )");

                return null;
            }
        }
        public bool RemoveKml(IKmlObject obj)
        {
            try
            {
                if (this.ge == null)
                {
                    FileLogManager.GetInstance().WriteLog("[GEController] RemoveKml( GEPlugin is null )");

                    throw new Exception("External Exception : GEPlugin is null.");
                }

                if (ge.getFeatures() == null)
                {
                    return false;
                }
                ge.getFeatures().removeChild(obj);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("RemoveKml Exception : " + ex.ToString());
                FileLogManager.GetInstance().WriteLog("[GEController] RemoveKml( " + ex.ToString() + " )");

                return false;
            }

            return true;
        }

        /// <summary>
        /// KmlDocument 의 Visibility 변경
        /// </summary>
        /// <param name="document"></param>
        /// <param name="isVisible"></param>
        public void SetKmlDocumentVisible(IKmlDocument document,bool isVisible)
        {
            if (document == null)
            {
                System.Console.WriteLine("SetKmlDocumentVisible - KmlDocument is null");
                FileLogManager.GetInstance().WriteLog("[GEController] SetKmlDocumentVisible( KmlDocument is null )");

                return;
            }

            try
            {
                document.setVisibility(Convert.ToInt32(isVisible));

                KmlObjectListCoClass kmlObjectListCoClass = document.getFeatures().getChildNodes();
                for (int i = 0; i < kmlObjectListCoClass.getLength(); i++)
                {
                    IKmlObject kmlObject = kmlObjectListCoClass.item(i);
                    string strType = kmlObject.getType();
                    if (strType == "KmlPlacemark")
                    {
                        IKmlPlacemark placemark = kmlObject as IKmlPlacemark;
                        placemark.setVisibility(Convert.ToInt32(isVisible));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("SetKmlDocumentVisible Exception : " + ex.ToString());
                FileLogManager.GetInstance().WriteLog("[GEController] SetKmlDocumentVisible( " + ex.ToString() + " )");
            }
        }

        /// <summary>
        /// 인자로 수신한 정보로 스타일 생성
        /// </summary>
        /// <param name="styleName"></param>
        /// <param name="iconScale"></param>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private IKmlStyle MakeStyle(string styleName, float iconScale, int a, int r, int g, int b)
        {
            IKmlStyle style = ge.createStyle(styleName);
            IKmlIconStyle iconStyle = style.getIconStyle();
            iconStyle.setScale(iconScale);
            IKmlIcon icon = iconStyle.getIcon();
            icon.setHref("http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png");
            IKmlVec2 hotSpot = iconStyle.getHotSpot();
            hotSpot.setX(20);
            hotSpot.setY(2);
            IKmlLineStyle lineStyle = style.getLineStyle();
            /*IKmlColor lineColor = lineStyle.getColor();
            lineColor.setA(255);
            lineColor.setR(0);
            lineColor.setG(0);
            lineColor.setB(0);*/
            lineStyle.setWidth(2F);
            IKmlPolyStyle polyStyle = style.getPolyStyle();
            IKmlColor color = polyStyle.getColor();
            color.setA(a);
            color.setR(r);
            color.setG(g);
            color.setB(b);
            return style;
        }
        /// <summary>
        /// 모든 스타일 맵 생성
        /// </summary>
        public void MakeAllStyleMap()
        {
            #region 흰색
            IKmlStyle styleNormal = MakeStyle("st_n_white", 1.1F, 20, 255, 255, 255);
            IKmlStyle styleHighlight = MakeStyle("st_h_white", 1.3F, 20, 255, 255, 255);
            IKmlStyleMap styleMap = ge.createStyleMap("stm_white");
            styleMap.setStyle(styleNormal, styleHighlight);
            dicStyleMap.Add(EColorStyle.WHITE, styleMap);
            #endregion
            #region 빨강
            styleNormal = MakeStyle("st_n_red", 1.1F, 127, 255, 0, 0);
            styleHighlight = MakeStyle("st_h_red", 1.3F, 127, 255, 0, 0);
            styleMap = ge.createStyleMap("stm_red");
            styleMap.setStyle(styleNormal, styleHighlight);
            dicStyleMap.Add(EColorStyle.RED, styleMap);
            #endregion
            #region 파랑
            styleNormal = MakeStyle("st_n_blue", 1.1F, 127, 0, 0, 255);
            styleHighlight = MakeStyle("st_h_blue", 1.3F, 127, 0, 0, 255);
            styleMap = ge.createStyleMap("stm_blue");
            styleMap.setStyle(styleNormal, styleHighlight);
            dicStyleMap.Add(EColorStyle.BLUE, styleMap);
            #endregion
            #region PALEGOLDENROD
            styleNormal = MakeStyle("st_n_palegoldenrod", 1.1F, 127, 210, 206, 152);
            styleHighlight = MakeStyle("st_h_alegoldenrod", 1.3F, 127, 210, 206, 152);
            styleMap = ge.createStyleMap("stm_alegoldenrod");
            styleMap.setStyle(styleNormal, styleHighlight);
            dicStyleMap.Add(EColorStyle.PALEGOLDENROD, styleMap);
            #endregion
            #region GOLD
            styleNormal = MakeStyle("st_n_gold", 1.1F, 127, 254, 224, 78);
            styleHighlight = MakeStyle("st_h_gold", 1.3F, 127, 254, 224, 78);
            styleMap = ge.createStyleMap("stm_gold");
            styleMap.setStyle(styleNormal, styleHighlight);
            dicStyleMap.Add(EColorStyle.GOLD, styleMap);
            #endregion
            #region ORANGE
            styleNormal = MakeStyle("st_n_orange", 1.1F, 127, 255, 125, 00);
            styleHighlight = MakeStyle("st_h_orange", 1.3F, 127, 255, 125, 00);
            styleMap = ge.createStyleMap("stm_orange");
            styleMap.setStyle(styleNormal, styleHighlight);
            dicStyleMap.Add(EColorStyle.ORANGE, styleMap);
            #endregion
            #region BRIGHTRED
            styleNormal = MakeStyle("st_n_brightred", 1.1F, 127, 255, 52, 52);
            styleHighlight = MakeStyle("st_h_brightred", 1.3F, 127, 255, 52, 52);
            styleMap = ge.createStyleMap("stm_brightred");
            styleMap.setStyle(styleNormal, styleHighlight);
            dicStyleMap.Add(EColorStyle.BRIGHTRED, styleMap);
            #endregion
            #region DEEPPINK
            styleNormal = MakeStyle("st_n_deeppink", 1.1F, 127, 232, 09, 142);
            styleHighlight = MakeStyle("st_h_deeppink", 1.3F, 127, 232, 09, 142);
            styleMap = ge.createStyleMap("stm_deeppink");
            styleMap.setStyle(styleNormal, styleHighlight);
            dicStyleMap.Add(EColorStyle.DEEPPINK, styleMap);
            #endregion
            #region PURPLE
            styleNormal = MakeStyle("st_n_purple", 1.1F, 127, 151, 28, 147);
            styleHighlight = MakeStyle("st_h_purple", 1.3F, 127, 151, 28, 147);
            styleMap = ge.createStyleMap("stm_purple");
            styleMap.setStyle(styleNormal, styleHighlight);
            dicStyleMap.Add(EColorStyle.PURPLE, styleMap);
            #endregion
            #region ROYALBLUE
            styleNormal = MakeStyle("st_n_royalblue", 1.1F, 127, 59, 94, 225);
            styleHighlight = MakeStyle("st_h_royalblue", 1.3F, 127, 59, 94, 225);
            styleMap = ge.createStyleMap("stm_royalblue");
            styleMap.setStyle(styleNormal, styleHighlight);
            dicStyleMap.Add(EColorStyle.ROYALBLUE, styleMap);
            #endregion
            #region DODGERBLUE
            styleNormal = MakeStyle("st_n_dodgerblue", 1.1F, 127, 59, 150, 227);
            styleHighlight = MakeStyle("st_h_dodgerblue", 1.3F, 127, 59, 150, 227);
            styleMap = ge.createStyleMap("stm_dodgerblue");
            styleMap.setStyle(styleNormal, styleHighlight);
            dicStyleMap.Add(EColorStyle.DODGERBLUE, styleMap);
            #endregion
            #region MEDIUMTURQUOISE
            styleNormal = MakeStyle("st_n_mediumturquoise", 1.1F, 127, 96, 208, 218);
            styleHighlight = MakeStyle("st_h_mediumturquoise", 1.3F, 127, 96, 208, 218);
            styleMap = ge.createStyleMap("stm_mediumturquoise");
            styleMap.setStyle(styleNormal, styleHighlight);
            dicStyleMap.Add(EColorStyle.MEDIUMTURQUOISE, styleMap);
            #endregion
            #region LIMEGREEN
            styleNormal = MakeStyle("st_n_limegreen", 1.1F, 127, 103, 206, 62);
            styleHighlight = MakeStyle("st_h_limegreen", 1.3F, 127, 103, 206, 62);
            styleMap = ge.createStyleMap("stm_limegreen");
            styleMap.setStyle(styleNormal, styleHighlight);
            dicStyleMap.Add(EColorStyle.LIMEGREEN, styleMap);
            #endregion


        }
        /// <summary>
        /// 스타일 맵 Get
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public IKmlStyleMap GetStyleMap(EColorStyle color)
        {
            if (!dicStyleMap.ContainsKey(color))
            {
                return null;
            }
            return dicStyleMap[color];
        }
        #endregion

        #region 플레이스마크 충돌 체크
        /// <summary>
        /// 입력 받은 두 선분이 서로 교차하는지 검사한다.
        /// 원본 소스: http://blog.naver.com/rldk2002/110186449056
        /// </summary>
        /// <param name="AB"></param>
        /// <param name="CD"></param>
        /// <returns></returns>
        public bool IsLineCrossed(TLine AB, TLine CD)
        {
            // 두 선분의 교차 조건 ( 1 & 2 )
            // 1. Direction(A,B,C) × Direction(A,B,D) ≦ 0
            // 2. Direction(C,D,A) × Direction(C,D,B) ≦ 0

            bool ret = false;

            if ((AB.p1.X == AB.p2.X && AB.p1.Y == AB.p2.Y) && (CD.p1.X == CD.p2.X && CD.p1.Y == CD.p2.Y) &&
                ((AB.p1.X != CD.p1.X) || (AB.p1.Y != CD.p1.Y)))
            {
                // AB와 CD가 모두 점인 경우로, 완전히 일치하는 점이 아닌 경우에는 교차가 아닌 것으로 판단.
                return false;
            }

            if ((GetLineDirection(AB.p1, AB.p2, CD.p1) * GetLineDirection(AB.p1, AB.p2, CD.p2) <= 0) &&
                (GetLineDirection(CD.p1, CD.p2, AB.p1) * GetLineDirection(CD.p1, CD.p2, AB.p2) <= 0))
            {
                //FileLogManager.GetInstance().WriteLog(
                //                "AB=[p1(" + AB.p1.X + "," + AB.p1.Y + "), p2(" + AB.p2.X + "," + AB.p2.Y + ")], " +
                //                "CD=[p1(" + CD.p1.X + "," + CD.p1.Y + "), p2(" + CD.p2.X + "," + CD.p2.Y + ")]");
                ret = true;
            }
            return ret;
        }
        /// <summary>
        /// 선분 AB를 기준으로 선분 CD의 점을 이었을 때, 꺽은 선의 방향을 조사한다.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public int GetLineDirection(Point A, Point B, Point C)
        {
            // 점 A를 여섯시 방향, 점 B를 중점에 두고 봤을 때, 점 B와 이어지는 점 C가 어느 방향에 있는지 체크한다.
            // 1.ABC가 일직선 상에 있지 않을 때,
            //     중점 B와 이어지는 C가 오른쪽에 있으면 시계 방향(1)을 반환한다.
            //     중점 B와 이어지는 C가 왼쪽에 있으면 시계 반대 방향(-1)을 반환한다.
            // 2.ABC가 일직선 상에 있을 때,
            //     C가 가운데 있는 경우, 0을 반환한다.
            //     C가 A나 B와 동일 점의 경우, 0을 반환한다.
            //     B가 가운데 있는 경우, 1을 반환한다.
            //     A가 가운데 있는 경우, -1을 반환한다.

            int dir = 0;
            int dxAB = B.X - A.X;
            int dyAB = B.Y - A.Y;
            int dxAC = C.X - A.X;
            int dyAC = C.Y - A.Y;

            if (dxAB * dyAC < dyAB * dxAC)
            {
                dir = 1;    // 시계 방향
            }
            if (dxAB * dyAC > dyAB * dxAC)
            {
                dir = -1;    // 반시계 방향
            }
            if (dxAB * dyAC == dyAB * dxAC)
            {
                if (dxAB == 0 && dyAB == 0)
                {
                    dir = 0;    /* A = B */
                }
                else if ((dxAB * dxAC < 0) || (dyAB * dyAC < 0))
                {
                    dir = -1;    /* A가 가운데 */
                }
                else if ((dxAB * dxAB + dyAB * dyAB >= dxAC * dxAC + dyAC * dyAC))
                {
                    dir = 0;  /* C가 가운데 */
                }
                else
                {
                    dir = 1;   /* B가 가운데 */
                }
            }
            return dir;
        }

        /// <summary>
        /// 입력받은 점이 입력 원의 내부에 포함되는 점인지 검사한다.
        /// ------------------------------------------------------------------------------------------------
        /// ※표준발령대에서는, 먼저 IsLineCrossed()를 수행한 후에 그 결과가 부정일 경우 이 함수를 수행한다.
        ///   이 경우, 모든 선이 서로 교차하지 않는 경우는 완전 내포와 완전 외포인 경우 뿐이므로,
        ///   다각형의 한 점에 대해서만 내포인지 검사를 수행하면 여부를 판단할 수 있다.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public bool IsPointInsideTheCircle(Point point, Point center, int radius)
        {
            // 각 포인트와 원의 중점과의 거리를 도출하여, 
            // 그 거리가 원의 반지름보다 작다면 원의 내부에 존재한다.
            // a = 절대값(points[i].x - center.x)
            // b = 절대값(points[i].y - center.y)
            // c = 루트(a제곱 + b제곱)

            bool isInclusion = false;

            int disX = Math.Abs(point.X - center.X);
            int disY = Math.Abs(point.Y - center.Y);
            double disZ = Math.Pow((Math.Pow(disX, 2) + Math.Pow(disY, 2)), 0.5);

            if (disZ <= Convert.ToDouble(radius))
            {
                isInclusion = true;
            }

            return isInclusion;
        }

        /// <summary>
        /// 입력 폴리곤이 입력 원에 내포되는지 검사한다.
        /// 이 함수는 현재 표준발령대에서 사용하지 않는 바, 충분한 검증을 거치지 않았으므로 사용시 주의 바람.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public bool IsPolygonInsideTheCircle(List<Point> points, Point center, int radius)
        {
            bool isInclusion = false;
            int inCount = 0;

            // 각 포인트와 원의 중점과의 거리를 도출하여, 
            // 그 거리가 원의 반지름보다 작으면 다각형은 원의 내부에 존재한다.
            // 하나라도 원의 반지름보다 작으면 원과 걸쳐져 있고, 
            // 전부 다 작다면 원의 내부에 포함되는(내포) 폴리곤이다.
            // a = 절대값(points[i].x - center.x)
            // b = 절대값(points[i].y - center.y)
            // c = 루트(a제곱 + b제곱)

            double dblRadius = Convert.ToDouble(radius);
            for (int index = 0; index < 1; index++)
            {
                int disX = Math.Abs(points[index].X - center.X);
                int disY = Math.Abs(points[index].Y - center.Y);
                double disZ = Math.Pow((Math.Pow(disX, 2) + Math.Pow(disY, 2)), 0.5);

                if (disZ <= dblRadius)
                {
                    isInclusion = true;
                    inCount++;
                }
                else
                {
                    isInclusion = false;
                    break;
                }
            }

            return isInclusion;
        }

        /// <summary>
        /// 입력받은 좌표가 폴리곤의 내부에 있는지 외부에 있는지 검사한다.
        /// ------------------------------------------------------------------------------------------------
        /// ※표준발령대에서는, IsLineCrossed() => IsPointInsideTheCircle() => 본 함수 순으로 수행하므로,
        ///   이와 같은 순차 검사 외에 단독 수행 시 문제가 없는 지에 대해서는 충분한 검증이 이루어지지 않았으므로 주의 바람.
        /// </summary>
        /// <param name="polygonPoints"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        public bool IsPointInsideThePolygon(List<Point> polygonPoints, Point center)
        {
            bool isInclusion = false;

            // 1. 기준 좌표를 중심으로 위쪽 또는 아래쪽 한 방향만 검사를 수행한다.
            // 2. 검사 방향에 대해서는 첫 좌표(실제로는 맨 끝 좌표)를 기준으로 판단한다.
            // 3. 기준 좌표의 X 점을 가로지르는 폴리곤의 선분 갯수를 구한다.
            // 4. 완전히 원의 중심 X와 동일한 경우 중심을 벗어나는 좌표가 발견될 때까지 보류(0으로 처리 == 미발견)
            // 5. 다각형의 처음 좌표(실제로는 맨 끝 좌표)가 기준 좌표와 동일한 경우, 먼저는 위의 4.에 따르고
            //    맨 마지막 처리에서 조건에 따라 검출수를 증가시킨다.

            if (polygonPoints.Count < 3)
            {
                System.Console.WriteLine("Is Not POLYGON. Check count of point: "+polygonPoints.Count.ToString());
                return false;
            }

            bool isUpperSide = true; // 1: 상반구, 2: 하반구
            int beforeX = polygonPoints[polygonPoints.Count - 1].X;
            int beforeY = polygonPoints[polygonPoints.Count - 1].Y;

            if (beforeY < center.Y)
            {
                isUpperSide = false;
            }
            //System.Console.WriteLine("center(x,y): {0},{1}", center.X, center.Y);

            int count = 0;
            int polygonCount = polygonPoints.Count;
            for (int index = polygonCount - 1; index >= 0; index--)
            {
                if (polygonPoints[index].X == center.X)
                {
                    polygonPoints.RemoveAt(index);
                }
            }
            polygonCount = polygonPoints.Count;
            for (int index = 0; index < polygonCount; index++)
            {
                Point current = polygonPoints[index];
                if (isUpperSide && current.Y < center.Y)
                {
                    beforeX = current.X;
                    continue;
                }
                if (!isUpperSide && current.Y >= center.Y)
                {
                    beforeX = current.X;
                    continue;
                }

                if (beforeX == current.X)
                {
                    continue;
                }

                if (
                    (beforeX < center.X && center.X < current.X) ||
                    (current.X < center.X && center.X < beforeX))
                {
                    count++;
                    System.Console.WriteLine("beforeX, current(x,y), count: " + beforeX + "\t(" + current.X + ", " + current.Y + ")\t" + count);
                }

                //System.Console.WriteLine("beforeX, polygonPoints(x,y), count: " + beforeX + "\t(" + current.X + ", " + current.Y + ")\t" + count);
                beforeX = current.X;
            }
            //System.Console.WriteLine("교차 횟수: " + count);
            if (count > 0)
            {
                if (0 != (count % 2))
                {
                    isInclusion = true; // 홀수면 도형 내부에 존재하는 점
                }
            }

            return isInclusion;
        }
        #endregion
    }

    public class TLine
    {
        public Point p1;
        public Point p2;
    };
}
