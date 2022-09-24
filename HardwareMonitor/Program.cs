using System;
using OpenHardwareMonitor.Hardware;

// Code adapted from: https://performancepsu.com/open-hardware-monitor-source-code-dll-with-c/

namespace HardwareMonitor
{
    internal class Program
    {
       /*
        * Define variables to hold stats
        */

        // CPU Temp
        static float cpuTemp;

        /**
         * Init OpenHardwareMonitor.dll Computer Object
         */
        static Computer c = new Computer()
        {
            CPUEnabled = true,
        };

        /**
         * Pulls data from OHM
         */
        static void ReportSystemInfo()
        {
            foreach (var hardware in c.Hardware)
            {
                if (hardware.HardwareType == HardwareType.CPU)
                {
                    // only fire the update when found
                    hardware.Update();

                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("CPU Package"))
                        {
                            printSensorData(sensor, ref cpuTemp, "cpuTemp (C°)");
                        }
                    }
                }
            }

        }

        private static void printSensorData(ISensor sensor, ref float sensorVar, string label)
        {
            sensorVar = sensor.Value.GetValueOrDefault();
            // print to console
            label += ": ";
            Console.WriteLine(label + sensorVar);
        }

        static void Main(string[] args)
        {
            c.Open();
            while (true)
            {
                ReportSystemInfo();
            }

        }
    }
}
