using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EzTvWatcher.Code
{
    public interface IServicePersistence
    {
        DateTimeOffset ReadLastProcessedDate();
        List<string> ReadWatchList();
        void WriteLastProcessedDate(DateTimeOffset dt);

        string GetEmailTo();
        string GetSendGridApiKey();
    }

    public class ServiceGoogleSheet : IDisposable, IServicePersistence
    {

        private readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
        private readonly string _applicationName;
        private readonly string _spreadsheetId;
        private readonly string _sheet;
        private SheetsService _service;
        private ServiceLogger _log;
      


        public ServiceGoogleSheet(GoogleApiConfig config, ServiceLogger log)
        {
            this._log = log;
            GoogleCredential credential;
            string credentialsAsJson = JsonConvert.SerializeObject(config);
                credential = GoogleCredential.FromJson(credentialsAsJson)
                    .CreateScoped(_scopes);

            this._spreadsheetId = config.SpreadsheetId;
            this._sheet = config.Sheet;
            this._applicationName = config.ApplicationName;
            // Create Google Sheets API service.
            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            });
        }
        public DateTimeOffset ReadLastProcessedDate()
        {
            var range = $"{_sheet}!B2";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    _service.Spreadsheets.Values.Get(_spreadsheetId, range);

            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count == 1)
            {
                var row = values[0];
                string result = row[0].ToString();
                DateTimeOffset dt;
                if (DateTimeOffset.TryParse(result,out dt))
                {
                    _log.Debug($"last processed date = {dt}");
                    return dt;
                }
                else
                {
                    _log.Error($"unable to parse {result}");
                    return DateTimeOffset.Now;
                }
            }
            _log.Error("last processed date not found");
            return DateTime.MaxValue;
        }

        public void WriteLastProcessedDate(DateTimeOffset dt)
        {

            var range = $"{_sheet}!B2";
            var valueRange = new ValueRange();

            var oblist = new List<object>() { dt.ToString() };
            valueRange.Values = new List<IList<object>> { oblist };

            var updateRequest = _service.Spreadsheets.Values.Update(valueRange, _spreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = updateRequest.Execute();
        }

        public List<String> ReadWatchList()
        {
            List<String> result = new List<string>();
            var range = $"{_sheet}!C:D";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    _service.Spreadsheets.Values.Get(_spreadsheetId, range);

            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    string value = row[0].ToString();
                    result.Add(value);
                }
            }
            _log.Debug($"watching a list of {result.Count} entries");
            return result;
        }

        public String GetEmailTo()
        {
            string result = getCellValue($"{_sheet}!B3");
           
            return result;
        }
        public String GetRssFeedUrl()
        {
            string result = getCellValue($"{_sheet}!B5");

            return result;
        }

        public String GetSendGridApiKey()
        {
            string result = getCellValue($"{_sheet}!B4");

            return result;
        }
        private string getCellValue(string range)
        {
            string result = string.Empty;
            SpreadsheetsResource.ValuesResource.GetRequest request =
                   _service.Spreadsheets.Values.Get(_spreadsheetId, range);

            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count == 1)
            {
                var row = values[0];
                result = row[0].ToString();

            }
            
            return result;
        }
        public void Dispose()
        {
            if (this._service != null)
            {
                this._service.Dispose();
            }
        }
    }
}
