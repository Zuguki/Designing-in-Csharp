using System;
using System.Collections.Generic;
using System.Linq;

namespace Incapsulation.Failures
{
    public class Common
    {
        public static int Earlier(object[] v, int day, int month, int year) =>
            Earlier(new DateTime((int) v[2], (int) v[1], (int) v[0]), new DateTime(year, month, day));

        public static int Earlier(DateTime perDate, DateTime current) =>
            perDate.Day < current.Day || perDate.Month < current.Month || perDate.Year < current.Year ? 1 : 0;
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
            // ---------------------------------------------------------------->
            var date = new DateTime(year, month, day);
            var failures = failureTypes
                .Select((item, index) => new Failure(item, deviceId[index], 
                    new DateTime((int) times[index][2], (int) times[index][1], (int) times[index][0])))
                .ToList();
            var allDevices = devices
                .Select(device => 
                    new Device(device))
                .ToList();

            return FindDevicesFailedBeforeDateObsolete(date, failures, allDevices);
        }

        public static List<string> FindDevicesFailedBeforeDateObsolete(DateTime date, List<Failure> failures, 
            List<Device> devices)
        {
            var problematicDevices = new HashSet<int>();
            foreach (var failure in failures
                         .Where(failure => Failure.IsFailureSerious(failure.FailureType) == 1 
                                           && Common.Earlier(failure.Date, date) == 1))
                problematicDevices.Add(failure.DeviceId);

            var results = new List<string>();
            foreach (var device in devices)
            {
                if (problematicDevices.Contains((int) device.Items["DeviceId"]))
                    results.Add(device.Items["Name"] as string);
            }

            return results;
        }
    }

    public class Failure
    {
        public int FailureType { get; }
        public int DeviceId { get; }
        public DateTime Date { get; }

        public Failure(int failureType, int deviceId, DateTime date)
        {
            FailureType = failureType;
            DeviceId = deviceId;
            Date = date;
        }

        public static int IsFailureSerious(int failureType) => failureType % 2 == 0 ? 1 : 0;
    }

    public class Device
    {
        public Dictionary<string, object> Items { get; private set; }

        public Device(Dictionary<string, object> items) =>
            Items = items;
    }
}