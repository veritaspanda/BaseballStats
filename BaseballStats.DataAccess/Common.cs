using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using BaseballStats.Common;
using System.IO;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;
using System.Data;

namespace BaseballStats.DataAccess
{
    public class Common
    {
        public static System.Data.DataTable TestGetPlayerStats()
        {
            System.Data.DataTable returnTable = new System.Data.DataTable();
            string _responseString = SendRequest("PLAYERSEARCH", "Y", "cespedes%25");

            Common dt = new Common();
            returnTable = dt.JsonToDataTable(_responseString);
            return returnTable;
        }

        public static System.Data.DataTable PromptedGetPlayerStats(string _firstName, string _lastName)
        {
            System.Data.DataTable returnTable = new System.Data.DataTable();
            string _responseString = string.Empty;

            if (_firstName != null && _firstName != "" && _firstName != "NOTVALID")
            {
                string _playerNameStr = _firstName + " " + _lastName;
                _responseString = SendRequest("PLAYERSEARCH", "Y", _playerNameStr);
            }
            else
            {
                string _playerNameStr = _lastName;
                _responseString = SendRequest("PLAYERSEARCH", "Y", _playerNameStr);
            }

            Common dt = new Common();
            returnTable = dt.JsonToDataTable(_responseString);
            return returnTable;
        }

        public static string SendRequest(string _requestId, string _param1, string _param2 = "", string _param3 = "")
        {
            WebResponse response = null;
            int _pageNumber = 1;
            int _totPageNum = 1;
            string _finalResponseString = string.Empty;
            string _urlSuffix = string.Empty;
            //string _decodedString = "";
            //dynamic _responseJobject = null;
            //dynamic _AggregateResponseObjet = null;
            //JObject _AggJobject = null;

            ServicePointManager.Expect100Continue = false;
            ServicePointManager.MaxServicePointIdleTime = 0;

            if(_requestId.ToUpper() == "PLAYERSEARCH")
            {
                _urlSuffix = Constants.funcReplacePlayerSearchValues(Constants.ApiPlayerSearchUrl, _param1, _param2);
            }
            else if(_requestId.ToUpper() == "PLAYERINFO")
            {
                _urlSuffix = Constants.funcReplacePlayerInfoValues(Constants.ApiPlayerInfoUrl, _param1);
            }
            else if (_requestId.ToUpper() == "SEASONHITTINGSTATS")
            {
                _urlSuffix = Constants.funcReplaceSeasonHittingStatsValues(Constants.ApiSeasonHittingStatsUrl, _param1, _param2, _param3);
            }
            else
            {
                _urlSuffix = Constants.funcReplacePlayerSearchValues(Constants.ApiPlayerSearchUrl, _param1, _param2);
            }

            string _apiUrl = GetEndpointUrl(_urlSuffix);

            for(int i = 1; i<= _totPageNum; i++)
            {
                HttpWebRequest request = null;
                if(i == 1)
                {
                    request = (HttpWebRequest)WebRequest.Create(_apiUrl);
                }
                else if(i == 5 || i == 10 || i == 15 || i == 20) //sleep option for ever five pages or diff logic example
                {
                    //System.Threading.Thread.Sleep(5000); //sleep 5 seconds
                    //string _newUrl = Constants.Link_Api_Url_WithPages;
                    //_newUrl = Constants.funcReplacePageNum(_newUrl, _pageNumber.ToString());
                    //request = (HttpWebRequest)WebRequest.Create(_newUrl);
                    string _tempCatch1 = string.Empty;
                }
                else
                {
                    //string _newUrl = Constants.Link_Api_Url_WithPages;
                    //_newUrl = Constants.funcReplacePageNum(_newUrl, _pageNumber.ToString());
                    //request = (HttpWebRequest)WebRequest.Create(_newUrl);
                    string _tempCatch1 = string.Empty;
                }

                try
                {
                    //get header info example
                    //request.Headers["Token"] = _apiKey.ToString();

                    request.Timeout = Convert.ToInt32(TimeSpan.FromMinutes(double.Parse("5")).TotalMilliseconds); //1800000 = 30 minutes

                    response = (HttpWebResponse)request.GetResponse();
                }
                catch(WebException ex)
                {
                    WebResponse errorResponse = ex.Response;
                    if(ex.Response != null && errorResponse != null)
                    {
                        using (Stream responseStream = errorResponse.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                            //TODO: add logging logic
                            //Logger.error(ex.Message);
                            //logger.error(reader.ReadToEnd());
                        }
                    }
                }

                if(response != null)
                {
                    try
                    {
                        //Get stream associated with response
                        Stream receiveStream = response.GetResponseStream();

                        //Pipes the stream to higher level stream reader with required encoding
                        StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                        string _responseString = readStream.ReadToEnd();
                        //_finalResponseString = _responseString;

                        //_responseString = _responseString.Replace("\"", "");
                        dynamic _responseDynamic = JsonConvert.DeserializeObject(_responseString);
                        _responseString = Convert.ToString(_responseDynamic);
                        //_responseString = JsonConvert.SerializeObject(_responseDynamic, Newtonsoft.Json.Formatting.Indented);
                        //int pFrom = _responseString.IndexOf("row:") + "row:".Length;
                        int pFrom = _responseString.IndexOf("row\":") + "row\":".Length;
                        //int pFrom = _responseString.IndexOf("{") + "{".Length;
                        //int pFrom = _responseString.IndexOf("row:") + "row:".Length;
                        int pTo = _responseString.LastIndexOf("}") + "}".Length;
                        //int pTo = _responseString.IndexOf("}") + "}".Length;
                        int pToCalc = 2;
                        if (pTo - pFrom <= 0)
                        {
                            pToCalc = 1;
                        }
                        else
                        {
                            pToCalc = pTo - pFrom;
                        }

                        if (_responseString.Length > 1)
                        {
                            //JObject jsonObj = JObject.Parse(_responseString);
                            //////jsonObj = (JObject)_responseString;
                            //////JArray rows = (JArray)jsonObj.SelectToken("row");
                            /*var rows = jsonObj.SelectTokens("search_player_all.queryResults[*].row[*]")
                                .Select(o => o.First)
                                .Cast<JProperty>()
                                .Select(o => o.Name);*/
                            //int _rowsCount = rows.Count();
                            //foreach (JToken row in rows)
                            //{
                                _responseString = _responseString.Substring(pFrom, pToCalc);
                                _responseString = _responseString.Replace("}", "");
                                //_responseString = _responseString.Replace("}}}", "");
                                //_responseString = _responseString.Replace("}}", "");
                                //_responseString = _responseString.Replace(_responseString.LastIndexOf("}"), "");
                                _responseString = _responseString.Replace("\r\n", "");
                                _responseString = _responseString.Replace(" ", "");
                                StringBuilder updatedResponseStr = new StringBuilder();
                                //string _startJson = "\"Output\": [";
                                ////_startJson = _startJson.Replace("\"", "");
                                //updatedResponseStr.Append(_startJson);
                                updatedResponseStr.Append("[");
                                updatedResponseStr.Append(_responseString);
                                updatedResponseStr.Append("}");
                                updatedResponseStr.Append("]");
                                //updatedResponseStr.Append("}");
                                dynamic parsedString = JsonConvert.DeserializeObject(updatedResponseStr.ToString());
                                _finalResponseString = JsonConvert.SerializeObject(parsedString, Newtonsoft.Json.Formatting.Indented);
                                //_finalResponseString = updatedResponseStr.ToString();
                                //_finalResponseString = _responseString;
                                ////_finalResponseString = Convert.ToString(_responseDynamic);
                            //}
                        }
                        //_finalResponseString = _responseString;

                        //_finalResponseString = GlobalSettings.Base64Encode(_responseString);
                        //_responseJobject = JObject.Parse(_responseString); //to use if need to parse page numbers
                        //ClassName.Tot_Page_Num = Convert.ToInt32(_responseJobject.total_pages);
                        //_totPageNum = ClassName.Tot_Page_Num;

                        //for return as JObject with multi page merge
                        /*JObject _rJobject = JObject.Parse(_responseString);

                        if(_pageNumber == 1)
                        {
                            _AggJobject = _rJobject;
                        }
                        else
                        {
                            _AggJobject.Merge(_rJobject, new JsonMergeSettings
                            {
                                //union to avoid duplicates
                                MergeArrayHandling = MergeArrayHandling.Union
                            });
                        }*/

                        response.Close();
                        readStream.Close();

                        _pageNumber++;


                        //Below if using custom based 64 enoded response
                        /*
                        //Get stream associated with response
                        Stream receiveStream = response.GetResponseStream();

                        //Pipes the stream to higher level stream reader with required encoding
                        StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                        string _responseString = readStream.ReadToEnd();
                        _responseString = _responseString.Replace("\"", "");
                        int pFrom = _responseString.IndexOf("row:") + "row:".Length;
                        int pTo = _responseString.LastIndexOf("}") + "}".Length;
                        int pToCalc = 2;
                        if(pTo - pFrom <=0)
                        {
                            pToCalc = 1;
                        }
                        else
                        {
                            pToCalc = pTo - pFrom;
                        }

                        if(_responseString.Length > 1)
                        {
                            _responseString = _responseString.Substring(pFrom, pToCalc);
                            _responseString = _responseString.Replace("}", "");
                            _decodedString = GlobalSettings.Base64Decode(_responseString);
                        }

                        response.Close();
                        readStream.Close();

                        _pageNumber++;
                        */
                    }
                    catch(Exception ex)
                    {
                        //TODO: add logger info
                        //Logger.errror(string.Format("Nothing returned for query {0}", _runSummaryParam));
                        //logger.info(ex.Message.ToString());
                        string _tempError = ex.Message.ToString();
                        string _anotherTemp = _tempError;
                    }

                }
            }
            return _finalResponseString;

        }

        public static string GetEndpointUrl(string UrlSuffix)
        {
            string _apiUrl = string.Empty;
            StringBuilder endpointUrl = new StringBuilder();
            _apiUrl = string.Format(Constants.ApiEndpointUrl.ToString(), UrlSuffix.ToString());
            endpointUrl.Append(_apiUrl.ToString());
            return endpointUrl.ToString();
        }

        public System.Data.DataTable JsonToDataTable(string _jsonToConvert)
        {
            System.Data.DataTable jsonResponseTable = new System.Data.DataTable();
            try
            {
                jsonResponseTable = (System.Data.DataTable)JsonConvert.DeserializeObject(_jsonToConvert, (typeof(System.Data.DataTable)));
                //jsonResponseTable = JsonConvert.DeserializeAnonymousType(_jsonToConvert, new { Makes = default(System.Data.DataTable) }).Makes;

                //XmlDocument xd1 = new XmlDocument();
                //xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(_jsonToConvert, "root");
                //DataSet jsonDataSet = new DataSet();
                //jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                //jsonResponseTable = jsonDataSet.Tables[0];

            }
            catch (Exception ex)
            {
                //TODO: add logger logic
                //Logger.info(ex.Message.ToString());
                string _tempError = ex.Message.ToString();
                string _anotherTemp = _tempError;
            }
            return jsonResponseTable;
        }
        
    }
}