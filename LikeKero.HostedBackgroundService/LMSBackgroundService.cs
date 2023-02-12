using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LikeKero.HostedBackgroundService.RefitClientFactory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Refit;

namespace LikeKero.HostedBackgroundService
{
    public class LMSBackgroundService : IHostedService, IDisposable
    {
         

        public string FilePath { get; set; }
        private IOptions<LMSBackGroundServiceConfiguration> _options;

        public LMSBackgroundService(IOptions<LMSBackGroundServiceConfiguration> options)
        {
            _options = options;
            var rootDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            FilePath = Path.Combine(rootDir.Replace(_options.Value.ReplaceLogFile, string.Empty), _options.Value.LogFileName);

        }

        private Timer _timer;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.WriteLog("LMSService Started");
            await this.ScheduleServiceAsync();

            //_timer = new Timer(
            //    (e) => WriteLog("Service Started"),
            //    null,
            //    TimeSpan.Zero,
            //    TimeSpan.FromMinutes(1));

            await Task.CompletedTask;
        }


        public async Task ScheduleServiceAsync()
        {
            try
            {
                _timer = new Timer(async o => await SchedularCallbackAsync(o));
                
                string mode = _options.Value.Mode;
                this.WriteLog("Mode: " + mode + " {0}");


                #region Check if Scheduler setting is to tun DAILY or after INTERVAL

                //Set the Default Time.
                DateTime scheduledTime = DateTime.MinValue;

                if (mode.ToUpper().TrimEnd() == "DAILY")
                {
                    //Get the Scheduled Time from AppSettings.
                    scheduledTime = DateTime.Parse(_options.Value.ScheduledTime);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next day.
                        scheduledTime = scheduledTime.AddDays(1);
                    }
                }

                if (mode.ToUpper().Trim() == "INTERVAL")
                {
                    //Get the Interval in Minutes from AppSettings.
                    int intervalMinutes = Convert.ToInt32(_options.Value.IntervalMinutes);

                    //Set the Scheduled Time by adding the Interval to Current Time.
                    scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next Interval.
                        scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                    }
                }

                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                this.WriteLog("Service to run after: " + schedule + " {0}");

                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

                //Change the Timer's Due Time.
                _timer.Change(dueTime, Timeout.Infinite);

                #endregion
            }
            catch (Exception ex)
            {
                WriteLog("Service Error on: {0} " + Convert.ToString(ex.Message) + Convert.ToString(ex.StackTrace) + Convert.ToString(ex.InnerException));

                //Stop the Windows Service.
                using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("LMSService"))
                {
                    serviceController.Stop();
                }
            }
        }

        private async Task SchedularCallbackAsync(object e)
        {
            try
            {
                
                var apiKey = _options.Value.apiKey;
                var apiCalls = RestService.For<IAPICalls>(hostUrl: _options.Value.APIURL);
                var apiResponse = await apiCalls.SendPendingEmails(apiKey);
                var response = apiResponse.Content;               

                this.WriteLog("Pending Email Executed");
                this.WriteLog(apiResponse.Content.ReadAsStringAsync().Result);


                var apiCallsBulk = RestService.For<IAPICalls>(hostUrl: _options.Value.APIURL);
                var apiResponseBulk = await apiCallsBulk.DoPendingBulkImport(apiKey);
                var responseBulk = apiResponseBulk.Content;

                this.WriteLog("Bulk Import Executed");
                this.WriteLog(apiResponseBulk.Content.ReadAsStringAsync().Result);

                var apiCallsBR = RestService.For<IAPICalls>(hostUrl: _options.Value.APIURL);
                var apiResponseBR = await apiCallsBR.RunBRRUles(apiKey);
                var responseBR = apiResponseBR.Content;

                this.WriteLog("Business Rule Executed");
                this.WriteLog(apiResponseBulk.Content.ReadAsStringAsync().Result);


                var apiCallsOIG = RestService.For<IAPICalls>(hostUrl: _options.Value.APIURL);
                var apiResponseOIG = await apiCallsOIG.BulkImportOIGFiles(apiKey);
                var responseOIG = apiResponseOIG.Content;

                this.WriteLog("OIG File User Executed");
                this.WriteLog(apiResponseOIG.Content.ReadAsStringAsync().Result);

                var apiCallsVendor = RestService.For<IAPICalls>(hostUrl: _options.Value.APIURL);
                var apiResponseVendor = await apiCallsVendor.BulkImportVendorFiles(apiKey);
                var responseVendor = apiResponseVendor.Content;

                this.WriteLog("OIG File Vendor Executed");
                this.WriteLog(apiResponseVendor.Content.ReadAsStringAsync().Result);

                var apiCallsBRFiles = RestService.For<IAPICalls>(hostUrl: _options.Value.APIURL);
                var apiResponseBRFiles = await apiCallsBRFiles.ProcessPendingBusinessRuleFiles(apiKey);
                var responseBRFiles = apiResponseBRFiles.Content;

                this.WriteLog("OIG File Vendor Executed");
                this.WriteLog(apiResponseBRFiles.Content.ReadAsStringAsync().Result);


                await this.ScheduleServiceAsync();
            }
            catch (Exception ex)
            {

                WriteLog(Convert.ToString(ex.Message) + Convert.ToString(ex.StackTrace) + Convert.ToString(ex.InnerException));
                await this.ScheduleServiceAsync();
            }
        }

        public void WriteLog(string Message)
        {
            if (!File.Exists(FilePath))
            {
                using (var sw = File.CreateText(FilePath))
                {
                    sw.WriteLine(Message + " -  " + DateTime.UtcNow.ToString("O"));
                }
            }
            else
            {
                using (var sw = File.AppendText(FilePath))
                {
                    sw.WriteLine(Message + " -  " + DateTime.UtcNow.ToString("O"));
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            WriteLog("LMSService Stoped");
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            WriteLog("LMSService Disposed");
            _timer?.Dispose();
        }
    }
}
