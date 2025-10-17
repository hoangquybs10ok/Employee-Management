namespace EmployeeManagement.Service
{
    public interface ILateTimeService
    {
        /// <summary>
        /// Gửi email báo cáo giờ làm việc của nhân viên ngày hôm qua
        /// </summary>
        Task SendYesterdayTimeLogReportAsync();
    }
}
