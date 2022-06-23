using System;
using System.Collections.Generic;
using System.Linq;

namespace Incapsulation.Failures
{
    public class Common
    {
        public static int WasEarlier(DateTime perDate, DateTime current) => perDate < current ? 1 : 0;
    }

    public class ReportMaker
    {
        public static IEnumerable<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes,
            int[] deviceId,
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            var date = new DateTime(year, month, day);
            var allDevices = new List<Device>();
            for (var index = 0; index < failureTypes.Length; index++)
            {
                var device = new Device(devices[index]["Name"] as string, 
                    new DateTime((int) times[index][2], (int) times[index][1], (int) times[index][0]));
                device.SetFailureType(failureTypes[index], date);
                
                allDevices.Add(device);
            }

            return FindDevicesFailedBeforeDate(date, allDevices);
        }

        public static IEnumerable<string> FindDevicesFailedBeforeDate(DateTime dateTime, IEnumerable<Device> devices)
        {
            return devices.Where(device => device.FailureType is FailureType.Success).Select(device => device.Name)
                .ToList();
        }
    }

    public class Device
    {
        public string Name { get; }

        public FailureType FailureType { get; private set; }
        
        private DateTime Date { get; }

        public Device(string name, DateTime date)
        {
            Name = name;
            Date = date;
        }

        public void SetFailureType(int failureType, DateTime currentDate) =>
            FailureType = IsFailureSerious(failureType) == 1 && Common.WasEarlier(Date, currentDate) == 1
                ? FailureType.Success
                : FailureType.Unsuccessful;
        
        private static int IsFailureSerious(int failureType) => failureType % 2 == 0 ? 1 : 0;
    }

    public enum FailureType
    {
        Success,
        Unsuccessful
    }
}