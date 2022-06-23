using System;
using System.Collections.Generic;
using System.Linq;

namespace Incapsulation.Failures
{
    public class Common
    {
        public static int Earlier(DateTime perDate, DateTime current) => perDate < current ? 1 : 0;
    }

    public class ReportMaker
    {
        public static List<string> FindDevicesFailedBeforeDateObsolete(
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
                var device = new Device(deviceId[index], devices[index]["Name"] as string, 
                    new DateTime((int) times[index][2], (int) times[index][1], (int) times[index][0]));
                if (Device.IsFailureSerious(failureTypes[index]) == 1 && Common.Earlier(device.Date, date) == 1)
                    device.IsFailure = true;
                allDevices.Add(device);
            }

            return FindDevicesFailedBeforeDateObsolete(allDevices);
        }

        public static List<string> FindDevicesFailedBeforeDateObsolete(IEnumerable<Device> devices) => 
            devices.Where(device => device.IsFailure).Select(device => device.Name).ToList();
    }

    public class Device
    {
        public int Id { get; }
        public string Name { get; }
        
        public DateTime Date { get; }
        
        public bool IsFailure { get; set; }

        public Device(int id, string name, DateTime date)
        {
            Id = id;
            Name = name;
            Date = date;
        }
        
        public static int IsFailureSerious(int failureType) => failureType % 2 == 0 ? 1 : 0;
    }
}