using System.Collections.Generic;
using System.Linq;

namespace Incapsulation.Failures
{
    public class Common
    {
        public static int Earlier(object[] v, int day, int month, int year) =>
            Earlier(new Date((int) v[0], (int) v[1], (int) v[2]), new Date(day, month, year));

        public static int Earlier(Date perDate, Date current) =>
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
            var date = new Date(day, month, year);
            var failures = failureTypes
                .Select((item, index) => new Failure(item, deviceId[index], 
                    new Date((int) times[index][0], (int) times[index][1], (int) times[index][2])))
                .ToList();
            var allDevices = devices
                .Select(device => 
                    new Device(device))
                .ToList();

            return FindDevicesFailedBeforeDateObsolete(date, failures, allDevices);
            // ---------------------------------------------------------------->
            // var problematicDevices = new HashSet<int>();
            // for (var i = 0; i < failureTypes.Length; i++)
            //     if (Common.IsFailureSerious(failureTypes[i]) == 1 && Common.Earlier(times[i], day, month, year) == 1)
            //         problematicDevices.Add(deviceId[i]);
            //
            // var result = new List<string>();
            // foreach (var device in devices)
            //     if (problematicDevices.Contains((int) device["DeviceId"]))
            //         result.Add(device["Name"] as string);
            //
            // return result;
        }

        public static List<string> FindDevicesFailedBeforeDateObsolete(Date date, List<Failure> failures, 
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

    public class Date
    {
        public int Day
        {
            get => _day;
            set => _day = CheckDate(value, 0, 31);
        }

        public int Month
        {
            get => _month;
            set => _month = CheckDate(value, 1, 12);
        }

        public int Year
        {
            get => _year;
            set => _year = value;
        }
        
        private int _day;
        private int _month;
        private int _year;

        public Date(int day, int month, int year)
        {
            Day = day;
            Month = month;
            Year = year;
        }

        private int CheckDate(int value, int lowerBound, int upperBound)
        {
            if (value < lowerBound)
                return lowerBound;

            return value > upperBound ? upperBound : value;
        }
    }

    public class Failure
    {
        public int FailureType { get; }
        public int DeviceId { get; }
        public Date Date { get; }

        public Failure(int failureType, int deviceId, Date date)
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